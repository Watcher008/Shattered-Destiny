using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class TimeManager : MonoBehaviour
{
    private const int MONTHS_IN_YEAR = 12;
    private const int DAYS_IN_MONTH = 30;
    private const int HOURS_IN_DAY = 24;
    private const int MINUTES_IN_HOUR = 60;
    //private const int SECONDS_IN_MINUTE = 60;

    [SerializeField] private TimeData timeData;
    [SerializeField] private PlayerTravelData playerTravelData;

    [Space]

    [SerializeField] private bool resetOnStart = false;

    private int day;
    private int month;
    private int year;
    private float hour;

    private Coroutine travelClockCoroutine;

    private void Start()
    {
        if (resetOnStart) timeData.ResetTime();

        year = timeData.Years;
        month = timeData.Months;
        day = timeData.Days;
        hour = timeData.Hours;
    }

    //Called from GameEventListener
    public void StartTravelClock()
    {
        if (travelClockCoroutine != null) StopCoroutine(travelClockCoroutine);
        travelClockCoroutine = StartCoroutine(RunTravelClock());
    }

    private IEnumerator RunTravelClock()
    {
        while (playerTravelData.PlayerIsMoving)
        {
            HandleHours();
            yield return null;
        }
    }

    private void HandleHours()
    {
        hour += timeData.HoursPerSecond * Time.deltaTime;
        if (hour >= HOURS_IN_DAY)
        {
            hour -= HOURS_IN_DAY;
            OnNewDay();
        }
        timeData.Hours = hour;
    }
    private void OnNewHour()
    {
        hour++;
        if (hour >= HOURS_IN_DAY)
        {
            hour -= HOURS_IN_DAY;
            OnNewDay();
        }
        timeData.Hours = hour;
        //if (hour >= HOURS_IN_DAY) OnNewHour();
    }

    private void OnNewDay()
    {
        day++;
        if (day >= DAYS_IN_MONTH)
        {
            day -= DAYS_IN_MONTH;
            OnNewMonth();
        }
        timeData.Days = day;
    }

    private void OnNewMonth()
    {
        month++;
        if (month >= MONTHS_IN_YEAR)
        {
            month -= MONTHS_IN_YEAR;
            OnNewYear();
        }
        timeData.Months = month;
    }

    private void OnNewYear()
    {
        year++;
        timeData.Years = year;
    }
}