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

    #region Private Member

    Rigidbody _rb;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    #endregion

    #region Public Method

    public void OnMove(Vector3 velocity)
    {
        _rb.velocity = velocity;
    }

    #endregion

    #region Private Property

    private Vector3 OnContorol()
    {
        var h = Input.GetAxis(InputName.HORIZONTAL);
        var v = Input.GetAxis(InputName.VERTICAL);
        var y = _rb.velocity.y;
        return new(h, y, v);
    }

    #endregion
}