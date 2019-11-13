﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Enemy : LivingEntity
{
    [Header("Enemy Components")]
    public CharacterInfoPanel myInfoPanel;
    [Header("Enemy Properties")]
    public string myName;

    // Initialization + Setup
    #region
    public override void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {              
        EnemyManager.Instance.allEnemies.Add(this);        
        base.InitializeSetup(startingGridPosition, startingTile);
        myInfoPanel.InitializeSetup(this);
        
    }
    #endregion

    // Activation + Related
    #region
    public virtual void StartMyActivation()
    {       
        StartCoroutine(StartMyActivationCoroutine());
    }
    public virtual IEnumerator StartMyActivationCoroutine()
    {
        yield return null;
    }
    public Action EndMyActivation()
    {
        Action action = new Action();
        StartCoroutine(EndMyActivationCoroutine(action));
        return action;

    }
    public IEnumerator EndMyActivationCoroutine(Action action)
    {
        Action endActivation = ActivationManager.Instance.EndEntityActivation(this);
        yield return new WaitUntil(() => endActivation.ActionResolved() == true);
        action.actionResolved = true;
        ActivationManager.Instance.ActivateNextEntity();
    }

    public bool currentlyActivated = false;
    public bool ActivationFinished()
    {
        if (currentlyActivated == false)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    #endregion

    // AI Targeting Logic
    #region
    public void SetTargetDefender(LivingEntity target)
    {
        myCurrentTarget = target;
    }
    
    #endregion    

    // Mouse + Click Events
    #region
    public void OnMouseDown()
    {
        Debug.Log("Enemy click detected");
        EnemyManager.Instance.SelectEnemy(this);
    }
    public void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            myInfoPanel.EnablePanelView();
        }
    }
    #endregion

}