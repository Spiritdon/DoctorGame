using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watering : MonoBehaviour
{
    public void WateringBttnClicked()
    {
        if (Stats.StressReleaseLimit <= 5)
        {
            Stats.StressReleaseLimit += 0.5f;
            Stats.Stress -= 0.5f;
        }
        Stats.UpdateTime(30, true);
    }
}
