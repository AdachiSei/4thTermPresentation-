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

    #endregion

    #region Inspector Member

    [SerializeField]
    [Header("カメラ")]
    private Camera _mainCamera = null;

    [SerializeField]
    [Header("歩くスピード")]
    private float _walkSpeed = 3f;

    [SerializeField]
    [Header("走るスピード")]
    private float _runSpeed = 6f;

    #endregion

    #region Private Member

    private Rigidbody _rb = null;
    private Animator _animator = null;
    private PhotonView _photonView = null;
    private PhotonRigidbodyView _rigidbodyView = null;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
        _rigidbodyView = GetComponent<PhotonRigidbodyView>();

        if (!_photonView.IsMine) return;

        this
            .FixedUpdateAsObservable()
            .Subscribe(_ => OnMove())
            .AddTo(this);

    }

    #endregion

    #region Public Method

    public void Activate()
    {
        _mainCamera.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        //_mainCamera.gameObject.SetActive(false);
    }

    #endregion

    #region Private Method

    private void OnMove()
    {
        var h = Input.GetAxisRaw(InputName.HORIZONTAL);
        var v = Input.GetAxisRaw(InputName.VERTICAL);
        var y = _rb.velocity.y;


        var speed = !Input.GetButton(InputName.FIRE3) ? _walkSpeed : _runSpeed;
        if(!Input.GetButton(InputName.FIRE3)) _animator.SetBool("IsRunning", false);
        else _animator.SetBool("IsRunning", true);

        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * v + Camera.main.transform.right * h;

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す
        var velocity = moveForward * speed + new Vector3(0, y, 0);

        _rb.velocity = velocity;

        if (moveForward != Vector3.zero) transform.rotation = Quaternion.LookRotation(moveForward);

        _animator.SetFloat("Speed", velocity.magnitude);
    }

    #endregion
}