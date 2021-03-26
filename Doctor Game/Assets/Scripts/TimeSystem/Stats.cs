using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Stats
{
    private static int day = 1;
    private static int min = 0;
    private static float hour = 8;
    private static float stamina = 5;
    private static float outOfStamina = 0;
    private static float stress = 0;
    private static Vector3 playerPos;

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

    public static float Hour
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

    public static float Stamina
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

    public static float OutOfStamina
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

    public static float Stress
    {
        get
        {
            return stress;
        }
        set
        {
            if(value >= 0)
            {
                stress = value;
            }
            else if(value <= 1)
            {
                stress = value;
            }
        }
    }

    public static Vector3 PlayerPos
    {
        get
        {
            return playerPos;
        }
        set
        {
            playerPos = value;
        }
    }

    public static void UpdateTime(int timeCost, bool isRelaxing)
    {
        dayLabel = GameObject.Find("DayLabel");
        timeLabel = GameObject.Find("TimeLabel");
        staminaLabel = GameObject.Find("StaminaLabel");

        Min += timeCost;

        hour += Mathf.FloorToInt(timeCost / 60f);
        while (Min >= 30)
        {
            Min -= 30;
            hour += 0.5f;

            if (Min < 0)
            {
                Min = Min * -1;
            }

            if (!isRelaxing)
            {
                if (stamina <= 0)
                {
                    OutOfStamina += 0.5f;
                    
                    if (OutOfStamina >= 5) //Pass out
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
                    stamina -= 0.5f;
                }
            }

        }

        if (Hour >= 24)
        {
            Day += 1;
            Hour = 0;
        }

        
        timeLabel.GetComponent<Text>().text = "Time" + "\n" + Hour.ToString();
        dayLabel.GetComponent<Text>().text = "Day" + "\n" + Day.ToString();
        if (Stamina <= 0 && OutOfStamina != 0)
        {
            staminaLabel.GetComponent<Text>().text = "Overtime Work Hours" + "\n" + OutOfStamina.ToString();
        }
        else
        {
            staminaLabel.GetComponent<Text>().text = "Stamina" + "\n" +  Stamina.ToString();
        }
    }
}
