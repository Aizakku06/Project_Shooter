﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerHUD : MonoBehaviour
{
    public WeaponInfo_UI weaponInfo;
    

    private void OnEnable()
    {
        EventManager.current.NewGunEvent.AddListener(EnableWeaponInfo);
        EventManager.current.PickUpEvent.AddListener(EnablePickUpInfo);
        EventManager.current.PickDownEvent.AddListener(DisablePickUpInfo);

        EventManager.current.DamageEnemyEvent.AddListener(EnableCrossHairDamage);
    }

    private void OnDisable()
    {
        EventManager.current.NewGunEvent.RemoveListener(EnableWeaponInfo);
        EventManager.current.PickUpEvent.RemoveListener(EnablePickUpInfo);
        EventManager.current.PickDownEvent.RemoveListener(DisablePickUpInfo);

        EventManager.current.DamageEnemyEvent.RemoveListener(EnableCrossHairDamage);
    }

    public void EnableWeaponInfo()
    {
        weaponInfo.gameObject.SetActive(true);
    }

    public void EnablePickUpInfo(string name, Sprite icon)
    {
        
    }

    public void DisablePickUpInfo()
    {
        
    }

    private void EnableCrossHairDamage(bool lastHit)
    {
        //if (croshairDamage.gameObject.activeSelf)
        {
            //croshairDamage.NewHit(false, lastHit);
        }
        //else
        {
            //croshairDamage.gameObject.SetActive(true);
            //if(lastHit) croshairDamage.NewHit(false, lastHit);
        }

            
    }
}