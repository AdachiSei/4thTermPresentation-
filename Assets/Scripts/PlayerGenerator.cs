using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// ジェネレーター
/// </summary>
public class PlayerGenerator : MonoBehaviour
{
    [SerializeField]
    [Header("")]
    ConnectionManager _connectionManager = null;

    [SerializeField]
    [Header("")]
    RPCManager _rpcManager = null;

    [SerializeField]
    GameManager _gameManager = null;

    [SerializeField]
    [Header("プレイヤーのプレファブ")]
    PlayerController _playerPrefab;

    [SerializeField]
    [Header("カメラ")]
    CinemachineFreeLook _freeLook = null;

    #region Private Methods

    private void Awake()
    {
        _connectionManager.OnJoinedRoomGeneratePlayer += Generate;
    }

    private void Generate()
    {
        var player = PhotonNetwork.Instantiate(_playerPrefab.name, _playerPrefab.transform.position, Quaternion.identity);
        player.name = PhotonNetwork.NickName;
        _freeLook.Follow = player.transform;
        _freeLook.LookAt = player.transform;
        _gameManager
            .PlayerComponentHolders[PhotonNetwork.PlayerList.OffsetLength()]
            .SetPlayer(player.GetComponent<PlayerController>());
    }

    #endregion
}