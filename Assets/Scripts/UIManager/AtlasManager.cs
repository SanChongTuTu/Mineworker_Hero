using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AtlasManager : MonoBehaviour
{
    [SerializeField] GameObject atlas;
    [SerializeField] GameObject illustrate;
    [SerializeField] TextMeshProUGUI monstername;
    [SerializeField] TextMeshProUGUI monsterblood;
    [SerializeField] TextMeshProUGUI monsterattack;
    [SerializeField] TextMeshProUGUI killmonsternum;
    [SerializeField] TextMeshProUGUI monsterinfo;
    [SerializeField] Image monstericon;

    [SerializeField] List<Button> Monsters;

    void Start()
    {
        atlas.SetActive(false);
        illustrate.SetActive(false);
        foreach(var btn in Monsters)
        {
            btn.onClick.AddListener(() =>
            {
                atlas.SetActive(true);
                btn.transform.GetChild(1).gameObject.SetActive(false);
                Monster info = btn.GetComponent<Monster>();
                monstername.text = info.monster.MonsterName;
                monsterblood.text = $"生命值: {info.monster.MonsterMinHP}~{info.monster.MonsterMaxHP}";
                monsterattack.text = $"攻击力:{info.monster.MonsterMinATK}~{info.monster.MonsterMaxATK}";
                killmonsternum.text = "击杀数量: ";
                monsterinfo.text = info.monster.MonsterInfo;
                monstericon.sprite = info.monster.MonsterIcon;
                illustrate.SetActive(true);
            });
        }
    }

    void Update()
    {
        
    }
}
