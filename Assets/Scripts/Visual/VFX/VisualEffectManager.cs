﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualEffectManager : MonoBehaviour
{
    [Header("VFX Prefab References")]
    public GameObject DamageEffectPrefab;
    public GameObject StatusEffectPrefab;
    public GameObject ImpactEffectPrefab;
    public GameObject MeleeAttackEffectPrefab;
    public GameObject GainBlockEffectPrefab;
    public GameObject BuffEffectPrefab;
    public GameObject DebuffEffectPrefab;
    public GameObject PoisonAppliedEffectPrefab;
    public GameObject DamagedByPoisonEffect;
    public GameObject TeleportEffectPrefab;
    public GameObject HealEffectPrefab;
    public GameObject AoeMeleeAttackEffectPrefab;

    [Header("Projectile Prefab References")]
    public GameObject ArrowPrefab;
    public GameObject FireBallPrefab;
    public GameObject ShadowBallPrefab;
    public GameObject FrostBoltPrefab;

    [Header("Properties")]
    public List<DamageEffect> vfxQueue = new List<DamageEffect>();
    public float timeBetweenEffectsInSeconds;
    public int queueCount;
    public Color blue;
    public Color red;
    public Color green;    

    // Initialization + Singleton Pattern
    #region
    public static VisualEffectManager Instance;
    private void Awake()
    {
        Instance = this;
    }   
    #endregion

    // Create VFX
    #region
    public IEnumerator CreateDamageEffect(Vector3 location, int damageAmount, bool playFXInstantly = true)
    {
        GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<DamageEffect>().InitializeSetup(damageAmount);
        yield return null;       

    }
    public IEnumerator CreateStatusEffect(Vector3 location, string statusEffectName, bool playFXInstantly, string color = "White")
    {
        Color thisColor = Color.white;
        if(color == "White")
        {
            thisColor = Color.white;
        }
        else if (color == "Blue")
        {
            // to do: find a good colour for buffing
            thisColor = Color.white;
        }
        else if (color == "Red")
        {
            //thisColor = red;
            thisColor = Color.white;
        }
        else if (color == "Green")
        {
            //thisColor = green;
            thisColor = Color.white;
        }
        queueCount++;
        GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<StatusEffect>().InitializeSetup(statusEffectName, thisColor);

        yield return null;
        /*
        if (playFXInstantly == true)
        {
            queueCount++;
            GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<StatusEffect>().InitializeSetup(statusEffectName, thisColor);
        }

        else
        {
            yield return new WaitForSeconds(queueCount * timeBetweenEffectsInSeconds);
            queueCount++;
            GameObject damageEffect = Instantiate(StatusEffectPrefab, location, Quaternion.identity);
            damageEffect.GetComponent<StatusEffect>().InitializeSetup(statusEffectName, thisColor);
        }
        */
        
    }  
    public IEnumerator CreateImpactEffect(Vector3 location)
    {
        GameObject damageEffect = Instantiate(ImpactEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<ImpactEffect>().InitializeSetup(location);

        yield return null;
    }
    public IEnumerator CreateMeleeAttackEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(MeleeAttackEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<MeleeAttackEffect>().InitializeSetup();
        yield return null;
    }
    public IEnumerator CreateGainBlockEffect(Vector3 location, int blockGained)
    {
        GameObject newImpactVFX = Instantiate(GainBlockEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, blockGained);
        yield return null;
    }
    public IEnumerator CreateBuffEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(BuffEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateDebuffEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(DebuffEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreatePoisonAppliedEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(PoisonAppliedEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateDamagedByPoisonEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(DamagedByPoisonEffect, location, Quaternion.identity);
        newImpactVFX.GetComponent<GainArmorEffect>().InitializeSetup(location, 0);
        yield return null;
    }
    public IEnumerator CreateTeleportEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(TeleportEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateHealEffect(Vector3 location, int healAmount)
    {
        GameObject damageEffect = Instantiate(DamageEffectPrefab, location, Quaternion.identity);
        damageEffect.GetComponent<DamageEffect>().InitializeSetup(healAmount, true);        
        GameObject newImpactVFX = Instantiate(HealEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }
    public IEnumerator CreateAoeMeleeAttackEffect(Vector3 location)
    {
        GameObject newImpactVFX = Instantiate(AoeMeleeAttackEffectPrefab, location, Quaternion.identity);
        newImpactVFX.GetComponent<BuffEffect>().InitializeSetup(location);
        yield return null;
    }


    #endregion

    // Projectiles
    #region
    public Action ShootFireball(Vector3 startPos, Vector3 endPos)
    {
        Action action = new Action();
        StartCoroutine(ShootFireballCoroutine(startPos, endPos, action));
        return action;
    }
    public IEnumerator ShootFireballCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed = 7)
    {        
        GameObject fireBall = Instantiate(FireBallPrefab, startPosition, FireBallPrefab.transform.rotation);
        ExplodeOnHit myExplodeOnHit = fireBall.gameObject.GetComponent<ExplodeOnHit>();

        while (fireBall.transform.position != endPosition)
        {
            fireBall.transform.position = Vector2.MoveTowards(fireBall.transform.position, endPosition, speed * Time.deltaTime);
            if (fireBall.transform.position == endPosition)
            {
                myExplodeOnHit.Explode();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public Action ShootArrow(Vector3 startPos, Vector3 endPos, float speed = 7)
    {
        Action action = new Action();
        StartCoroutine(ShootArrowCoroutine(startPos, endPos, action, speed));
        return action;
    }
    public IEnumerator ShootArrowCoroutine(Vector3 startPos, Vector3 endPos, Action action, float speed = 7)
    {
        GameObject arrow = Instantiate(ArrowPrefab,startPos,Quaternion.identity);
        Projectile projectileScript = arrow.GetComponent<Projectile>();
        projectileScript.InitializeSetup(startPos, endPos, speed);
        yield return new WaitUntil(() => projectileScript.destinationReached == true);
        action.actionResolved = true;
    }
    public Action ShootShadowBall(Vector3 startPos, Vector3 endPos)
    {
        Action action = new Action();
        StartCoroutine(ShootShadowBallCoroutine(startPos, endPos, action));
        return action;
    }
    public IEnumerator ShootShadowBallCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed = 4)
    {
        GameObject shadowBall = Instantiate(ShadowBallPrefab, startPosition, ShadowBallPrefab.transform.rotation);
        ExplodeOnHit myExplodeOnHit = shadowBall.gameObject.GetComponent<ExplodeOnHit>();

        while (shadowBall.transform.position != endPosition)
        {
            shadowBall.transform.position = Vector2.MoveTowards(shadowBall.transform.position, endPosition, speed * Time.deltaTime);
            if (shadowBall.transform.position == endPosition)
            {
                myExplodeOnHit.Explode();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public Action ShootFrostBolt(Vector3 startPos, Vector3 endPos)
    {
        Action action = new Action();
        StartCoroutine(ShootFrostBoltCoroutine(startPos, endPos, action));
        return action;
    }
    public IEnumerator ShootFrostBoltCoroutine(Vector3 startPosition, Vector3 endPosition, Action action, float speed = 5)
    {
        GameObject frostBolt = Instantiate(FrostBoltPrefab, startPosition, FrostBoltPrefab.transform.rotation);
        FaceDestination(frostBolt, endPosition);
        ExplodeOnHit myExplodeOnHit = frostBolt.gameObject.GetComponent<ExplodeOnHit>();

        while (frostBolt.transform.position != endPosition)
        {
            frostBolt.transform.position = Vector2.MoveTowards(frostBolt.transform.position, endPosition, speed * Time.deltaTime);
            if (frostBolt.transform.position == endPosition)
            {
                myExplodeOnHit.Explode();
                action.actionResolved = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    // Logic
    #region
    public void FaceDestination(GameObject projectile, Vector3 destination)
    {
        Vector2 direction = destination - projectile.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        projectile.transform.rotation = Quaternion.Slerp(projectile.transform.rotation, rotation, 10000f);
    }
    #endregion

}