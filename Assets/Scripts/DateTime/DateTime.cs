public static class DateTime
{
    private const int MINUTES_IN_HOUR = 60;
    private const int HOURS_IN_DAY = 24;
    private const int DAYS_IN_MONTH = 30;
    private const int MONTHS_IN_YEAR = 12;

    private const int START_MINUTE = 0;
    private const int START_HOUR = 12;
    private const int START_DAY = 11;
    private const int START_MONTH = 4;
    private const int START_YEAR = 1236;

    private static int _minute;
    private static int _hour;
    private static int _day;
    private static int _month;
    private static int _year;

    public static float Minutes => _minute;
    public static float Hours => _hour;
    public static int Days => _day;
    public static int Months => _month;
    public static int Years => _year;

    public static void ResetTime()
    {
        _minute = START_MINUTE;
        _hour = START_HOUR;
        _day = START_DAY;
        _month = START_MONTH;
        _year = START_YEAR;
    }

    public static void IncrementMinute(int minutes = 1)
    {
        _minute += minutes;
        while(_minute >= MINUTES_IN_HOUR)
        {
            _minute -= MINUTES_IN_HOUR;
            IncrementHour();
        }
    }

    public static void IncrementHour(int hours = 1)
    {
        _hour += hours;
        while(_hour >= HOURS_IN_DAY)
        {
            _hour -= HOURS_IN_DAY;
            IncrementDay();
        }
    }

    private static void IncrementDay()
    {
        _day++;
        while(_day >= DAYS_IN_MONTH)
        {
            _day -= DAYS_IN_MONTH;
            IncrementMonth();
        }
    }

    private static void IncrementMonth()
    {
        _month++;
        while(_month >= MONTHS_IN_YEAR)
        {
            _month -= MONTHS_IN_YEAR;
            IncrementYear();
        }
    }

    private static void IncrementYear()
    {
        _year++;
    }

    public static string GetTimeOfDay()
    {
        if (_hour > 4 && _hour < 12) return "Morning";
        else if (_hour >= 12 && _hour < 17) return "Afternoon";
        else if (_hour >= 17 && _hour < 22) return "Evening";
        return "Night";
    }

    public static int NetDays()
    {
        return ((_year - START_YEAR) * DAYS_IN_MONTH * MONTHS_IN_YEAR) + 
            ((_month - START_MONTH) * DAYS_IN_MONTH) + _day - START_DAY;
    }
}
