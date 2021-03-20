using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TimeObj
{
    private static int day = 1;
    private static int min = 0;
    private static int hour = 8;
    private static int stamina = 5;
    private static int outOfStamina = 0;

    private static GameObject dayLabel;
    private static GameObject timeLabel;
    private static GameObject staminaLabel;

    public static int Day
    {
        get
        {
            return day;
        }
        set
        {
            day = value;
        }
    }

    public static int Min
    {
        get
        {
            return min;
        }
        set
        {
            min = value;
        }
    }

    public static int Hour
    {
        get
        {
            return hour;
        }
        set
        {
            hour = value;
        }
    }

    public static int Stamina
    {
        get
        {
            return stamina;
        }
        set
        {
            stamina = value;
        }
    }

    public static int OutOfStamina
    {
        get
        {
            return outOfStamina;
        }
        set
        {
            outOfStamina = value;
        }
    }

    public static void UpdateTime(int timeCost, bool isRelaxing)
    {

        dayLabel = GameObject.Find("DayLabel");
        timeLabel = GameObject.Find("TimeLabel");
        staminaLabel = GameObject.Find("StaminaLabel");

        Min += timeCost;
        while (Min >= 60)
        {
            Min -= 60;
            Hour += 1;

            if (Min < 0)
            {
                Min = Min * -1;
            }

            if (!isRelaxing)
            {
                if (stamina <= 0)
                {
                    OutOfStamina++;
                    if (OutOfStamina >= 5)
                    {
                        hour = 12;
                        day++;
                        min = 0;
                        stamina = 3;
                        outOfStamina = 0;
                    }
                }
                else
                {
                    stamina -= 1;
                }
            }

        }

        if (Hour >= 24)
        {
            Day += 1;
            Hour = 0;
        }

        timeLabel.GetComponent<Text>().text = "Times: " + Hour.ToString() + ":" + Min.ToString();
        dayLabel.GetComponent<Text>().text = "Day: " + Day.ToString();
        if (Stamina <= 0 && OutOfStamina != 0)
        {
            staminaLabel.GetComponent<Text>().text = "Overtime Work Hours: " + OutOfStamina.ToString();
        }
        else
        {
            staminaLabel.GetComponent<Text>().text = "Stamina: " + Stamina.ToString();
        }
    }
}
