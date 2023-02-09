using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 左上のルーム名・プレイヤー名の表示を管理するコンポーネント
/// </summary>
[DisallowMultipleComponent]
public class RoomInfoView : MonoBehaviour
{
    public string RoomName => _roomNameText.text;
    public string RedPlayerName => _redPlayerNameText.text;
    public string BluePlayerName => _bluePlayerNameText.text;

    [SerializeField]
    private TMP_Text _roomNameText = null;

    [SerializeField]
    private TMP_Text _redPlayerNameText = null;

    [SerializeField]
    private TMP_Text _bluePlayerNameText = null;


    public string SetRoomName(string name) =>
        _roomNameText.text = name;

    public string SetRedPlayerName(string name) =>
        _redPlayerNameText.text = name;

    public string SetBluePlayerName(string name) =>
        _bluePlayerNameText.text = name;
}