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
    public TMP_Text[] PlayerName => _playerNameText;

    [SerializeField]
    private TMP_Text _roomNameText = null;

    [SerializeField]
    private TMP_Text[] _playerNameText = null;


    public string SetRoomName(string name) =>
        _roomNameText.text = name;

    public string SetPlayerName(string name,int index) =>
        _playerNameText[index].text = name;
}