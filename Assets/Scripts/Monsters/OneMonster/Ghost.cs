using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ghost : Monster
{


    void Start()
    {
        MonsterName = monster.MonsterName;
        maxblood = Random.Range(monster.MonsterMinHP, monster.MonsterMaxHP + 1);
        maxattack = Random.Range(monster.MonsterMinATK, monster.MonsterMaxATK + 1);
        MonsterHP = maxblood;
        MonsterATK = maxattack;

    }


    void Update()
    {
        
    }
}
