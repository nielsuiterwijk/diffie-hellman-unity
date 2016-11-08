using System;
using System.Threading;

/*
    Threadsafe RingBuffer
*/
public class RingBuffer<T>
{
	private int size;
	private T[] data;

	private volatile int writePosition;
	private volatile int readPosition;
	private volatile int maxReadPosition;

	public int Length { get { return size; } }

	public T[] Data
	{
		get { return data; }
	}

	public RingBuffer(int size)
	{
		if (size <= 1)
		{
			throw new ArgumentException("RingBuffer size must be 2 or greater.");
		}

		this.size = size;

		data = new T[this.size];

		writePosition = 0;
		readPosition = 0;
		maxReadPosition = 0;
	}

	public void Enqueue(T value)
	{
		int currentWriteIndex = 0;

		do
		{
			//Note: primitives are guaranteed to be atomic reads.
			currentWriteIndex = writePosition;
		}
		while (Interlocked.CompareExchange(ref writePosition, currentWriteIndex + 1, currentWriteIndex) != currentWriteIndex);

		int currentReadIndex = readPosition;

		// In case of over taking the read position, move the read position forward. This will work if a 2nd
		// producer thread is also moving the read position forward, CAS saving the day.
		while (currentWriteIndex - currentReadIndex  >= size)
		{
			//The result does not actually matter, because we'll read it anyway.
			Interlocked.CompareExchange(ref readPosition, currentReadIndex + 1, currentReadIndex);
			currentReadIndex = readPosition;
		}

		data[currentWriteIndex % size] = value;

		// update the maximum read index after saving the data. It wouldn't fail if there is only one thread
		// inserting in the queue. It might fail if there are more than 1 producer threads because this
		// operation has to be done in the same order as the previous CAS

		while (Interlocked.CompareExchange(ref maxReadPosition, currentWriteIndex + 1, currentWriteIndex) != currentWriteIndex)
		{
			// SpinWait will yield if needed, and we know we dont have to wait long.
			// Could use Thread.Sleep(0) (force a context switch), but this provides better performance.
			Thread.SpinWait(10);
		}

	}

	public bool Dequeue(out T item)
	{
		// First check if we can actualy read, if not, don't bother. If we can, read the item, and if we
		// could increment the read position, we are the only one's reading this. If a producer or another
		// consumer incremented it, we will just start over again. Worst case you keep failing and eventually
		// returning false. It's possible that producers keep moving the read position forward, effectively causing
		// this to be an endless loop.
		do
		{
			int currentReadPosition = readPosition;
			int currentMaxReadPosition = maxReadPosition;

			if (currentReadPosition == currentMaxReadPosition)
			{
				item = default(T);
				return false;
			}

			item = data[currentReadPosition % size];

			if (Interlocked.CompareExchange(ref readPosition, currentReadPosition + 1, currentReadPosition) == currentReadPosition)
			{
				//Note that this assignment can actually overwrite a correctly set value.
				//data[currentReadPosition % size] = default(T);
				return true;
			}

		}
		while (true);
	}

}

