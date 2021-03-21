using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Stats.UpdateTime(0,false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Stats.UpdateTime(90,false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Stats.Stamina++;
            Stats.UpdateTime(82, true);
        }
    }
}
