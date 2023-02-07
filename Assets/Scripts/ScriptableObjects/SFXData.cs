using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "SFXData", menuName = "ScriptableObjects/SFXData", order = 1)]
public class SFXData : ScriptableObject
{
    public string Name => _name;
    public int Volume => _volume;
    public AudioClip SFXClip => _sfxClip;

    [SerializeField]
    [Header("名前")]
    private string _name;

    [SerializeField]
    [Header("音量")]
    private int _volume = 1;

    [SerializeField]
    [Header("効果音")]
    private AudioClip _sfxClip;
}