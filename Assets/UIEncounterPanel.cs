using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SD.LocationSystem
{
    public class UIEncounterPanel : MonoBehaviour
    {
        [SerializeField] private PlayerLocationReference locationReference;

        [Space]

        [SerializeField] private TMP_Text header, body;
        [SerializeField] private RectTransform optionButtonParent;

        public void SetInitialValues()
        {
            gameObject.SetActive(true);
            header.text = locationReference.playerLocation.Name;
        }
    }
}