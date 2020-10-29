using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssumedController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("up"))
        {
            transform.position += new Vector3(0,0,100*Time.deltaTime);
        }
        if(Input.GetKey("down"))
        {
            transform.position += new Vector3(0,0,-100*Time.deltaTime);
        }
        if(Input.GetKey("left"))
        {
            transform.position += new Vector3(100*Time.deltaTime,0,0);
        }
        if(Input.GetKey("right"))
        {
            transform.position += new Vector3(-100*Time.deltaTime,0,0);
        }
    }
}
