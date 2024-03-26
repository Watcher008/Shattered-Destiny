using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SD.EventSystem;

public class RandomEncounterManager : MonoBehaviour
{
    [Header("Random Encounters")]
    [SerializeField] private GameEvent _combatEvent;
    [SerializeField] private GameObject _eventPanel;
    [SerializeField] private TMP_Text _eventText;
    [SerializeField] private Button _eventButton;

    [Space]

    [SerializeField, Range(0, 100)]
    private int encounterChance = 100; // Much higher for non-testing
    private int currentChance;

    private string[] _combatText =
    {
        "You sense danger as shadows gather. Weapons at the ready!",
        "Enemies approach with hostility. Prepare for battle!",
        "The air grows tense as foes emerge from the darkness."
    };

    private string[] _randomText =
    {
        // Passing Band of Merchants
        "A merry caravan passes by, laden with goods and tales from afar.",
        "The jingle of coin and laughter fills the air as merchants pass through.",
        "A colorful procession of traders greets you with friendly smiles.",

        // Group of Slain Merchants
        "Tragedy strikes the road as you come upon a scene of carnage.",
        "The remains of a merchant party lie scattered, their goods plundered.",
        "Silence grips the air, broken only by the sight of fallen merchants.",

        // Patrolling Group of Guards
        "Steel-clad guardians march with purpose, their gaze unwavering.",
        "The insignia of the kingdom gleams on the armor of passing guards.",
        "Vigilant sentinels patrol the realm, ensuring order and safety.",

        // Strange Event, Possibly Magical
        "An eerie glow envelops the surroundings, hinting at mystical forces.",
        "Reality twists as arcane energies manifest in a bewildering display.",
        "You witness a phenomenon beyond comprehension, a glimpse into the unknown.",
    };

    /// <summary>
    /// Randomly determines if a random encounter occurs based on player location and time.
    /// </summary>
    public void RollForEvent()
    {
        // Later this should take into account the current terrain, kingdom, setting, etc.
        if (currentChance > Random.Range(0, encounterChance))
        {
            currentChance = 0;
            WorldMapManager.Instance.onPauseInput?.Invoke();

            _eventPanel.SetActive(true);
            _eventButton.onClick.RemoveAllListeners();

            if (Random.value <= 0.3f) // Combat
            {
                _eventText.text = _combatText[Random.Range(0, _combatText.Length)];
                _eventButton.onClick.AddListener(delegate
                {
                    _combatEvent.Invoke();
                });
            }
            else // Random non-combat event
            {
                _eventText.text = _randomText[Random.Range(0, _randomText.Length)];
                _eventButton.onClick.AddListener(delegate
                {
                    _eventPanel.SetActive(false);
                    WorldMapManager.Instance.onResumeInput?.Invoke();
                });
            }
        }
        else currentChance++;
    }
}
