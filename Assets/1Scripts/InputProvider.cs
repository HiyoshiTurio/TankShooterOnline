using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class InputProvider : SimulationBehaviour, INetworkRunnerCallbacks 
{
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        PlayerInput playerInput = new PlayerInput();
        if (Input.GetMouseButton(0)) 
        {
            playerInput.Buttons.Set(MyButtons.Attack, true);
        }
        if (Input.GetKey(KeyCode.Space)) 
        {
            playerInput.Buttons.Set(MyButtons.Jump, true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerInput.Buttons.Set(MyButtons.Left, true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerInput.Buttons.Set(MyButtons.Right, true);
        }
        if (Input.GetKey(KeyCode.W))
        {
            playerInput.Buttons.Set(MyButtons.Forward, true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerInput.Buttons.Set(MyButtons.Backward, true);
        }
        playerInput.MousePos = Input.mousePosition;
        Debug.Log("Sending input : "+ input.Set(playerInput));
        // Reset the input struct to start with a clean slate
        // when polling for the next tick
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnConnectedToServer(NetworkRunner runner){}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
public struct PlayerInput : INetworkInput
{
    public NetworkButtons Buttons;
    public Vector3 MousePos;
}

enum MyButtons
{
    Forward = 0,
    Backward = 1,
    Left = 2,
    Right = 3,
    Jump = 4,
    Attack = 5,
}