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
    private RPCManager _rpcManager = null;

    [SerializeField]
    private GameManager _gameManager = null;

    private TeamColor _teamColor;
    private PlayerController _my;
    private PlayerController _enemy;

    #endregion

    #region Unity Methods

    public void SetTest()
    {
        Move();
        _rpcManager.OnMovePlayer += _enemy.OnMove;
    }

    #endregion

    #region Public Member

    public void Init(TeamColor teamColor)
    {
        _teamColor = teamColor;
        GetPlayerController(_teamColor);
        _my.Activate();
        _enemy.Deactivate();
    }

    #endregion

    #region Private Member

    private PlayerController GetPlayerController(TeamColor teamColor)
    {
        _my = teamColor == TeamColor.Red ? _playerRed : _playerBlue;
        _enemy = teamColor == TeamColor.Blue ? _playerRed : _playerBlue;
        return _my;
    }

    async private void Move()
    {   
        while(true)
        {
            _my.OnMove(_my.OnContorol());
            _rpcManager.SendMovePlayer(_my.Velocity);
            await UniTask.NextFrame();
        }
    }

    #endregion
}