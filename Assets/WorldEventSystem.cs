using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventSystem : MonoBehaviour
{
    [SerializeField] private TimeData _timeData;

    [SerializeField] private float minRealTimeSecondsBetweenEvents = 300; //5 Minutes
    [SerializeField] private float maxRealTimeSecondsBetweenEvents = 900; //15 minutes

    [SerializeField] private int minGameDaysBetweenEvents = 7;
    [SerializeField] private int maxGameDaysBetweenEvents = 24;

    [SerializeField] private float realTimeSinceLastEvent;
    private int lasyDayOnRecord;
    [SerializeField] private int gameDayOfLastEvent;

    private void Update()
    {
        realTimeSinceLastEvent += Time.deltaTime;
    }

    /// <summary>
    /// A function that is called when time progress in-game
    /// </summary>
    public void OnTimeIncrement()
    {
        lasyDayOnRecord = _timeData.NetDays();

        if (realTimeSinceLastEvent < minRealTimeSecondsBetweenEvents) return;
        int dayDelta = _timeData.NetDays() - lasyDayOnRecord;
        if (dayDelta < minGameDaysBetweenEvents) return;
        //enough in-game and real-time has passed for the possibility of an event to trigger

        if (realTimeSinceLastEvent >= maxRealTimeSecondsBetweenEvents && dayDelta >= maxGameDaysBetweenEvents)
        {
            OnTriggerWorldEvent();
            return;
        }


    }

    private void OnTriggerWorldEvent()
    {
        Debug.Log("A World Event has been triggered!");
        realTimeSinceLastEvent = 0;
        gameDayOfLastEvent = _timeData.NetDays();
    }
}
