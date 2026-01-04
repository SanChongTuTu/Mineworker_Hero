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
    [Header("怪物临时攻击加成")]
    public int MonsterATKBonus;
    [Header("怪物攻击概率")]
    public int MonsterATKProbability;
    [Header("怪物性格")]
    public Type MonsterCharacter;

    [Header("怪物本轮选择")]
    public ChooseSkill monsterChooseSkill = ChooseSkill.none;

    public void ResetMonster()
    {
        MonsterATKBonus = 0;
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

    public void TakeDamage(int damage)
    {
        damage = Mathf.Clamp(damage, (MonsterHP - maxblood), MonsterHP);
        MonsterInfoUI.Instance.Decreaseblood(damage);

        // 检查是否死亡
        if (MonsterHP <= 0)
        {
            OnMonsterDeath();
        }
    }

    public void AddATK(int num)
    {
        StartCoroutine(AddaTK(num));
    }

    IEnumerator AddaTK(int num)
    {
        num = Mathf.Clamp(num, (int)(1-MonsterATK - MonsterATKBonus), (int)(99 - MonsterATK - MonsterATKBonus));
        Color color = num >= 0 ? Color.green : Color.red;
        BattleController.Instance.monsterATKtext.color = color;
        MonsterATKBonus += num;

        yield break;
    }

    // 怪物死亡处理
    void OnMonsterDeath()
    {
        if (PlayerPrefs.GetInt(MonsterName, 0) == 0)
        {
            PlayerPrefs.SetInt(MonsterName, -1);
            PlayerPrefs.Save();
        }
        else
        {
            int savenum = PlayerPrefs.GetInt(MonsterName, 0);
            savenum++;
            PlayerPrefs.SetInt(MonsterName, savenum);
            PlayerPrefs.Save();
        }

        // 显示胜利面板
        if (SimplePlayer.Instance.resultPanel != null)
        {
            // 计算获得的超能矿石
            int crystalCount = CalculateVictoryCrystals();

            // 设置并显示胜利面板
            SimplePlayer.Instance.resultPanel.number = crystalCount;
            SimplePlayer.Instance.resultPanel.ShowVictory(crystalCount);
        }
        else
        {
            Debug.LogError("ResultPanelUI 未设置！");
        }

    }

    // 计算胜利时获得的超能矿石
    int CalculateVictoryCrystals()
    {
        if (SimplePlayer.Instance.crystalCalculator != null)
        {
            // 需要获取当前层数和其他信息
            int currentLayer = 1; // 假设是第一层，根据实际情况调整
            int monsterMaxHP = maxblood; // 假设Monster类有maxHP属性
            int monsterCurrentHP = 0; // 怪物死亡时当前血量为0
            bool isDefeated = true; // 击败了怪物

            return SimplePlayer.Instance.crystalCalculator.CalcLayerCrystals(
                currentLayer,
                monsterMaxHP,
                monsterCurrentHP,
                isDefeated
            );
        }

        // 如果没有计算器，返回默认值
        return 5; // 默认胜利获得5个矿石
    }

    // 如果需要，可以添加一个获取怪物信息的方法
    //public Monster GetMonsterInfo()
    //{
    //    return monster;
    //}

}
