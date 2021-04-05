using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Stats
{
    private static bool playedOnce;
    private static int day = 1;
    private static int min = 0;
    private static float hour = 8;
    private static float hoursWorked = 0;
    private static float stamina = 5;
    private static float outOfStamina = 0;
    private static float stress = 0;
    private static float stressReleaseLimit = 0;
    private static Vector3 playerPos;
    private static int wentHomeStressed;
    private static int variety;
    private static string[] dailyVariety = new string[3];

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
    public static bool PlayedOnce
    {
        get
        {
            return playedOnce;
        }
        set
        {
            playedOnce = value;
        }
    }

    public static void DestressUsed(string str)
    {
        for (int i = 0; i < 3; i++)
        {
            if (str == dailyVariety[i])
            {
                return;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if ("" == dailyVariety[i])
            {
                dailyVariety[i] = str;
            }
        }
    }

    public static int WentHomeStressed
    {
        get
        {
            return wentHomeStressed;
        }
        set
        {
            wentHomeStressed = value;
        }
    }

    public static int Variety
    {
        get
        {
            return variety;
        }
        set
        {
            variety = value;
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
    public static float StressReleaseLimit
    {
        get
        {
            return stressReleaseLimit;
        }
        set
        {
            stressReleaseLimit = value;
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
            if (value < 0)
            {
                stress = 0;
            }
            else
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

    public static float HoursWorked
    {
        get
        {
            return hoursWorked;
        }
        set
        {
            hoursWorked = value;
        }
    }

    public static void UpdateTime(int timeCost, bool isRelaxing)
    {
        dayLabel = GameObject.Find("DayLabel");
        timeLabel = GameObject.Find("TimeLabel");
        staminaLabel = GameObject.Find("StaminaLabel");

        Min += timeCost;

        hour += Mathf.FloorToInt(timeCost / 60f);

        //update hours every 30 min
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
                hoursWorked += 0.5f;
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
            else if (isRelaxing && outOfStamina <= 0 && stressReleaseLimit <= 3)
            {
                stamina += 0.5f;
            }

        }

        //update days every 24 hours
        if (Hour >= 24)
        {
            Day += 1;
            Hour = 8;
            hoursWorked = 0;
            stressReleaseLimit = 0;
            stamina = 5;
            outOfStamina = 0;
            for (int i = 0; i < 3; i++) {
                if(dailyVariety[i] != "")
                {
                    variety++;
                }
                dailyVariety[i] = "";
            }
        }

        timeLabel.GetComponent<Text>().text = "Time" + "\n" + Hour.ToString();
        dayLabel.GetComponent<Text>().text = "Day" + "\n" + Day.ToString();
        if (Stamina <= 0 && OutOfStamina != 0)
        {
            staminaLabel.GetComponent<Text>().text = "Overtime Work Hours" + "\n" + OutOfStamina.ToString();
        }
        else
        {
            staminaLabel.GetComponent<Text>().text = "Stamina" + "\n" + Stamina.ToString();
        }
    }
}
