using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner networkRunnerPrefab;
    [SerializeField] private GameObject inGameManagerPrefab;
    [SerializeField, Header("プレイヤーのプレハブ")] private NetworkPrefabRef playerAvatarPrefab;
    private NetworkRunner _networkRunner;

    private async void Start() 
    {
        // ランダムなプレイヤー名を設定する
        PlayerData.NickName = $"Player{UnityEngine.Random.Range(0, 10000)}";
        
        // NetworkRunnerを生成する
        _networkRunner = Instantiate(networkRunnerPrefab);
        _networkRunner.AddCallbacks(this);
        // StartGameArgsに渡した設定で、セッションに参加する
        var result = await _networkRunner.StartGame(new StartGameArgs 
        {
            GameMode = GameMode.AutoHostOrClient,
            SceneManager = _networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
            Debug.Log("成功！");
        else
            Debug.Log("失敗！");
    }
    // セッションへプレイヤーが参加した時に呼ばれるコールバック
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        // ホスト（サーバー兼クライアント）かどうかはIsServerで判定できる
        if (!runner.IsServer) { return; }
        // ランダムな生成位置（半径5の円の内部）を取得する
        var randomValue = UnityEngine.Random.insideUnitCircle * 5f;
        var spawnPosition = new Vector3(randomValue.x, 5f, randomValue.y);
        // 参加したプレイヤーのアバターを生成する
        var avatar = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);
        // プレイヤー（PlayerRef）とアバター（NetworkObject）を関連付ける
        runner.SetPlayerObject(player, avatar);
    }

    // セッションからプレイヤーが退出した時に呼ばれるコールバック
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer)
        {
            return;
        }
        // 退出したプレイヤーのアバターを破棄する
        if (runner.TryGetPlayerObject(player, out var avatar))
        {
            runner.Despawn(avatar);
        }
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
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