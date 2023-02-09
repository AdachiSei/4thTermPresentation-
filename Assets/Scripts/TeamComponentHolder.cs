using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 1チームあたりのコンポーネントをまとめるためのコンポーネント
/// </summary>
[DisallowMultipleComponent]
public class TeamComponentHolder : MonoBehaviour
{
    public string PlayerName => _playerNameText.text;
    public string ScoreText => _scoreText.text;
    //public ShotSpace ShotSpace => _shotSpace;

    [SerializeField]
    private TMP_Text _playerNameText = null;

    [SerializeField]
    private TMP_Text _scoreText = null;

    //[SerializeField]
    //private ShotSpace _shotSpace = null;

    public void SetPlayerName(string name) =>
        _playerNameText.text = name;

    public void SetScoreText(string score) =>
        _scoreText.text = score;
}