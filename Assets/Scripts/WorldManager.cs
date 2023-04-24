using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public float playerMoney;
    public float playerInfluence;
    public float playerGems;

    public PlayerLocomotion player { get; set; }
    public TimeManager timeManager;

    private void Start()
    {
        player = FindObjectOfType<PlayerLocomotion>();
    }
}