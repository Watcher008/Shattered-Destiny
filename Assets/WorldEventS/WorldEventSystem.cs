using UnityEngine;

public class WorldEventSystem : MonoBehaviour
{
    [SerializeField] private TimeData _timeData;

    [SerializeField] private float minRealTimeSecondsBetweenEvents = 300; //5 Minutes
    [SerializeField] private float maxRealTimeSecondsBetweenEvents = 900; //15 minutes

    [SerializeField] private int minGameDaysBetweenEvents = 7; //Min 7 days travel time
    [SerializeField] private int maxGameDaysBetweenEvents = 24; //Max 24 days travel time

    private int lasyDayOnRecord;
    [SerializeField] private float realTimeSinceLastEvent;
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

        //The minimum amount of real-world time must pass for an event to trigger
        if (realTimeSinceLastEvent < minRealTimeSecondsBetweenEvents) return;

        //The number of days since the last event triggered
        int dayDelta = _timeData.NetDays() - gameDayOfLastEvent;


        if (dayDelta < minGameDaysBetweenEvents) return;

        //enough in-game and real-time has passed for the possibility of an event to trigger
        if (realTimeSinceLastEvent >= maxRealTimeSecondsBetweenEvents && dayDelta >= maxGameDaysBetweenEvents)
        {
            OnTriggerWorldEvent();
            return;
        }

        OnTriggerWorldEvent();
    }

    private void OnTriggerWorldEvent()
    {
        Debug.Log("A World Event has been triggered!");
        realTimeSinceLastEvent = 0;
        gameDayOfLastEvent = _timeData.NetDays();
    }
}
