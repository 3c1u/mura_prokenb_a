using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class TimerScript : MonoBehaviour {
 
	[SerializeField]
	private int minute;
	[SerializeField]
	private float seconds;
	private float oldSeconds;
	private Text timerText;
 
	void Start () {
		minute = 0;
		seconds = 0f;
		oldSeconds = 0f;
		timerText = GetComponentInChildren<Text> ();
	}
 
	void Update () {
		seconds += Time.deltaTime;
		if(seconds >= 60f) {
			minute++;
			seconds = seconds - 60;
		}
		if((int)seconds != (int)oldSeconds) {
			timerText.text = minute.ToString("00") + ":" + ((int) seconds).ToString ("00");
		}
		oldSeconds = seconds;
	}
}