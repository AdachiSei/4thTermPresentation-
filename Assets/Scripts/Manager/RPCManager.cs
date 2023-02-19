using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PhotonのRPC送受信を担うコンポーネント
/// </summary>
[DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
public class RPCManager : MonoBehaviour
{
    #region Private Member

    private PhotonView _photonView;

    #endregion

    #region Event

    public event Action OnReceiveStartGame;
    public event Action<string> OnSetPlayer;
    public event Action<int> OnCaughtSurvivor;

    #endregion

    #region Unity Method

    private void Awake()
    {
        TryGetComponent(out _photonView);
    }

    #endregion

    #region Public Methods

    public void SendStartGame()
    {
        _photonView.RPC(nameof(StartGame), RpcTarget.AllViaServer);
    }

    public void SendCaughtSurvivor(int id)
    {
        _photonView.RPC(nameof(CaughtSurvivor), RpcTarget.AllViaServer, id);
    }

    #endregion

    #region PunRPC Methods

    [PunRPC]
    private void StartGame()
    {
        OnReceiveStartGame?.Invoke();
        Debug.Log("Start");
    }

    [PunRPC]

    private void CaughtSurvivor(int id)
    {
        OnCaughtSurvivor?.Invoke(id);
        Debug.Log("誰かが捕まった");
    }

    #endregion
}