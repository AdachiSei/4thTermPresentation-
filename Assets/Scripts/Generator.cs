using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ジェネレーター
/// </summary>
public class Generator : MonoBehaviour
{
    [SerializeField]
    [Header("")]
    ConnectionManager _connectionManager = null;

    [SerializeField]
    [Header("プレイヤーのプレファブ")]
    PlayerController _playerPrefab;

    #region Private Methods

    private void Awake()
    {
        _connectionManager.OnJoinedRoomGeneratePlayer += Generate;
    }

    private void Generate()
    {
        PhotonNetwork.Instantiate(_playerPrefab.name, _playerPrefab.transform.position, Quaternion.identity);
    }

    #endregion
}