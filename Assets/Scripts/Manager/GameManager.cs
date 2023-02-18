using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームの全体的な管理を担うコンポーネント
/// </summary>
[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public TeamColor MyTeamColor { get; private set; }

    private static readonly int ScoreToWin = 5;

    [SerializeField]
    private RoomInfoView _roomInfoView = null;

    [SerializeField]
    private Button _startGameButton = null;

    [SerializeField]
    private ConnectionManager _connectionManager = null;

    [SerializeField]
    private RPCManager _rpcManager = null;

    [SerializeField]
    private TeamComponentHolder _redTeamComponentHolder = null;

    [SerializeField]
    private TeamComponentHolder _blueTeamComponentHolder = null;

    //[SerializeField]
    //private TargetManager _targetManager = null;

    [SerializeField]
    private ResultView _resultView = null;

    private GameData _redGameData = new GameData(TeamColor.Red);
    private GameData _blueGameData = new GameData(TeamColor.Blue);

    private void Start()
    {
        _connectionManager.OnJoinedRoomEvent += room =>
        {
            _roomInfoView.SetRoomName(room.Name);
            UpdatePlayerList();

            if (PhotonNetwork.IsMasterClient)
            {
                _startGameButton.gameObject.SetActive(true);
                _startGameButton.onClick.AddListener(_rpcManager.SendStartGame);
            }
        };
        _connectionManager.OnPlayerEnteredEvent += player => UpdatePlayerList();
        _connectionManager.OnPlayerLeftEvent += player => UpdatePlayerList();

        //_rpcManager.OnReceiveStartGame += StartGame;
    }

    public void Init(TeamColor myTeamColor)
    {
        MyTeamColor = myTeamColor;
    }

    public void AddScore(TeamColor teamColor, int score)
    {
        var gameData = GetTeamGameData(teamColor);
        gameData.AddScore(score);
        GetTeamComponentHolder(teamColor).SetScoreText(gameData.Score.ToString());

        if (gameData.Score == ScoreToWin)
        {
            if (teamColor == MyTeamColor)
            {
                _resultView.ShowWin();
            }
            else
            {
                _resultView.ShowLose();
            }
            //_targetManager.DeactivateSpawn();
        }
    }

    public GameData GetTeamGameData(TeamColor teamColor)
    {
        return teamColor == TeamColor.Red ? _redGameData : _blueGameData;
    }

    public TeamComponentHolder GetTeamComponentHolder(TeamColor teamColor)
    {
        return teamColor == TeamColor.Red ? _redTeamComponentHolder : _blueTeamComponentHolder;
    }

    private void UpdatePlayerList()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                // マスタークライアントはRed決め打ち
                _roomInfoView.SetRedPlayerName(player.NickName);
            }
            else
            {
                // ゲストクライアントはBlue決め打ち
                _roomInfoView.SetBluePlayerName(player.NickName);
            }
        }
    }

    private void StartGame()
    {
        _startGameButton.gameObject.SetActive(false);
        //_targetManager.ActivateSpawn();
    }

}
