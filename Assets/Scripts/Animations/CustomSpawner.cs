using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Object;
using UnityEngine;

public class CustomSpawner : MonoBehaviour
{
    [SerializeField] private NetworkObject[] Prefabs;
    [SerializeField] private Transform[] SpawnPositions;
    [SerializeField] private NetworkManager _networkManager;

    public event Action OnStarted;

    public void Awake()
    {
        _networkManager.SceneManager.OnClientLoadedStartScenes += OnClientLoadedStartScenes;
    }

    private void OnDestroy()
    {
        if (_networkManager != null)
            _networkManager.SceneManager.OnClientLoadedStartScenes -= OnClientLoadedStartScenes;
    }

    private void OnClientLoadedStartScenes(NetworkConnection _, bool asServer)
    {
        if (!asServer)
            return;

        List<NetworkConnection> authenticatedClients = _networkManager.ServerManager.Clients.Values
            .Where(conn => conn.IsAuthenticated).ToList();
        if (authenticatedClients.Count < 2) return;

        Spawn();
    }

    [Server]
    void Spawn()
    {
        int index = 0;
        foreach (var client in _networkManager.ServerManager.Clients.Values)
        {
            // Since the ServerManager.Clients collection contains all clients (even non-authenticated ones),
            // we need to check if they are authenticated first before spawning a player object for them.
            if (!client.IsAuthenticated)
                continue;

            var pos = SpawnPositions[index];
            var prefab = Prefabs[index];
            NetworkObject obj = _networkManager.GetPooledInstantiated(prefab, position: pos.position,
                rotation: pos.rotation, asServer: true);
            _networkManager.ServerManager.Spawn(obj, client);

            if (!client.Scenes.Contains(gameObject.scene))
                _networkManager.SceneManager.AddOwnerToDefaultScene(obj);
            index++;
        }

        OnStarted?.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}