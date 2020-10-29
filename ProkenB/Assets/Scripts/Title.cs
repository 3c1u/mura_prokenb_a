using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using ProkenB.Game;

public class Title : MonoBehaviour
{
    private const string SceneName = "Game Scene";
    public void OnclickStartButton()
    {
        SceneManager.LoadScene(SceneName);
    }
}
