using Cysharp.Threading.Tasks;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// スクリプト
/// </summary>
public class PlayerManager : MonoBehaviour
{
    #region Inspector Member

    [SerializeField]
    [Header("クライアント")]
    PlayerController _playerRed = null;

    [SerializeField]
    [Header("その他")]
    PlayerController _playerBlue = null;

    [SerializeField]
    [Header("プレイヤー")]
    PlayerController[] _players;

    [SerializeField]
    private RPCManager _rpcManager = null;

    [SerializeField]
    private GameManager _gameManager = null;

    #endregion

    #region Private Member

    private PlayerController _my;
    private PlayerController _enemy;

    #endregion

    #region Public Methods

    public void OnMove()
    {
        OnMoveAll();
        _rpcManager.OnMovePlayer += _enemy.OnMove;
    }

    public void Init(TeamColor teamColor)
    {
        GetPlayerController(teamColor);
        _my.Activate();
        _enemy.Deactivate();
    }

    #endregion

    #region Private Member

    private PlayerController GetPlayerController(TeamColor teamColor)
    {
        //_my = teamColor == TeamColor.Blue ? _playerRed : _playerBlue;
        //_enemy = teamColor == TeamColor.Blue ? _playerRed : _playerBlue;
        _my = teamColor == TeamColor.Red ? _players[0] : _players[1];
        _enemy = teamColor == TeamColor.Blue ? _players[0] : _players[1];
        return _my;
    }

    async private void OnMoveAll()
    {   
        while(true)
        {
            _my.OnMove(_my.OnContorol());
            _rpcManager.SendMovePlayer(_my.Velocity);
            await UniTask.WaitForFixedUpdate();
        }
    }

    #endregion
}