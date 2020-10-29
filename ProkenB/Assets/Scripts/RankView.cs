using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ProkenB.Game;
using ProkenB.Model;

public class RankView : MonoBehaviour
{
    public Text rankText;
    Vector3 m_goalPosition = new Vector3(0.0f, 0.0f, 450.0f);

    public int rank = 0;

    void Start()
    {
        rankText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        var players = GameManager.Instance.Model.Players;
        var localPlayer = GameManager.Instance.Model.LocalPlayer;

        var topPlayers =
        players.OrderBy(p => p.GoalTime ?? float.PositiveInfinity)
            .ThenBy(p => (p.Position - m_goalPosition).magnitude)
            .ToList();

        rank = 1 + topPlayers.IndexOf(localPlayer);

        rankText.text = $"{rank} 位";
    }
}
