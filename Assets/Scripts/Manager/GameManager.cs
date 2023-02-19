using Photon.Pun;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using TMPro;

/// <summary>
/// ゲームの全体的な管理を担うコンポーネント
/// </summary>
[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public TeamColor MyTeamColor { get; private set; }
    public PlayerComponentHolder[] PlayerComponentHolders => _playerComponentHolders;

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
    private SpawnManager _spawnManager = null;

    [SerializeField]
    private TMP_Text _winText;

    [SerializeField]
    private PlayerComponentHolder[] _playerComponentHolders = null;

    //[SerializeField]
    //private TargetManager _targetManager = null;

    [SerializeField]
    private ResultView _resultView = null;

    private GameData _redGameData = new GameData(TeamColor.Red);
    private GameData _blueGameData = new GameData(TeamColor.Blue);
    private GameData _yellowGameData = new GameData(TeamColor.Yellow);
    private GameData _greenGameData = new GameData(TeamColor.Green);

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

        _rpcManager.OnReceiveStartGame += StartGame;
    }

    #region Public Methods

    public void Init(TeamColor myTeamColor)
    {
        MyTeamColor = myTeamColor;
    }

    public void AddScore(TeamColor teamColor, int score)
    {
        var gameData = GetTeamGameData(teamColor);
        gameData.AddScore(score);
        GetTeamComponentHolder(teamColor).SetJobText(gameData.Score.ToString());

        if (gameData.Score >= ScoreToWin)
        {
            if (teamColor == MyTeamColor) _resultView.ShowWin();
            else _resultView.ShowLose();

            //_targetManager.DeactivateSpawn();
        }
    }

    public GameData GetTeamGameData(TeamColor teamColor)
    {
        switch (teamColor)
        {
            case TeamColor.Blue:
                return _blueGameData;
            case TeamColor.Yellow:
                return _yellowGameData;
            case TeamColor.Green:
                return _greenGameData;
            default:
                return _redGameData;
        }
    }

    public PlayerComponentHolder GetTeamComponentHolder(TeamColor teamColor)
    {
        switch (teamColor)
        {
            case TeamColor.Blue:
                return PlayerComponentHolders[1];
            case TeamColor.Yellow:
                return PlayerComponentHolders[2];
            case TeamColor.Green:
                return PlayerComponentHolders[3];
            default:
                return PlayerComponentHolders[0];
        }
    }

    #endregion

    #region Private Methods

    private void UpdatePlayerList()
    {
        foreach (var holder in _playerComponentHolders)
        {
            holder.SetJobText("Survivor");
        }
        foreach (var player in PhotonNetwork.PlayerList)
        {
            Debug.Log(player.ActorNumber);
            _roomInfoView.SetPlayerName(player.NickName, player.ActorNumber - 1);
            if (player.IsMasterClient) _playerComponentHolders[0].SetJobText("Hunter");
        }
    }

    private void StartGame()
    {
        _startGameButton.gameObject.SetActive(false);
        var photonViews = FindObjectsOfType<PhotonView>();
        var players = photonViews
                        .OrderBy(player => player.ViewID)
                        .Select(playes => playes.GetComponent<PlayerController>());
        var count = -2;
        foreach (var player in players)
        {
            count++;
            if(count == -1) continue;

            player.transform.position =  _spawnManager.SpawnPos();
            _playerComponentHolders[count].SetPlayer(player);
        }

        HunterWin();
    }

    async private void HunterWin()
    {
        await UniTask
            .WaitUntil(() => !_playerComponentHolders[1].Player || _playerComponentHolders[1].Player.gameObject.activeSelf == false);
        await UniTask
            .WaitUntil(() => !_playerComponentHolders[2].Player || _playerComponentHolders[2].Player.gameObject.activeSelf == false);
        await UniTask
            .WaitUntil(() => !_playerComponentHolders[3].Player || _playerComponentHolders[3].Player.gameObject.activeSelf == false);
        Debug.Log("おめでとう");
        if (_winText.text == "") _winText.text = "Hunter Win";
    }

    #endregion
}
