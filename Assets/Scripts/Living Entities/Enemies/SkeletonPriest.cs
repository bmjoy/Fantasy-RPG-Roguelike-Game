﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPriest : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Invigorate");
        mySpellBook.EnemyLearnAbility("Healing Light");
        myPassiveManager.ModifyEncouragingPresence(1);

    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability invigorate = mySpellBook.GetAbilityByName("Invigorate");
        Ability healingLight = mySpellBook.GetAbilityByName("Healing Light");


        /* AI LOGIC
        if invigorate ready, invigorate
        Find the closest wounded ally
        if heal ready and wounded ally in range, heal. 
        If closest wounded ally is not in range and move ready, move towards them. 
        Check if all allies are at full hp
        GetClosestAlly(dont include self)
        If all allies are at full health, and move ready, move towards ally
        will try to move adjacent to an ally to give the protector bonus. 
        If no allies are in range to move next to, it will try to move into a grass tile. 
        If no grass tile is within range, it will stand still. 
        Only attacks enemies with strike if all allies are dead, and will target the closest enemy.
        */

        ActionStart:

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }
        
        // Invigorate
        else if (EntityLogic.IsTargetInRange(this, GetBestInvigorateTarget(invigorate.abilityRange), invigorate.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, invigorate))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Invigorate", false));
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformInvigorate(this, GetBestInvigorateTarget(invigorate.abilityRange));
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Healing Light
        else if (EntityLogic.IsTargetInRange(this, GetBestHealingLightTarget(), healingLight.abilityRange) &&
            GetBestHealingLightTarget().currentHealth < GetBestHealingLightTarget().currentMaxHealth &&
            EntityLogic.IsAbilityUseable(this, healingLight))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Healing Light", false));
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformHealingLight(this, GetBestHealingLightTarget());
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }        

        // Move towards an ally to give Encouraging presence bonus
        else if (EntityLogic.GetClosestAlly(this) != this &&
            EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestAlly(this), currentMobility) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this,move)
            )
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);
            Tile destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, EntityLogic.GetClosestAlly(this), 1, currentMobility);
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);

            AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();

    }

    public Enemy GetBestInvigorateTarget(int range)
    {
        Enemy bestTarget = null;

        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            if(enemy.currentAP < enemy.currentMaxAP &&
                EntityLogic.IsTargetInRange(this, enemy, range) &&
                enemy != this)
            {
                bestTarget = enemy;
            }
        }

        if(bestTarget == null)
        {
            Debug.Log("GetBestInvigorateTarget() is null, returning caster as best target");
            bestTarget = this;
        }
        Debug.Log("GetBestInvigorateTarget() return character: " + bestTarget.name);
        return bestTarget;
    }       

    public Enemy GetBestHealingLightTarget()
    {
        Enemy bestTarget = null;
        int lowestHPvalue = 1000;

        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            if (enemy.currentHealth < lowestHPvalue &&
                enemy.currentHealth < enemy.currentMaxHealth)
            {
                bestTarget = enemy;
                lowestHPvalue = enemy.currentHealth;
            }
        }

        if(bestTarget == null)
        {
            Debug.Log("GetBestHealingLightTarget() is null, returning caster as best target");
            bestTarget = this;
        }

        return bestTarget;
    }
}