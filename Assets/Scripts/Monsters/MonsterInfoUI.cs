using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using System.Collections;
using System.Threading;

public class MonsterInfoUI : MonoBehaviour
{
    [Header("怪物初始资源")]
    public Monster monster;

    [Header("UI组件")]
    public Image monsterIcon;
    [Header("头像UI")]
    public Image icon;
    [Header("血条UI")]
    public Image healthbarquick; // 血条填充图片
    public Image healthbarslow;  // 血条慢速填充图片
    public TextMeshProUGUI bloodtext;
    public TextMeshProUGUI useskillname;//使用技能名称显示
    [Header("UI提示框")]
    //public GameObject resultPanelPrefab; // 结果面板预制体

    public TextMeshProUGUI monsterNameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI specialText;

    [Header("当前显示的怪物数据")]
    public CreateMonster currentMonsterData;
    public int currentFloor = 1;

    [Header("每层可选怪物列表")]
    public CreateMonster[] floor1Monsters; // 第1层可能出现的怪物
    public CreateMonster[] floor2Monsters; // 第2层可能出现的怪物
    public CreateMonster[] floor3Monsters; // 第3层可能出现的怪物

    private static MonsterInfoUI instance;
    public static MonsterInfoUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MonsterInfoUI>();
                if (instance == null)
                {
                    Debug.Log("MonsterInfoUI实例未找到！");
                }
            }
            return instance;
        }
    }

    void Start()
    {
        useskillname.gameObject.SetActive(false);
        // 初始显示第1层随机怪物
        ShowRandomMonsterForFloor(currentFloor);

        healthbarquick.rectTransform.sizeDelta = new Vector2(0, 30);
        healthbarslow.rectTransform.sizeDelta = new Vector2(0, 30);
        bloodtext.gameObject.SetActive(false);
    }

    public void ShowUseSkill(string skillName)
    {
        useskillname.text = skillName;
        useskillname.gameObject.SetActive(true);
    }

    public void Decreaseblood(float num)
    {
        StartCoroutine(DecreaseBlood((int)num));
    }

    IEnumerator DecreaseBlood(int num)
    {
        float aimtime = 0.5f;
        monster.MonsterHP =Mathf.Max(0,monster.MonsterHP-num);


        // 更新血条UI
        bloodtext.text = monster.MonsterHP + " / " + monster.maxblood;
        float healthRatio = (float)monster.MonsterHP / monster.maxblood;
        healthbarquick.rectTransform.sizeDelta = new Vector2(300 * healthRatio, 30);

        float time = 0;
        float start = healthbarslow.rectTransform.sizeDelta.x;

        while (time < aimtime)
        {
            float nowhealthbarslow = Mathf.Lerp(start, healthRatio * 300, time / aimtime);
            healthbarslow.rectTransform.sizeDelta = new Vector2(nowhealthbarslow, 30);
            time += Time.deltaTime;
            yield return null;
        }

        if (healthbarslow.rectTransform.sizeDelta.x < 300 * healthRatio)
        {
            healthbarslow.rectTransform.sizeDelta = new Vector2(healthRatio * 300, 30);
        }

        yield break;
    }

    // 显示指定层的随机怪物
    public void ShowRandomMonsterForFloor(int floor)
    {
        currentFloor = floor;
 
        // 根据层数获取对应的怪物列表
        CreateMonster[] availableMonsters = GetMonstersForFloor(floor);

        if (availableMonsters == null || availableMonsters.Length == 0)
        {
            Debug.LogWarning($"第{floor}层没有配置怪物");
            ClearUI();
            return;
        }

        // 随机选择一个怪物
        int randomIndex = UnityEngine.Random.Range(0, availableMonsters.Length);
        currentMonsterData = availableMonsters[randomIndex];

        GameObject summonobj=Instantiate(currentMonsterData.obj, new Vector3(3, -41, 0), new Quaternion(0, 0, 0, 0));
        monster=summonobj.GetComponent<Monster>();
        monster.ResetMonster();

        // 更新UI
        UpdateUI();

        Debug.Log($"第{floor}层显示怪物: {currentMonsterData.MonsterName}");
    }

    // 获取指定层的怪物列表
    CreateMonster[] GetMonstersForFloor(int floor)
    {
        return floor switch
        {
            1 => floor1Monsters,
            2 => floor2Monsters,
            3 => floor3Monsters,
            _ => null
        };
    }

    // 更新UI显示
    void UpdateUI()
    {
        if (currentMonsterData == null)
        {
            ClearUI();
            return;
        }

        DecreaseBlood(0);

        // 怪物图标
        if (monsterIcon != null)
        {
            monsterIcon.sprite = currentMonsterData.MonsterIcon;
            monsterIcon.enabled = currentMonsterData.MonsterIcon != null;
        }

        icon.sprite=currentMonsterData.MonsterIcon;

        string character="";
        switch (monster.MonsterCharacter)
        {
            case Monster.Type.active:
                character = "<color=red>活跃的 </color>";
                break;
            case Monster.Type.normal:
                character = "<color=yellow>正常的 </color>";
                break;
            case Monster.Type.conservative:
                character = "<color=blue>保守的 </color>";
                break;
        }

        // 怪物名称
        if (monsterNameText != null)
        {
            monsterNameText.text = character + currentMonsterData.MonsterName;
        }

        // 所在层数

        // 生命值（使用CreateMonster配置的值）
        if (healthText != null)
        {
            // 注意：策划案要求怪物属性在一定范围内随机
            // 但你的CreateMonster已经有固定值，这里可以：
            // 1. 直接使用固定值
            healthText.text = $"{monster.maxblood}";

            // 2. 或者在范围内随机（如果需要随机的话）
            // int randomHP = GetRandomMonsterHP(currentFloor);
            // healthText.text = $"生命值: {randomHP}";
        }

        // 攻击力
        if (attackText != null)
        {
            attackText.text = $"{monster.maxattack}";
        }

        // 怪物介绍/特殊特征
        if (specialText != null)
        {
            specialText.text = GetMonsterSpecialAbility(currentMonsterData);
        }
    }

    // 获取怪物特殊能力描述
    string GetMonsterSpecialAbility(CreateMonster monster)
    {
        // 根据怪物类型返回特殊能力描述
        // 这里需要根据你的怪物类来判断
        //if (monster.MonsterName.Contains("幽灵"))
        //    return "受到攻击有50%的概率不受伤害";
        //if (monster.MonsterName.Contains("蝙蝠"))
        //    return "每次攻击恢复3点生命值";
        //if (monster.MonsterName.Contains("火苗"))
        //    return "火焰攻击";
        //if (monster.MonsterName.Contains("藤蔓"))
        //    return "缠绕攻击";

        return monster.MonsterInfo; // 使用CreateMonster中的介绍
    }

    // 清空UI
    void ClearUI()
    {
        if (monsterIcon != null) monsterIcon.enabled = false;
        
        
        if (healthText != null) healthText.text = "";
        if (attackText != null) attackText.text = "";
        if (specialText != null) specialText.text = "";
    }

    // 获取当前怪物信息（用于战斗系统）
    public CreateMonster GetCurrentMonsterData()
    {
        return currentMonsterData;
    }

}