using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

/// <summary>
/// スクリプト
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Public Property

    public Vector3 Velocity => _rb.velocity;
    public TeamColor MyColor => _teamColor;

    #endregion

    #region Inspector Member

    [SerializeField]
    [Header("カメラ")]
    private Camera _mainCamera = null;

    [SerializeField]
    [Header("スピード")]
    private float _speed = 5f;

    [SerializeField]
    [Header("カラー")]
    TeamColor _teamColor;

    #endregion

    #region Private Member

    private Rigidbody _rb = null;
    private PhotonView _photonView = null;
    private PhotonRigidbodyView _rigidbodyView = null;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        _rigidbodyView = GetComponent<PhotonRigidbodyView>();
        if (!_photonView.IsMine) return;
        this.FixedUpdateAsObservable().Subscribe(_ => OnMove()).AddTo(this);
    }

    #endregion

    #region Public Method

    //public Vector3 OnContorol()
    //{
    //    var h = Input.GetAxisRaw(InputName.HORIZONTAL);
    //    var v = Input.GetAxisRaw(InputName.VERTICAL);
    //    var y = _rb.velocity.y;
    //    return new Vector3(h, y, v).normalized * _speed;
    //}

    public void OnMove()
    {
        var h = Input.GetAxisRaw(InputName.HORIZONTAL);
        var v = Input.GetAxisRaw(InputName.VERTICAL);
        var y = _rb.velocity.y;
        var velocity = new Vector3(h, y, v).normalized * _speed;
        _rb.velocity = velocity;
    }

    public void Activate()
    {
        _mainCamera.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        //_mainCamera.gameObject.SetActive(false);
    }

    #endregion
}