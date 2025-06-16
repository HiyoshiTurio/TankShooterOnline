using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class InputProvider : SimulationBehaviour, INetworkRunnerCallbacks 
{
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        MyInput myInput = new MyInput();
        if (Input.GetMouseButton(0)) 
        {
            myInput.Buttons.Set(MyButtons.Attack, true);
        }
        if (Input.GetKey(KeyCode.Space)) 
        {
            myInput.Buttons.Set(MyButtons.Jump, true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            myInput.Buttons.Set(MyButtons.Left, true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            myInput.Buttons.Set(MyButtons.Right, true);
        }
        if (Input.GetKey(KeyCode.W))
        {
            myInput.Buttons.Set(MyButtons.Forward, true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            myInput.Buttons.Set(MyButtons.Backward, true);
        }
        Debug.Log("Sending input : "+ input.Set(myInput));
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