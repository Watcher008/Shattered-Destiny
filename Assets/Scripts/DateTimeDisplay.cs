using UnityEngine;
using TMPro;

public class DateTimeDisplay : MonoBehaviour
{
    [SerializeField] private TimeData timeData;

    [Space]

    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text timeText;

    private void Update()
    {
        DisplayTimeDate();
    }

    private void DisplayTimeDate()
    {
        dateText.text = string.Format("Day: {0:00} Month: {1:00} Year: {2:0000}", timeData.Days, timeData.Months, timeData.Years);
        //timeText.text = string.Format("Hour {0:00}:{1:00}", timeData.Hours, timeData.Minutes);
        timeText.text = timeData.GetTimeOfDay();
    }
}