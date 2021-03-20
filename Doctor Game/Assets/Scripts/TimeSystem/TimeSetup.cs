using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TimeObj.UpdateTime(0,false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TimeObj.UpdateTime(60,false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TimeObj.Stamina++;
            TimeObj.UpdateTime(60, true);
        }
    }
}
