using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ProkenB.Model;
using ProkenB.Game;

public class GameFinish : MonoBehaviour
{
    public Text finishTime;
    public GameObject finishUI;
    // Start is called before the first frame update
    void Start()
    {
        finishTime.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        PlayerModel player = new PlayerModel();
        if (player.HasReachedGoal == true)
        {
            finishUI.SetActive(true);
            var time = GameManager.Instance.Timer;
            finishTime.text = "Finish Time = " + time;

        }
    }
    public void OnRetry()
    {
        SceneManager.LoadScene("Title");
    }
}
