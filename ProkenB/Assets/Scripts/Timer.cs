using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public Text CountText;
    float countdown = 3f;
    int count;
    int minute = 0;
    float seconds = 0f;
    float oldSeconds = 0f;
    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown >= 0)
        {
            countdown -= Time.deltaTime;
            count = (int)countdown;
            CountText.text = count.ToString();
        }
        if (countdown <= 0)
        {
            CountText.text = "";
            if (Goal.goal == false)
            {
                seconds += Time.deltaTime;
            }
            if (seconds >= 60f)
            {
                minute++;
                seconds = seconds - 60;
            }
            if ((int)seconds != (int)oldSeconds)
            {
                timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
            }
            oldSeconds = seconds;
        }
    }
}
