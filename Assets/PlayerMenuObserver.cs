using UnityEngine;
using SD.Primitives;

public class PlayerMenuObserver : MonoBehaviour
{
    [SerializeField] private IntReference playerMenuStatusRef;
    [SerializeField] private GameObject[] playerMenuTabs;

    private void OnEnable()
    {
        OnPlayerMenuTabChanged();
    }

    private void OnDisable()
    {
        playerMenuStatusRef.Value = (int)PlayerMenuStatus.Closed;
    }

    //Called from GameEventListener
    public void OnPlayerMenuTabChanged()
    {
        for (int i = 0; i < playerMenuTabs.Length; i++)
        {
            //value -1 because 0 = closed
            if (i == playerMenuStatusRef.Value - 1)
            {
                playerMenuTabs[i].SetActive(true);
            }
            else playerMenuTabs[i].SetActive(false);
        }
    }
}
