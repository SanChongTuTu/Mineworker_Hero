using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Monster",menuName ="CreateDate/Monster",order = 1)]
public class CreateMonster : ScriptableObject
{
    [Header("ID")]
    public int ID;
    [Header("怪物名称")]
    public string MonsterName;
    [Header("怪物生命值")]
    public int MonsterHP;
    [Header("怪物攻击力")]
    public int MonsterATK;
}
