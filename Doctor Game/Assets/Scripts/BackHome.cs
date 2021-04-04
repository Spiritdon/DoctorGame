using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackHome : MonoBehaviour
{
    public GameObject bttn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Stats.Hour >= 18)
        {
            bttn.SetActive(true);
        }
        else
        {
            bttn.SetActive(false);
        }
    }

    public void BackHomeBttnClick()
    {
        Stats.Day += 1;
        Stats.Hour = 8;
        Stats.StressReleaseLimit = 0;
        Stats.Stamina = 5;
        Stats.OutOfStamina = 0;
        Stats.UpdateTime(0, true);
        if(Stats.Stress >= 7)
        {
            Stats.WentHomeStressed++;
        }
    }
}
