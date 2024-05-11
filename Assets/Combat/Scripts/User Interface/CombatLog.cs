using System.Text;
using UnityEngine;
using TMPro;

public class CombatLog : MonoBehaviour
{
    private static CombatLog instance;

    [SerializeField] private TMP_Text log;
    private StringBuilder builder;

    private void Awake()
    {
        instance = this;

        builder = new StringBuilder();
        log.text = builder.ToString();
    }

    public static void Log(string message)
    {
        instance.builder.Insert(0, $"{message}\n");
        instance.log.text = instance.builder.ToString();
    }
}
