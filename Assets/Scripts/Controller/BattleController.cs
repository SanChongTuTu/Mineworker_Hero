using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    private static BattleController instance;
    public static BattleController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BattleController>();
                if (instance == null)
                {
                    Debug.Log("No BattleController!");
                }
            }
            return instance;
        }
    }

    public enum ChooseSkill
    {
        none,
        attack,
        defence,
        mine,
    }

    public Button attack;
    public TextMeshProUGUI attacktext;
    public TextMeshProUGUI playerblood;
    public TextMeshProUGUI playerATKtext;
    public TextMeshProUGUI monsterATKtext;
    public Button defence;
    public Button mine;
    public ChooseSkill chooseSkill;

    public bool inround;

    private void Awake()
    {
        chooseSkill = ChooseSkill.none;
        inround = false;
        attack.onClick.AddListener(Attack);
        defence.onClick.AddListener(Defence);
        mine.onClick.AddListener(Mine);
    }

    void Start()
    {

    }


    void Update()
    {
        attack.interactable = inround;
        defence.interactable = inround;
        mine.interactable = inround;

        if (GameDateController.Instance.tempAttackBonus > 0)
        {
            attacktext.text = $"造成<color=red>{GameDateController.Instance.attack}</color>+<color=green>{GameDateController.Instance.tempAttackBonus}</color>点伤害";
            playerATKtext.text = $"{GameDateController.Instance.attack}+<color=green>{GameDateController.Instance.tempAttackBonus}</color>";
        }
        else
        {
            attacktext.text = $"造成<color=red>{GameDateController.Instance.attack}</color>点伤害";
            playerATKtext.text = $"{GameDateController.Instance.attack}";
        }

        if (MonsterInfoUI.Instance.monster.MonsterATKBonus > 0)
        {
            monsterATKtext.text = $"{MonsterInfoUI.Instance.monster.MonsterATK}+<color=green>{MonsterInfoUI.Instance.monster.MonsterATKBonus}</color>";
        }
        else if(MonsterInfoUI.Instance.monster.MonsterATKBonus==0)
        {
            monsterATKtext.text = $"{MonsterInfoUI.Instance.monster.MonsterATK}";
        }
        else
        {
            monsterATKtext.text = $"{MonsterInfoUI.Instance.monster.MonsterATK}-<color=red>{MonsterInfoUI.Instance.monster.MonsterATKBonus}</color>";
        }

        playerblood.text = GameDateController.Instance.blood.ToString();
    }

    public void Attack()
    {
        chooseSkill = ChooseSkill.attack;
        NextRound();
    }

    public void Defence()
    {
        chooseSkill = ChooseSkill.defence;
        NextRound();
    }

    public void Mine()
    {
        chooseSkill = ChooseSkill.mine;
        NextRound();
    }

    public void NextRound()
    {
        inround = false;
        StartCoroutine(Nextround());
    }


    IEnumerator Nextround()
    {

        MonsterInfoUI.Instance.monster.Action();
        StartCoroutine(Compare());

        yield return new WaitUntil(() => inround);



        yield break;
    }

    IEnumerator Compare()
    {

        switch (MonsterInfoUI.Instance.monster.monsterChooseSkill)
        {
            case Monster.ChooseSkill.attack:
                MonsterInfoUI.Instance.monster.MonsterATKBonus = 0;

                switch (chooseSkill)
                {
                    case ChooseSkill.attack:
                        int takedamage = (int)(GameDateController.Instance.attack + GameDateController.Instance.tempAttackBonus);
                        if (GameDateController.Instance.criticalChance - 1 > 0)
                        {
                            int randnum = Random.Range(1, 101);
                            if(randnum < 26)
                            {
                                takedamage = (int)(1.2 * takedamage);
                            }
                        }

                        MonsterInfoUI.Instance.monster.TakeDamage(takedamage);
                        GameDateController.Instance.tempAttackBonus = 0;

                        int get = MonsterInfoUI.Instance.monster.MonsterATK + MonsterInfoUI.Instance.monster.MonsterATKBonus;
                        if (GameDateController.Instance.absorptionCount > 0)
                        {
                            GameDateController.Instance.absorptionCount--;
                            get = 0;
                            MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 攻击!\n触发天赋，免除你受到的伤害";
                        }
                        else
                        {
                            SimplePlayer.Instance.TakeDamage(get);
                            MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 攻击!\n对你造成了<color=red>{get}</color>点伤害";
                        }
                        break;
                    case ChooseSkill.defence:
                        int num = (int)(0.4f * (MonsterInfoUI.Instance.monster.MonsterATK + MonsterInfoUI.Instance.monster.MonsterATKBonus));
                        num=Mathf.Max(1,num);
                        if (GameDateController.Instance.absorptionCount > 0)
                        {
                            GameDateController.Instance.absorptionCount--;
                            MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 攻击!\n触发天赋，免除你受到的伤害";
                        }
                        else
                        {
                            SimplePlayer.Instance.TakeDamage(num);
                            SimplePlayer.Instance.AddATK(3);
                            MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 攻击!\n被防御，仅造成<color=red>{num}</color>点伤害";
                        }
                        break;
                    case ChooseSkill.mine:
                        if (GameDateController.Instance.absorptionCount > 0)
                        {
                            GameDateController.Instance.absorptionCount--;
                            MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 攻击!\n触发天赋，免除你受到的伤害";
                        }
                        else
                        {
                            SimplePlayer.Instance.TakeDamage(MonsterInfoUI.Instance.monster.MonsterATK + MonsterInfoUI.Instance.monster.MonsterATKBonus);
                            MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 攻击!\n对你造成了<color=red>{MonsterInfoUI.Instance.monster.MonsterATK + MonsterInfoUI.Instance.monster.MonsterATKBonus}</color>点伤害";
                        }
                        break;
                }
                break;
            case Monster.ChooseSkill.defence:
                switch (chooseSkill)
                {
                    case ChooseSkill.attack:

                        int num=(int)(0.4f * (GameDateController.Instance.attack + GameDateController.Instance.tempAttackBonus));
                        num = Mathf.Max(1,num);
                        if (GameDateController.Instance.criticalChance - 1 > 0)
                        {
                            int randnum = Random.Range(1, 101);
                            if (randnum < 26)
                            {
                                num = (int)(1.2 * num);
                            }
                        }
                        MonsterInfoUI.Instance.monster.TakeDamage(num);
                        GameDateController.Instance.tempAttackBonus = 0;
                        MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 防御!\n抵挡了你的攻击!";
                        MonsterInfoUI.Instance.monster.AddATK(3);
                        break;
                    case ChooseSkill.defence:
                        MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 防御!\n没有什么效果";
                        break;
                    case ChooseSkill.mine:
                        MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 防御!\n没有什么效果";
                        int rannum = Random.Range(-5, 0);
                        SimplePlayer.Instance.TakeDamage(rannum);
                        break;
                }
                break;
            case Monster.ChooseSkill.none:
                switch (chooseSkill)
                {
                    case ChooseSkill.attack:

                        int num = (int)((GameDateController.Instance.attack + GameDateController.Instance.tempAttackBonus));
                        num = Mathf.Max(1, num);
                        if (GameDateController.Instance.criticalChance - 1 > 0)
                        {
                            int randnum = Random.Range(1, 101);
                            if (randnum < 26)
                            {
                                num = (int)(1.2 * num);
                            }
                        }
                        MonsterInfoUI.Instance.monster.TakeDamage(num);
                        GameDateController.Instance.tempAttackBonus = 0;
                        MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 挖矿!\n被打断!";
                        break;
                    case ChooseSkill.defence:
                        int rannum1 = Random.Range(-5, 0);
                        MonsterInfoUI.Instance.monster.TakeDamage(rannum1);
                        MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 挖矿!\n回复了<color=green>{-rannum1}</color>点生命值";
                        break;
                    case ChooseSkill.mine:
                        int rannum2 = Random.Range(-5, 0);
                        MonsterInfoUI.Instance.monster.TakeDamage(rannum2);
                        MonsterInfoUI.Instance.useskillname.text = $"{MonsterInfoUI.Instance.monster.MonsterName} 使用了 挖矿!\n回复了<color=green>{-rannum2}</color>点生命值";
                        int rannum = Random.Range(-5, 0);
                        SimplePlayer.Instance.TakeDamage(rannum);
                        break;
                }
                break;
        }
        MonsterInfoUI.Instance.useskillname.gameObject.SetActive(true);
        CanvasGroup thealpha = MonsterInfoUI.Instance.useskillname.transform.parent.GetComponent<CanvasGroup>();
        thealpha.alpha = 1.0f;

        yield return new WaitForSeconds(2);
        //float time = 0;
        //while (time < 1)
        //{
        //    time += Time.deltaTime;
        //    thealpha.alpha = Mathf.Lerp(1.0f, 0.0f, time / 1.0f);
        //    yield return null;
        //}
        //MonsterInfoUI.Instance.useskillname.gameObject.SetActive(false);
        inround = true;

        yield break;
    }
}
