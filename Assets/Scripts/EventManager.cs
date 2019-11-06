﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public TreasureChest activeTreasureChest;
    public WorldEncounter.EncounterType currentEncounterType;

    public Action StartNewBasicEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewBasicEncounterEventCoroutine(action));
        return action;
    }

    public IEnumerator StartNewBasicEncounterEventCoroutine(Action action)
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = false;

        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();

        // Create a new level
        LevelManager.Instance.CreateLevel();

        // Create defender GO's        
        CharacterRoster.Instance.InstantiateDefenders();  
        
        // Instantiate enemies
        EnemySpawner.Instance.SpawnEnemyWave();
               
        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        currentEncounterType = WorldEncounter.EncounterType.BasicEnemy;

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Start activations / combat start events
        ActivationManager.Instance.OnNewCombatEventStarted();
        
        // declare this event complete
        action.actionResolved = true;
    }
    public Action StartNewEliteEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewEliteEncounterEventCoroutine(action));
        return action;
    }

    public IEnumerator StartNewEliteEncounterEventCoroutine(Action action)
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = false;

        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();

        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();

        // Create a new level
        LevelManager.Instance.CreateLevel();

        // Create defender GO's        
        CharacterRoster.Instance.InstantiateDefenders();

        // Instantiate enemies
        EnemySpawner.Instance.SpawnEnemyWave("Elite");

        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        currentEncounterType = WorldEncounter.EncounterType.EliteEnemy;

        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Start activations / combat start events
        ActivationManager.Instance.OnNewCombatEventStarted();

        // declare this event complete
        action.actionResolved = true;
    }
    public void StartNewRestSiteEncounterEvent()
    {
        StartCoroutine(StartNewRestSiteEncounterEventCoroutine());
    }
    public IEnumerator StartNewRestSiteEncounterEventCoroutine()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = false;
        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);
        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        CampSiteManager.Instance.EnableCampSiteView();
        // disable world map/char roster/inventory view to avoid clutter
        UIManager.Instance.DisableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
        CampSiteManager.Instance.ResetEventProperties();
        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
    }
    public void StartShopEncounterEvent()
    {
        StartCoroutine(StartShopEncounterEventCoroutine());
    }

    public IEnumerator StartShopEncounterEventCoroutine()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = true;
        // fade out view, wait until completed
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 6, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);
        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        ShopScreenManager.Instance.EnableShopScreenView();
        ShopScreenManager.Instance.LoadShopScreenEntities();
        // disable world map/char roster/inventory view to avoid clutter
        UIManager.Instance.DisableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
        // Fade scene back in, wait until completed
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 6, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
        //CampSiteManager.Instance.ResetEventProperties();
    }
    public void StartNewTreasureRoomEncounterEvent()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = true;
        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        // Create a new level
        LevelManager.Instance.CreateLevel();
        //EnemySpawner.Instance.PopulateEnemySpawnLocations();        
        CharacterRoster.Instance.InstantiateDefenders();        
        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        // Instantiate treasure chest and populate it with loot
        CreateNewTreasureChest();
    }
    public void StartNewEndBasicEncounterEvent()
    {
        StartCoroutine(StartNewEndBasicEncounterEventCoroutine());        
    }
    public IEnumerator StartNewEndBasicEncounterEventCoroutine()
    {
        Debug.Log("StartNewEndBasicEncounterEvent() coroutine started...");
        // Show combat end visual events before loot reward screen appears
        preLootScreenEventFinished = false;
        // Disable end turn button
        UIManager.Instance.DisableEndTurnButton();
        // Unselect defender to hide ability bar UI, prevent null behaviors
        DefenderManager.Instance.ClearSelectedDefender();
        // Hide ability info panel
        // Show xp rewards + level ups
        Action lootEvent = StartPreLootScreenVisualEvent(20);
        yield return new WaitUntil(() => lootEvent.ActionResolved() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(20);      
        SpellInfoBox.Instance.HideInfoBox();       
        // re enable world map + get next viable enocunter hexagon tiles
        WorldMap.Instance.SetWorldMapReadyState();
        // Start loot creation/display process
        StartNewLootRewardEvent();
        yield return null;
    }

    public Action StartNewEndEliteEncounterEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewEndEliteEncounterEventCoroutine(action));
        return action;
    }

    public IEnumerator StartNewEndEliteEncounterEventCoroutine(Action action)
    {
        Debug.Log("StartNewEndEliteEncounterEvent() coroutine started...");
        // Show combat end visual events before loot reward screen appears
        preLootScreenEventFinished = false;
        // Disable end turn button
        UIManager.Instance.DisableEndTurnButton();
        // Unselect defender to hide ability bar UI, prevent null behaviors
        DefenderManager.Instance.ClearSelectedDefender();
        // Hide ability info panel
        // Show xp rewards + level ups
        Action lootEvent = StartPreLootScreenVisualEvent(50);
        yield return new WaitUntil(() => lootEvent.ActionResolved() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(50);
        SpellInfoBox.Instance.HideInfoBox();
        // re enable world map + get next viable enocunter hexagon tiles
        WorldMap.Instance.SetWorldMapReadyState();
        // Start loot creation/display process
        StartNewLootRewardEvent(false);        
        action.actionResolved = true;
        
    }

    public Action StartPreLootScreenVisualEvent(int xpReward)
    {
        Action action = new Action();
        StartCoroutine(StartPreLootScreenVisualEventCoroutine(xpReward, action));
        return action;
    }

    public IEnumerator StartPreLootScreenVisualEventCoroutine(int xpReward, Action action)
    {
        Debug.Log("StartPreLootScreenVisualEvent() coroutine started...");
        preLootScreenEventFinished = false;
        // THIS COROUTINE DOES NOT ACTUALLY MODIFY XP/LEVEL!!!! only displays visual info

        // disable activation panel view
        ActivationManager.Instance.SetActivationWindowViewState(false);

        // short yield for seconds to smoothen the transistion
        yield return new WaitForSeconds(1f);

        foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            Debug.Log("StartPreLootScreenVisualEvent() creating visual status xp gained effect...");
            // Dead characters get no XP
            if(character.CurrentHealth > 0)
            {
                StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "XP + " + xpReward.ToString(), true));
            }            
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            if(character.currentXP + xpReward >= character.currentMaxXP)
            {
                Debug.Log("StartPreLootScreenVisualEvent() creating visual status LEVEL GAINED! effect...");
                StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "LEVEL UP!" + xpReward.ToString(), true));
                yield return new WaitForSeconds(0.5f);

            }            
        }
        yield return new WaitForSeconds(1f);

        action.actionResolved = true;
    }

    public bool preLootScreenEventFinished;
    public bool PreLootScreenEventFinished()
    {
        if(preLootScreenEventFinished == true)
        {
            Debug.Log("PreLootScreenEventFinished() bool returning true...");
            return true;
        }
        else
        {
            return false;
        }
    }

    

    public void StartNewMysteryEncounterEvent()
    {
        int randomNumber = Random.Range(1, 101);

        if(randomNumber >= 1 && randomNumber <= 50)
        {
            StartNewBasicEncounterEvent();
        }
        else if (randomNumber >= 51 && randomNumber <= 70)
        {
            StartNewTreasureRoomEncounterEvent();
        }
        else if (randomNumber >= 71 && randomNumber <= 90)
        {
            StartShopEncounterEvent();
        }
        else if (randomNumber >= 91 && randomNumber <= 100)
        {
            StartNewEliteEncounterEvent();
        }
    }

    public void ClearPreviousEncounter()
    {
        // Destroy the level data + all tile Go's
        LevelManager.Instance.DestroyCurrentLevel();
        // Destroy defender GO's from previous encounter
        DefenderManager.Instance.DestroyAllDefenders();
        // Destroy all activation windows
        ActivationManager.Instance.ClearAllWindowsFromActivationPanel();
        // Hide rest site view screen if open
        CampSiteManager.Instance.DisableCampSiteView();
        // Hide Shop view screen if open
        ShopScreenManager.Instance.DisableShopScreenView();
        // Destroy active treasure chest if it exists
        if(activeTreasureChest != null)
        {
            activeTreasureChest.DestroyChest();
        }
    }

    public void StartNewLootRewardEvent(bool basicEnemy = true)
    {
        Debug.Log("StartNewLootRewardEvent() called...");

        if (basicEnemy == true)
        {
            //BlackScreenManager.Instance.FadeOut(5, .5f,true);
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateItemRewardButton();            
            RewardScreen.Instance.PopulateItemScreen();
        }
        else if(basicEnemy == false)
        {
            //BlackScreenManager.Instance.FadeOut(5, .5f);
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateItemRewardButton();
            RewardScreen.Instance.CreateArtifactRewardButton();
            RewardScreen.Instance.PopulateItemScreen();
        }
        
    }

    public void CreateNewTreasureChest()
    {
        // create a treasure chest game object
        GameObject newTreasureChest = Instantiate(PrefabHolder.Instance.TreasureChest);
        newTreasureChest.GetComponent<TreasureChest>().InitializeSetup();
        RewardScreen.Instance.CreateArtifactRewardButton();
    }

    public void EndNewLootRewardEvent()
    {
        RewardScreen.Instance.ClearRewards();
        UIManager.Instance.DisableRewardScreenView();
        UIManager.Instance.EnableWorldMapView();
        WorldMap.Instance.HighlightNextAvailableEncounters();
    }

    


}
