using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
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

}
