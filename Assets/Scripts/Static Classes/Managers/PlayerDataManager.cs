﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [Header("Component References")]
    public TextMeshProUGUI currentGoldText;    

    [Header("Properties")]
    public int startingGold;
    public int currentGold;

    // Initialization + Setup
    #region
    private void Start()
    {
        InitializeSetup();
    }
    public void InitializeSetup()
    {
        ModifyGold(startingGold);
    }
    #endregion

    public void ModifyGold(int goldGainedOrLost)
    {
        int goldGainFinalValue = 0;
        if(StateManager.Instance.DoesPlayerAlreadyHaveState("Pennywise") && goldGainedOrLost > 0)
        {
            goldGainFinalValue = (int) (goldGainedOrLost * 1.5f);
        }
        else
        {
            goldGainFinalValue = goldGainedOrLost;
        }

        currentGold += goldGainFinalValue;
        currentGoldText.text = currentGold.ToString();
    }
}
