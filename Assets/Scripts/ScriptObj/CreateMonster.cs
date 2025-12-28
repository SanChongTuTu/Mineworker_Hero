using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="Monster",menuName ="CreateDate/Monster",order = 1)]
public class CreateMonster : ScriptableObject
{
    public enum Level
    {
        firstlevel,
        secondlevel,
        thirdlevel,
    } 

    [Header("ID")]
    public int ID;
    [Header("怪物名称")]
    public string MonsterName;
    [Header("怪物图标")]
    public Sprite MonsterIcon;
    [Header("怪物等级/层级")]
    public Level monsterLevel;
    [Header("怪物最小生命值")]
    public int MonsterMinHP;
    [Header("怪物最大生命值")]
    public int MonsterMaxHP;
    [Header("怪物最小攻击力")]
    public int MonsterMinATK;
    [Header("怪物最大攻击力")]
    public int MonsterMaxATK;
    [Header("怪物预制体")]
    public GameObject obj;
    [Header("怪物介绍")]
    [TextArea]
    public string MonsterInfo;
}
