using UnityEngine;
using TMPro;

public class DateTimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _dateText, _timeText;

    private void Update()
    {
        DisplayDateTime();
    }

    private void DisplayDateTime()
    {
        _dateText.text = string.Format("Day: {0} Month: {1} Year: {2}", 
            DateTime.Days, DateTime.Months, DateTime.Years);
        _timeText.text = $"{DateTime.GetTimeOfDay()} {DateTime.Hours:00}:{DateTime.Minutes:00}";
    }
}
