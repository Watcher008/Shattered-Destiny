using SD.CharacterSystem;
using UnityEngine;

public class PlayerCompanionsPanel : MonoBehaviour
{
    [SerializeField] private CharacterCollection companions;

    [Space]

    [SerializeField] private CharacterPortraitPanel panelPrefab;

    private void OnEnable()
    {
        companions.onCollectionChanged += UpdatePanels;
        UpdatePanels();
    }

    private void OnDisable()
    {
        companions.onCollectionChanged -= UpdatePanels;
    }

    private void ClearPanels()
    {
        int count = transform.childCount;
        for (int i = count - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void UpdatePanels()
    {
        ClearPanels();

        for (int i = 0; i < companions.Characters.Count; i++)
        {
            var panel = Instantiate(panelPrefab);
            panel.SetCharacter(companions.Characters[i]);
            panel.transform.SetParent(transform, false);
        }
    }
}
