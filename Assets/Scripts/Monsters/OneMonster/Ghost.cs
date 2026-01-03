using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ghost : Monster
{


    void Start()
    {

    }


    void Update()
    {
        
    }

    public override void Action()
    {
        int rand = Random.Range(0, 100);
        if (rand <= MonsterATKProbability)
        {
            monsterChooseSkill = ChooseSkill.attack;
        }
        else
        {
            monsterChooseSkill = ChooseSkill.defence;
        }
    }
}
