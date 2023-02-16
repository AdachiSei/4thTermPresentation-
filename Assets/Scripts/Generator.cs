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
    RPCManager _rpcManager = null;

    [SerializeField]
    [Header("プレイヤーのプレファブ")]
    PlayerController _playerPrefab;

    #region Private Methods

    private void Awake()
    {
        _rpcManager.OnReceiveStartGame += Generate;
    }

    private void Generate()
    {
        PhotonNetwork.Instantiate(_playerPrefab.name, new(), Quaternion.identity);
    }

    #endregion
}