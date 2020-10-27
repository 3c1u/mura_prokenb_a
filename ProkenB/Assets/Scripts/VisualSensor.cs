using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProkenB.Game;
using ProkenB.Detector;

public class VisualSensor : MonoBehaviour
{
    // Start is called before the first frame update
    public Text fftText;
    public Text voiceText;
    public Text breathText;
    void Start()
    {
        fftText.text = "FFT:None";
        voiceText.text = "Voice:None";
        breathText.text = "Breath:None";
    }

    // Update is called once per frame
    void Update()
    {
        var breath = GameManager.Instance.Detector.Breath;
        var voice = GameManager.Instance.Detector.Voice;
        fftText.text = "FFT";
        voiceText.text = "Voice:" + voice;
        breathText.text = "Breath:" + breath;
    }
}
