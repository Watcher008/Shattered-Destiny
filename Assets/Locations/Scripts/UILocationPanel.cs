using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SD.LocationSystem
{
    public class UILocationPanel : MonoBehaviour
    {
        [SerializeField] private PlayerLocationReference locationReference;

        [Space]

        [SerializeField] private TMP_Text header, body;
        [SerializeField] private RectTransform optionButtonParent;

        public void SetInitialValues()
        {
            gameObject.SetActive(true);
            var location = locationReference.playerLocation;

            header.text = location.name;

            body.text = "You arrive at " + location.name + ".";

            body.text += "\n \n";

            body.text += location.description;
        }
    }
}