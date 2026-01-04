using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Monster
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
            int randnum = Random.Range(0, 2);
            monsterChooseSkill = randnum == 0 ? ChooseSkill.defence : ChooseSkill.none;
        }
    }
}
