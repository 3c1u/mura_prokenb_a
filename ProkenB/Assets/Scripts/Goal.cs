using System.Collections;
using System.Collections.Generic;
using ProkenB.Game;
using ProkenB.View;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static bool goal;
    void start()
    {
        goal = false;
    }
    void OnTriggerEnter(Collider other)
    {
        var otherObject = other.gameObject;

        if (otherObject.CompareTag("Player"))
        {
            var playerView = otherObject.GetComponent<PlayerView>();
            playerView.NotifyGoalReached();
        }
    }
}
