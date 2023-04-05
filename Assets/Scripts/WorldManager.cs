using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using SD.Pathfinding;

public class WorldManager : MonoBehaviour
{
    public float playerMoney;
    public float playerInfluence;
    public float playerGems;

    public PlayerLocomotion player;
    public TimeManager timeManager;


    public TextMeshProUGUI dateTimeText;

    [SerializeField] private int mapWidth = 100;
    [SerializeField] private int mapHeight = 70;
    [SerializeField] private float cellSize = 0.2f;

    private void Awake()
    {
        var pathfinding = new Pathfinding(mapWidth, mapHeight, cellSize);

    }

    private void Start()
    {
        player = FindObjectOfType<PlayerLocomotion>();
        dateTimeText.text = timeManager.GetDateTimeText();
    }

    private void Update()
    {
        UpdateTime();

    }

    private void UpdateTime()
    {
        if (player.playerIsMoving)
        {
            dateTimeText.text = timeManager.GetDateTimeText();
        }
    }
}