using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner networkRunnerPrefab;
    [SerializeField] private NetworkPrefabRef playerAvatarPrefab;
    [SerializeField] private GameObject inGameManagerPrefab;
    private NetworkRunner _networkRunner;

    private async void Start()
    {
        // NetworkRunnerを生成する
        _networkRunner = Instantiate(networkRunnerPrefab);
        _networkRunner.AddCallbacks(this);
        _networkRunner.AddCallbacks(_networkRunner.GetComponent<InputProvider>());
        // 共有モードのセッションに参加する
        var result = await _networkRunner.StartGame(new StartGameArgs { GameMode = GameMode.Shared });
        // 結果をコンソールに出力する
        Debug.Log(result);

        GameObject manager = Instantiate(inGameManagerPrefab);
        manager.GetComponent<InGameManager>().SetRunner(_networkRunner);
    }
    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        // セッションへ参加したプレイヤーが自分自身かどうかを判定する
        if (player == runner.LocalPlayer) {
            // アバターの初期位置を計算する（半径5の円の内部のランダムな点）
            var rand = UnityEngine.Random.insideUnitCircle * 5f;
            var spawnPosition = new Vector3(rand.x, 2f, rand.y);
            // 自分自身のアバターをスポーンする
            var playerAvater = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, onBeforeSpawned: (_, networkObject) => {
                // プレイヤー名のネットワークプロパティの初期値として、ランダムな名前を設定する
                networkObject.GetComponent<TankController>().NickName = $"Player{UnityEngine.Random.Range(0, 10000)}";
            });
            //runner.SetPlayerObject(player, playerAvatar);
            //runner.AddCallbacks(Instantiate(inputProviderPrefab));
        }
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}