using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public enum Type
    {
        active,//活跃的
        normal,//正常的
        conservative,//保守的
    }

    public enum ChooseSkill
    {
        none,
        attack,
        defence,
    }

    public int maxblood;//生命上限
    public int maxattack;//攻击力上限
    [Header("怪物初始资源")]
    public CreateMonster monster;
    [Header("怪物当前名称")]
    public string MonsterName;
    [Header("怪物当前生命值")]
    public int MonsterHP;
    [Header("怪物当前攻击力")]
    public int MonsterATK;
    [Header("怪物攻击概率")]
    public int MonsterATKProbability;
    [Header("怪物性格")]
    public Type MonsterCharacter;

    [Header("怪物本轮选择")]
    public ChooseSkill monsterChooseSkill = ChooseSkill.none;

    private void Start()
    {
        monsterChooseSkill = ChooseSkill.none;
        MonsterName = monster.MonsterName;
        maxblood = Random.Range(monster.MonsterMinHP, monster.MonsterMaxHP + 1);
        maxattack = Random.Range(monster.MonsterMinATK, monster.MonsterMaxATK + 1);
        MonsterATKProbability = Random.Range(monster.MonsterMinATKProbability, monster.MonsterMaxATKProbability + 1);
        MonsterHP = maxblood;
        MonsterATK = maxattack;

        if (MonsterATKProbability >= 66)
        {
            MonsterCharacter = Type.active;
        }
        else if (MonsterATKProbability <= 33)
        {
            MonsterCharacter = Type.conservative;
        }
        else
        {
            MonsterCharacter = Type.normal;
        }
    }


    public abstract void Action();

    public static void TakeDamage(int num)
    {

    }
}
