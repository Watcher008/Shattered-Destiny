using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WorldManager : MonoBehaviour
{
    
    public float playerMoney;
    public float playerInfluence;
    public float playerGems;
    public GameObject player;

    public playerScript playerScript;
    public TimeManager timeManager;


    public TextMeshProUGUI dateTimeText;



    // Start is called before the first frame update
    void Start()
    {

        dateTimeText.text = timeManager.GetDateTimeText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();

    }

    public void UpdateTime()
    {
        if (playerScript.playerIsMoving)
        {
            dateTimeText.text = timeManager.GetDateTimeText();
        }
    }


}
