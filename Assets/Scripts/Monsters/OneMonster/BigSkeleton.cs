using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSkeleton : Monster
{

    // Start is called before the first frame update
    void Start()
    {
        MonsterName=monster.MonsterName;
        maxblood = Random.Range(monster.MonsterMinHP, monster.MonsterMaxHP + 1);
        maxattack = Random.Range(monster.MonsterMinATK, monster.MonsterMaxATK + 1);
        MonsterHP = maxblood;
        MonsterATK = maxattack;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
