using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ChangeScene", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ChangeScene()
    {
        SceneManager.LoadScene("WorkScene");
    }
}
