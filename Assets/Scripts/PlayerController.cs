using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("スピード")]
    private float _speed = 5f;

    #endregion

    #region Private Member

    Rigidbody _rb = null;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    #endregion

    #region Public Method

    public Vector3 OnContorol()
    {
        var h = Input.GetAxisRaw(InputName.HORIZONTAL);
        var v = Input.GetAxisRaw(InputName.VERTICAL);
        var y = _rb.velocity.y;
        return new Vector3(h, y, v).normalized * Time.deltaTime * _speed;
    }

    public void OnMove(Vector3 velocity)
    {
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