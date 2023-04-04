using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class TimeManager : MonoBehaviour
{
    private int day;
    private int month;
    private int year;
    private int hour;
    private int minute;
    private int second;
    public float TIME_SPEED = 7000000f;

    private const int START_YEAR = 1;
    private const int START_MONTH = 0;
    private const int START_DAY = 1;
    private const int START_HOUR = 0;
    private const int START_MINUTE = 0;
    private const int START_SECOND = 0;

    private const int MONTHS_IN_YEAR = 12;
    private const int DAYS_IN_MONTH = 30;
    private const int HOURS_IN_DAY = 24;
    private const int MINUTES_IN_HOUR = 60;
    private const int SECONDS_IN_MINUTE = 60;

    void Start()
    {
        year = START_YEAR;
        month = START_MONTH;
        day = START_DAY;
        hour = START_HOUR;
        minute = START_MINUTE;
        second = START_SECOND;
    }

    void Update()
    {
        DateSystem(Time.deltaTime);
    }

    public string GetDateTimeText()
    {
        string timeString = string.Format("Hour {0:00}:{1:00}:{2:00}", hour, minute, second);
        return string.Format("Day: {0:00} Month: {1:00} Year: {2:0000}  \n{3}", day, month, year, timeString);
    }

    public void DateSystem(float deltaTime)
    {
        float timeIncrement = deltaTime * TIME_SPEED;

        int totalSeconds = second + Mathf.FloorToInt(timeIncrement);
        second = totalSeconds % SECONDS_IN_MINUTE;
        int totalMinutes = minute + totalSeconds / SECONDS_IN_MINUTE;
        minute = totalMinutes % MINUTES_IN_HOUR;
        int totalHours = hour + totalMinutes / MINUTES_IN_HOUR;
        hour = totalHours % HOURS_IN_DAY;
        int totalDays = day + totalHours / HOURS_IN_DAY;
        day = totalDays % DAYS_IN_MONTH;
        int totalMonths = month + totalDays / DAYS_IN_MONTH;
        month = totalMonths % MONTHS_IN_YEAR;
        year += totalMonths / MONTHS_IN_YEAR;
    }
}

