using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SD.CharacterSystem
{
    public class UICharacterSheet : MonoBehaviour
    {
        [SerializeField] private Image characterPortrait;
        [SerializeField] private TMP_Text ageText;
        [SerializeField] private TMP_Text wellBeingText;

        [Space]

        [SerializeField] private Image healthBar;
        [SerializeField] private Image staminaBar;
        [SerializeField] private Image actionPointBar;

        [Space]

        [SerializeField] private TMP_Text agiText;
        [SerializeField] private TMP_Text chaText;
        [SerializeField] private TMP_Text intText;
        [SerializeField] private TMP_Text perText;
        [SerializeField] private TMP_Text strText;

        public void DisplayValues(CharacterSheet character)
        {


            agiText.text = "Agility: " + character.attributes[0].Value;
            chaText.text = "Charisma: " + character.attributes[1].Value;
            intText.text = "Intelligence: " + character.attributes[2].Value;
            perText.text = "Perception: " + character.attributes[3].Value;
            strText.text = "Strength: " + character.attributes[4].Value;
        }
    }
}