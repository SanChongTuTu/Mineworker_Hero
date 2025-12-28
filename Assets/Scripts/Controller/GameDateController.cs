using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDateController : MonoBehaviour
{
    [System.Serializable]
    public class ResourceInfo
    {
        private int mask;
        private int check;
        public CreateInGameResource resource;
        public int num;
        public int realnum
        {
            get
            {
                int v=mask^Game.IntMask;
                if (v + 0x47 != check)
                {
                    Game.ExitGame();
                }
                return v;
            }
            set
            {
                mask= value^Game.IntMask;
                check = value + 0x47;
            }
        }

        public void SetAResource(CreateInGameResource res, int amount)
        {
            resource = res;
            mask = 0;
            check = 0;
            realnum = amount;
            num = realnum;
        }

        public void AddAResourcenum(int amount)
        {
            realnum += amount;
            num = realnum;
        }
    }

    [Header("资源")]
    public List<ResourceInfo> resources;
    [Header("生命ֵ")]
    public float blood;
    [Header("攻击")]
    public float attack;
    [Header("��ҵ�ǰ��������")]
    public float strength;
    [Header("挖矿速度")]
    public float minespeed;
    [Header("移动速度")]
    public float movespeed;
    public float criticalChance;
    public float miningFortune;
    public float absorptionCount;
    public float dimensionalPickaxeCount;

    public int powerCrystalNumber;

    private static GameDateController instance;
    public static GameDateController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameDateController>();
                if (instance == null)
                {
                    Debug.Log("No GameDateController");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null&&instance!= this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        var scriptobj = Resources.FindObjectsOfTypeAll<CreateInGameResource>();
        foreach (var obj in scriptobj)
        {
            if (obj.hideFlags == HideFlags.HideAndDontSave) continue;
            bool have = false;
            foreach (ResourceInfo resource in resources)
            {
                if (resource.resource == obj)
                {
                    have = true;
                    break;
                }
            }
            if (have)
            {
                continue;
            }
            else
            {
                ResourceInfo newresource = new ResourceInfo();
                newresource.SetAResource(obj, PlayerPrefs.GetInt(obj.name, 0));
                resources.Add(newresource);
            }
        }
    }

    void Update()
    {
        
    }

    public static void AddResource(CreateInGameResource res, int amount)
    {
        foreach (ResourceInfo resource in Instance.resources)
        {
            if (resource.resource == res)
            {
                resource.AddAResourcenum(amount);
                //PlayerPrefs.SetInt(res.name, resource.realnum);
                return;
            }
        }
        ResourceInfo newresource = new ResourceInfo();
        newresource.SetAResource(res, amount);
        Instance.resources.Add(newresource);
        //PlayerPrefs.SetInt(res.name, newresource.realnum);
    }
}
