using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スクリプト
/// </summary>
public class GameData
{
    #region Public Member

    public TeamColor TeamColor { get; private set; }

    #endregion

    #region Public Method

    public GameData(TeamColor teamColor)
    {
        TeamColor = teamColor;
    }

    #endregion
}