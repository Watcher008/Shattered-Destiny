using SD.CharacterSystem;
using UnityEngine;

public class PlayerCompanionsPanel : MonoBehaviour
{
    [SerializeField] private CharacterCollection _companions;

    [Space]

    [SerializeField] private CharacterPortraitPanel panelPrefab;

    private void OnEnable()
    {
        _companions.onCollectionChanged += UpdatePanels;
        UpdatePanels();
    }

    private void OnDisable()
    {
        _companions.onCollectionChanged -= UpdatePanels;
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

        for (int i = 0; i < _companions.Characters.Count; i++)
        {
            var panel = Instantiate(panelPrefab);
            panel.SetCharacter(_companions.Characters[i]);
            panel.transform.SetParent(transform, false);
        }
    }
}
