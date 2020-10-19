using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFinish : MonoBehaviour
{
    public Text finishTime;
    public GameObject finishUI;
    GameObject refObj;
    // Start is called before the first frame update
    void Start()
    {
        finishTime.text="";
        refObj=GameObject.Find("GameObject");
    }

    // Update is called once per frame
    void Update()
    {
        if(Goal.goal==true)
        {
            finishUI.SetActive(true);
            Timer t=refObj.GetComponent<Timer>();
            string v = "Finish Time = " + t.timerText.text;
            finishTime.text = v;
        }

    }
    public void OnRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
