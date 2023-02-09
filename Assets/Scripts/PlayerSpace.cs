using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1チームに対する視点を管理するためのコンポーネント
/// </summary>
[DisallowMultipleComponent]
public class PlayerSpace : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera = null;

    [SerializeField]
    private Transform _shotOrigin = null;

    public Vector3 ShotOriginPosition => _shotOrigin.position;

    public void Activate()
    {
        _mainCamera.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _mainCamera.gameObject.SetActive(false);
    }
}