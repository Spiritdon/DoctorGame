using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text timeLabel;
    public Text dayLabel;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTime(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTime(int timeCost)
    {
        TimeObj.Min += timeCost;
        while (TimeObj.Min >= 60)
        {
            TimeObj.Min -= 60;
            TimeObj.Hour += 1;
            if (TimeObj.Min < 0)
            {
                TimeObj.Min = TimeObj.Min * -1;
            }
        }

        if (TimeObj.Hour >= 24f)
        {
            TimeObj.Day += 1;
            TimeObj.Hour = 0;
        }

        timeLabel.text = "Times: " + TimeObj.Hour.ToString() + ":" + TimeObj.Min.ToString();
        dayLabel.text = "Day: " + TimeObj.Day.ToString();
        Debug.Log(TimeObj.Min);
    }
}
