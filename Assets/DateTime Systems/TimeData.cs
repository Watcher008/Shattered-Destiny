using UnityEngine;

[CreateAssetMenu(fileName = "Time Data", menuName = "Scriptable Objects/Data/Time Data")]
public class TimeData : ScriptableObject
{
    [field: SerializeField] public float HoursPerSecond { get; private set; }

    public float Minutes { get; set; } = 0;
    public float Hours { get; set; } = 0;
    public int Days { get; set; } = 1; 
    public int Months { get; set; } = 0;
    public int Years { get; set; } = 1;

    public void ResetTime()
    {
        Minutes = 0;
        Hours = 0;
        Days = 1;
        Months = 0;
        Years = 1;
    }

    public string GetTimeOfDay()
    {
        if (Hours > 4 && Hours < 12) return "Morning";
        else if (Hours >= 12 && Hours < 17) return "Afternoon";
        else if (Hours >= 17 && Hours < 22) return "Evening";
        else return "Night";
    }
}