using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProkenB.Detector;

public class describe : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer lr;
    private int count = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MicrophoneSoundDetector x = new MicrophoneSoundDetector();
        count++;
        lr.SetPosition(count - 1, new Vector3(count, x.fftResultBuffer[count], 0));
    }
}
