using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeObj
{
    private static int day = 1;
    private static int min = 0;
    private static int hour = 8;

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
}
