﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_attribute : MonoBehaviour
{
    public int playerHealth= Settings.PlayerMaxHealth;
    public int playerDamage= Settings.PlayerDamage;
    public float playerloadingTime= Settings.PlayerLoadingTime;
    private Text responseText;
    // Start is called before the first frame update
    void Start()
    {
        responseText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //remember when call these methods need to has skill points
    public void levelUpHealth()
    {
        if (Player.PlayerSkillPoints > 0)
        {
            Settings.PlayerMaxHealth = playerHealth + 1000;
            playerHealth = Settings.PlayerMaxHealth;
            Player.PlayerSkillPoints -= 1;
        }
        else
        {
            responseText.text = "Sorry Captain, you don't have enough skill points for this upgrade";
        }

    }

    public void levelUpLoading()
    {
        if (Player.PlayerSkillPoints > 0)
        {
            Settings.PlayerLoadingTime = playerloadingTime * 0.95f;
            playerloadingTime = Settings.PlayerLoadingTime;
            Player.PlayerSkillPoints -= 1;
        }
        else
        {
            responseText.text = "Sorry Captain, you don't have enough skill points for this upgrade";
        }
        
    }

    public void levelUpDamage()
    {
        if (Player.PlayerSkillPoints > 0)
        {
            Settings.PlayerDamage = playerDamage + 100;
            playerDamage = Settings.PlayerDamage;
            Player.PlayerSkillPoints -= 1;
        }
        else
        {
            responseText.text = "Sorry Captain, you don't have enough skill points for this upgrade";
        }
    }
    

}
