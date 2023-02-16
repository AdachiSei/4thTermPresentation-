using Cysharp.Threading.Tasks;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// 最初の画面
/// </summary>
[DisallowMultipleComponent]
public class StartPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _playerNameInputField = null;

    [SerializeField]
    private TMP_InputField _roomNameInputField = null;

    [SerializeField]
    private GameObject _uiRoot = null;

    [SerializeField]
    private Button _startButton = null;

    //[SerializeField]
    //private FadeImageController _fadeImageController = null;

    [SerializeField]
    private ConnectionManager _connectionManager = null;

    [SerializeField]
    private GameManager _gameManager = null;

    [SerializeField]
    private PlayerManager _playerManager = null;

    private void Awake()
    {
        // デフォルトのプレイヤーネーム
        _playerNameInputField.text = $"player-{Random.Range(100, 1000):D03}";

        // デフォルトのルーム名
        _roomNameInputField.text = $"room-{Random.Range(100, 1000):D03}";

        _startButton.onClick.AddListener(OnStartButtonClicked);

        _uiRoot.SetActive(true);
    }

    private void OnStartButtonClicked()
    {
        _startButton.interactable = false;

        var nickName = _playerNameInputField.text;
        var roomName = _roomNameInputField.text;

        _connectionManager
            .Connect
                (nickName, roomName, Transition, Debug.LogError);
    }

    async private void Transition()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f));

        _uiRoot.SetActive(false);

        // チームカラーを決定して初期化
        _gameManager.Init(PhotonNetwork.IsMasterClient ? TeamColor.Red : TeamColor.Blue);

        await UniTask.WaitUntil(() => PhotonNetwork.PlayerList.Length > 1);

        _playerManager.OnMove();
    }
}