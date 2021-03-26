using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watering : MonoBehaviour
{
    public void WateringBttnClicked()
    {
        Stats.UpdateTime(30, true);
    }
}
