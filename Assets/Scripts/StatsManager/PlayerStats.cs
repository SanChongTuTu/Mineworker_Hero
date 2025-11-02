using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class PlayerStats
{
    [Header("Basic Stats")]
    public static float Health;
    public static float Vitality;
    public static float Speed;
    
    [Header("Combat Stats")]
    public static float BaseDamage;
    public static float Strength;
    
    [Header("Mining Stats")]
    public static float MiningSpeed;
    public static int BreakingPower;
    public static float MiningFortune;

    [Header("Misc Stats")] 
    public static float DefaultAvailableTime;

    public static float CalculateActualDamage(float baseDamage, float strength)
    {
        float damage = (5 + baseDamage) * (1 + strength / 100);
        return damage;
    }

    public static float CalculateActualMiningTimeMs(float miningSpeed, int blockStrength)
    {
        int miningTicks = Convert.ToInt32(Math.Round(30.0 * blockStrength / miningSpeed));
        float mingTimeMs = 50 * miningTicks;
        return mingTimeMs;
    }
}
