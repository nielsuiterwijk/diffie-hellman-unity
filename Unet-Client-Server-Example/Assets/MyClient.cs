using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using Assets.Encrypter;

public class MyClient : MonoBehaviour
{
	private NetworkClient _client;
	private NetworkIdentity _networkStateEntityProtoType;
	void Start()
	{
		//on client, this isn't required but is nice for testing.
		Application.runInBackground = true;

		var globals = FindObjectOfType<GlobalAssets>();

		_networkStateEntityProtoType = globals.NetworkEntityStatePrototype.GetComponent<NetworkIdentity>();

		ClientScene.RegisterSpawnHandler(_networkStateEntityProtoType.assetId, OnSpawnEntity, OnDespawnEntity);

		_client = new NetworkClient();
		_client.Connect("localhost", 7777);
		_client.RegisterHandler(MsgType.Connect, OnClientConnected);

		NetworkConnection connectionToClient = _networkStateEntityProtoType.connectionToClient;
		NetworkConnection connectionToServer = _networkStateEntityProtoType.connectionToServer;

		DiffieHellman a = new DiffieHellman();

	}

	private void OnDespawnEntity(GameObject spawned)
	{
		Destroy(spawned);
	}

	private void OnClientConnected(NetworkMessage netMsg)
	{
		ClientScene.Ready(netMsg.conn);
		ClientScene.AddPlayer(0);


		NetworkConnection connectionToClient = _networkStateEntityProtoType.connectionToClient;
		NetworkConnection connectionToServer = _networkStateEntityProtoType.connectionToServer;
		NetworkConnection connection = netMsg.conn;

	}
	private GameObject OnSpawnEntity(Vector3 position, NetworkHash128 assetId)
	{
		var networkEntity = Instantiate<NetworkIdentity>(_networkStateEntityProtoType);

		networkEntity.transform.SetParent(this.transform);
		return networkEntity.gameObject;
	}
}