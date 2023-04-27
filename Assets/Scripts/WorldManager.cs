using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public float playerMoney;
    public float playerInfluence;
    public float playerGems;

    public GameObject player { get; private set; }
    public TimeManager timeManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}