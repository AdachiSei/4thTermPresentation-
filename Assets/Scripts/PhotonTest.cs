using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// Photon���g�����ʐM�̃`���[�g���A��
/// �ǂ��₷���̂��߁A�����Ă��ׂĂ̏��������̃N���X�ɉ�������ł��܂�
/// </summary>
[DisallowMultipleComponent]
public class PhotonTest : MonoBehaviourPunCallbacks
{
    #region Inspector Member

    [SerializeField]
    [Header("�}�[�J�[�p�̃C���[�W")]
    private Image _markerImage;

    [SerializeField]
    [Header("���O�e�L�X�g1�s���̃I�u�W�F�N�g�̃v���n�u")]
    private TMP_Text _logLinePrefab = null;

    [SerializeField]
    [Header("���O�e�L�X�g�̃��[�g")]
    private Transform _logLineRoot = null;

    [SerializeField]
    [Header("�j�b�N�l�[�����̓t�B�[���h")]
    private TMP_InputField _nickNameInputField = null;

    [SerializeField]
    [Header("���[�������̓t�B�[���h")]
    private TMP_InputField _roomNameInputField = null;

    [SerializeField]
    [Header("���b�Z�[�W���̓t�B�[���h")]
    private TMP_InputField _messageInputField = null;

    [SerializeField]
    [Header("Connect�{�^��")]
    private Button _connectButton = null;

    [SerializeField]
    [Header("Disconnect�{�^��")]
    private Button _disconnectButton = null;

    [SerializeField]
    [Header("Create Room�{�^��")]
    private Button _createRoomButton = null;

    [SerializeField]
    [Header("Join Room�{�^��")]
    private Button _joinRoomButton = null;

    [SerializeField]
    [Header("Send RPC�{�^��")]
    private Button _sendRpcButton = null;

    #endregion

    #region Private Member

    private List<TMP_Text> _logLines = new ();

    // Photon�l�b�g���[�N�z����RPC�̂��������邽�߂ɕK�v�ȃR���|�[�l���g
    private PhotonView _photonView;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        // �e��{�^�����N���b�N�����Ƃ��̃C�x���g��o�^
        _connectButton.onClick.AddListener(Connect);
        _disconnectButton.onClick.AddListener(Disconnect);
        _createRoomButton.onClick.AddListener(CreateRoom);
        _joinRoomButton.onClick.AddListener(JoinRoom);
        _sendRpcButton.onClick.AddListener(SendSampleRpc);

        TryGetComponent(out _photonView);
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom && Input.GetMouseButtonDown(0))
        {
            var sender = PhotonNetwork.NickName;
            var position = Input.mousePosition;
            _markerImage.rectTransform.position = position;
            _photonView.RPC(nameof(NotifyClick), RpcTarget.AllViaServer, sender, position);
        }
    }
    private void OnDestroy()
    {
        if (PhotonNetwork.IsConnected) Disconnect();
    }

    #endregion

    #region Private Methods

    // =======================================
    // �{�^���N���b�N���̃C�x���g����
    // =======================================

    private void Connect()
    {
        // Photon�T�[�o�ɐڑ�
        if (!PhotonNetwork.ConnectUsingSettings())
        {
            AppendLog("ConnectUsingSettings error");
        }
    }

    private void Disconnect()
    {
        // Photon�T�[�o����ؒf
        PhotonNetwork.Disconnect();
    }

    private void CreateRoom()
    {
        // �j�b�N�l�[����ݒ�
        PhotonNetwork.NickName = _nickNameInputField.text;

        // ���͂���Ă��郋�[�����Ń��[���쐬
        if (!PhotonNetwork.CreateRoom(_roomNameInputField.text))
            AppendLog("CreateRoom error");
    }

    private void JoinRoom()
    {
        // �j�b�N�l�[����ݒ�
        PhotonNetwork.NickName = _nickNameInputField.text;

        // ���͂���Ă��郋�[�����Ń��[������
        if (!PhotonNetwork.JoinRoom(_roomNameInputField.text))
            AppendLog("JoinRoom error");
    }

    private void SendSampleRpc()
    {
        var message = _messageInputField.text;

        // ����PhotonView�Ɠ����View ID������PhotonView��RPC�𑗐M
        // �^����p�����[�^�� [PunRPC] ������RPC���\�b�h�̈����ƍ��킹��
        _photonView.RPC(nameof(SampleRpc), RpcTarget.AllViaServer, PhotonNetwork.NickName, message);
    }

    #endregion

    #region PunRPC Methods

    // RPC���\�b�h�̎���
    // PhotonView.RPC���g����Photon�T�[�o�o�R�Ŋe�N���C�A���g�̓��ꃁ�\�b�h���Ăяo�����Ƃ��ł���
    // �ꉞ�A���ʂ̃��\�b�h�ł�����̂ŕ��ʂɌĂяo�����Ƃ��ł���i������񂱂̏ꍇ��Photon�͊֌W�Ȃ��j
    [PunRPC]
    private void SampleRpc(string sender, string message)
    {
        AppendLog($"{sender}> {message}");
    }

    [PunRPC]
    private void NotifyClick(string sender, Vector3 position)
    {
        AppendLog($"{sender}> {position}");
    }

    #endregion

    #region MonoBehaviourPunCallbacks Methods

    // =======================================
    // MonoBehaviourPunCallbacks
    // =======================================

    // Photon�̃}�X�^�[�T�[�o�ɐڑ��ł����Ƃ��ɌĂ΂��
    // ����ȍ~�A���[���̍쐬�E�������ł���
    // ���[���̈ꗗ���擾����ɂ́A�������炳��Ƀ��r�[�ɓ���K�v������
    public override void OnConnectedToMaster()
    {
        AppendLog("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    // �}�X�^�[�T�[�o�̃��r�[�ɓ�����
    // ����ȍ~�A���[���ꗗ�̎擾���ł���
    public override void OnJoinedLobby()
    {
        AppendLog("OnJoinedLobby");
    }

    // ���r�[���̃��[���ꗗ���X�V���ꂽ
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var roomInfo in roomList)
        {
            AppendLog($"{roomInfo.Name} (removed={roomInfo.RemovedFromList})");
        }
    }

    // ���[���쐬������Ɋ��������Ƃ��ɌĂ΂��
    public override void OnCreatedRoom()
    {
        AppendLog("OnCreatedRoom");
    }

    // ���[������������Ɋ��������Ƃ��ɌĂ΂��
    public override void OnJoinedRoom()
    {
        AppendLog("OnJoinedRoom");
    }

    // ���[���쐬�Ɏ��s�����Ƃ��ɌĂ΂��
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        AppendLog($"OnCreateRoomFailed: {message} ({returnCode})");
    }

    // ���[�������Ɏ��s�����Ƃ��ɌĂ΂��
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        AppendLog($"OnJoinRoomFailed: {message} ({returnCode})");
    }

    // �T�[�o����ؒf���ꂽ�Ƃ��ɌĂ΂��
    public override void OnDisconnected(DisconnectCause cause)
    {
        AppendLog($"OnDisconnected: {cause}");
    }

    #endregion

    // =======================================
    // ���̑�
    // =======================================

    private void AppendLog(string message)
    {
        var logLine = Instantiate(_logLinePrefab, _logLineRoot, false);
        logLine.text = message;
        _logLines.Add(logLine);

        if (_logLines.Count == 10)
        {
            Destroy(_logLines[0].gameObject);
            _logLines.RemoveAt(0);
        }
    }
}
