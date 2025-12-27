using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSkeleton : Monster
{

    // Start is called before the first frame update
    void Start()
    {
        MonsterName=monster.MonsterName;
        MonsterHP=monster.MonsterHP;
        MonsterATK = monster.MonsterATK;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
