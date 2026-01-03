using System.Collections;
using System.Collections.Generic;
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
                switch (chooseSkill)
                {
                    case ChooseSkill.attack:

                        break;
                    case ChooseSkill.defence:

                        break;
                    case ChooseSkill.mine:

                        break;
                }
                break;
            case Monster.ChooseSkill.defence:
                switch (chooseSkill)
                {
                    case ChooseSkill.attack:

                        break;
                    case ChooseSkill.defence:

                        break;
                    case ChooseSkill.mine:

                        break;
                }
                break;
        }
        yield break;
    }
}
