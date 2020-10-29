using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProkenB.Game;
using ProkenB.Model;

public class RankView : MonoBehaviour
{
    // Start is called before the first frame update
    public Text rankText;
    Vector3 tmp1 = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 tmp2 = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 tmp3 = new Vector3(0.0f, 0.0f, 0.0f);
    // Goal Objectの所在が分からなかったので座標で宣言しておく
    Vector3 goalPosition = new Vector3(0.0f, 0.0f, 450.0f);
    public float relativepass1, relativepass2, relativepass3;
    public int rank;

    void Start()
    {
        rankText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        var player = GameManager.Instance.Model.Players;
        var local_player = GameManager.Instance.Model.LocalPlayer;
        PlayerModel Player1 = player[0];
        PlayerModel Player2 = player[1];
        PlayerModel Player3 = player[2];
        tmp1 = Player1.Position;
        tmp2 = Player2.Position;
        tmp3 = Player3.Position;
        relativepass1 = goalPosition.z - tmp1.z;
        relativepass2 = goalPosition.z - tmp2.z;
        relativepass3 = goalPosition.z - tmp3.z;
        var relativepass = goalPosition.z - local_player.Position.z;
        if (relativepass == relativepass1)
        {
            relativepass = relativepass1;
        }
        if (relativepass == relativepass2)
        {
            relativepass = relativepass2;
        }
        if (relativepass == relativepass3)
        {
            relativepass = relativepass3;
        }
        float[] List = { relativepass1, relativepass2, relativepass3 };
        var list = new List<float>();
        list.AddRange(list);
        list.Sort();
        list.Reverse();
        rank = list.IndexOf(relativepass) + 1;
        rankText.text = rank + "位";
    }
}
