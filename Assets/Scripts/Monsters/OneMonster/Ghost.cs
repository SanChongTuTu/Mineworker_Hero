using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ghost : Monster
{


    void Start()
    {
        MonsterName = monster.MonsterName;
        MonsterHP = monster.MonsterHP;
        MonsterATK = monster.MonsterATK;

    }


    void Update()
    {
        
    }
}
