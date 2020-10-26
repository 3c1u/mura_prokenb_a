using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankView : MonoBehaviour
{
    // Start is called before the first frame update
    public Text rankText;
    Vector3 tmp1, tmp2, tmp3;
    Vector3 goalPosition;
    public float relativepass1, relativepass2, relativepass3;
    void Start()
    {
        goalPosition = GameObject.Find("GoalLine").transform.position;
        tmp1 = GameObject.Find("Player").transform.position;
        tmp2 = GameObject.Find("Player2").transform.position;
        tmp3 = GameObject.Find("Player3").transform.position;
        rankText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        tmp1 = GameObject.Find("Player").transform.position;
        tmp2 = GameObject.Find("Player2").transform.position;
        tmp3 = GameObject.Find("Player3").transform.position;
        relativepass1 = goalPosition.z - tmp1.z;
        relativepass2 = goalPosition.z - tmp2.z;
        relativepass3 = goalPosition.z - tmp3.z;
        float[] List = { relativepass1, relativepass2, relativepass3 };
        var list = new List<float>();
        list.AddRange(list);
        list.Sort();
        float i = relativepass1;
        int rank = list.IndexOf(relativepass1) + 1;
        rankText.text = rank + "位";
    }
}
