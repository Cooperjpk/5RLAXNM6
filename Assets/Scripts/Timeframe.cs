using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timeframe {

    private static bool timeOver;
    private static float timeRate = 1;
    public static float TimeRate
    {
        get
        {
            return timeRate;
        }
        set
        {
            timeRate = value;
        }
    }

    public static void TimeSet(float time)
    {
        timeOver = false;
        
    }

    static void TimeEnd()
    {

    }

    /*
    private static float cooldownStamp;
    public static float CooldownStamp
    {
        get
        {
            return cooldownStamp;
        }
        set
        {
            cooldownStamp = value;
        }
    }
    private static float cooldownTime;
    public static float CooldownTime
    {
        get
        {
            return cooldownTime;
        }
        set
        {
            cooldownTime = value;
        }
    }

    /// <summary>
    /// Start the ticker.
    /// </summary>
    public void TimeSet(float time)
    {
        timeOver = false;
        TimeStart();
        CooldownStamp = CooldownTime + Time.time;
        StartCoroutine(Timeframe(TimeRate));
    }

    /// <summary>
    /// Stops the ticker coroutine.
    /// </summary>
    void TimeInterrupt()
    {
        timeOver = false;
        TimeStop();
        StopCoroutine(Timeframe(TimeRate));
    }

    /// <summary>
    /// Called everytime that the time is started.
    /// </summary>
    public void TimeStart()
    {
        Debug.Log("Timestamp started!");
    }

    /// <summary>
    /// Called everytime that the time is interrupted.
    /// </summary>
    public void TimeStop()
    {
        Debug.Log("Timestamp interrupted!");
    }

    /// <summary>
    /// Called everytime that the time is interrupted.
    /// </summary>
    public void TimeEnd()
    {
        Debug.Log("Timestamp ended!");
    }
    */
}
