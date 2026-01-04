using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SimpleOre;

public class OreDropSpawner : MonoBehaviour
{
    private static OreDropSpawner _instance;
    public static OreDropSpawner Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<OreDropSpawner>();
                if (_instance == null)
                {
                    Debug.Log("No OreDropSpawner in scene!");
                }
            }
            return _instance;
        }
    }
    public FlyingOreIcon flyingIconPrefab;  

    public RectTransform targetAnchor;      

    public float flyDuration = 0.6f;

    public List<FlyingOreIcon> flyingIconPool = new List<FlyingOreIcon>();
    public List<RectTransform> rectTransforms = new List<RectTransform>();
    public SpriteRenderer player;


    public void DropOreIcon(SimpleOre targetOre,bool bymouse)
    {
        VoiceController.Instance.PlayHit();
        SimpleOre.OreType theoreType = targetOre.oreType;
        bool ismouse = bymouse;
        switch (theoreType)
        {
            case SimpleOre.OreType.Ruby:
                flyingIconPrefab=flyingIconPool[0];
                targetAnchor = rectTransforms[0];
                break;
            case SimpleOre.OreType.Blue:
                flyingIconPrefab = flyingIconPool[1];
                targetAnchor = rectTransforms[1];
                break;
            case SimpleOre.OreType.Purple:
                flyingIconPrefab = flyingIconPool[2];
                targetAnchor = rectTransforms[2];
                break;
            case SimpleOre.OreType.Lava:
                if(ismouse)
                {
                    
                }
                else
                {
                    //StartCoroutine(SimplePlayer.PlayerHurt());
                    ApplyOreEffect(theoreType, ismouse);
                }
                return;
            default:
                ApplyOreEffect(theoreType, ismouse);
                return;
        }
        Vector3 worldPos = targetOre.transform.position;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        FlyingOreIcon icon = Instantiate(flyingIconPrefab, transform);
        icon.StartFlying(screenPos, targetAnchor, flyDuration,theoreType,ismouse);
    }

    public void ApplyOreEffect(OreType oreType, bool isminebymouse)
    {
        if (GameDateController.Instance == null) return;

        switch (oreType)
        {
            case OreType.Normal:
                Debug.Log("普通矿石被挖掉");
                break;

            case OreType.Ruby:
                GameDateController.Instance.blood += 2;
                if (GameDateController.Instance.miningFortune - 1 > 0)
                {
                    int randnum = Random.Range(1, 5);
                    if (randnum == 1)
                    {
                        GameDateController.Instance.blood += 1;
                        Debug.Log($"挖矿幸运触发！红宝石额外生命值+1");
                    }
                }
                Debug.Log($"红宝石！生命值+2");
                break;

            case OreType.Blue:
                GameDateController.Instance.attack += 1;
                if (GameDateController.Instance.miningFortune - 1 > 0)
                {
                    int randnum = Random.Range(1, 5);
                    if (randnum == 1)
                    {
                        GameDateController.Instance.attack += 1;
                        Debug.Log($"挖矿幸运触发！蓝宝石额外生命值+1");
                    }
                }
                Debug.Log($"蓝宝石！攻击力+1");
                break;

            case OreType.Purple:
                GameDateController.Instance.blood += 3;
                GameDateController.Instance.attack += 2;
                Debug.Log($"紫水晶！生命值+3，攻击力+2");
                break;

            case OreType.Hard:
                Debug.Log($"坚硬矿石被挖掉，无效果");
                break;

            case OreType.Lava:
                if (isminebymouse)
                {
                    Debug.Log($"鼠标挖掉熔岩块！不扣血");
                }
                else
                {
                    if (GameDateController.Instance.blood > 1)
                    {
                        GameDateController.Instance.maxblood = GameDateController.Instance.blood;
                        SimplePlayer.Instance.TakeDamage(1);
                        Debug.Log($"键盘挖掉熔岩块！生命值-1");
                    }
                    else
                    {
                        Debug.Log($"键盘挖掉熔岩块！生命值为1，不扣血");
                    }
                }
                break;
        }

        UpdateUI(oreType);
    }

    public void UpdateUI(OreType oreType)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBloodUI();
            UIManager.Instance.UpdateAttackUI();
            if (oreType == OreType.Ruby || oreType == OreType.Blue || oreType == OreType.Purple)
            {
                UIManager.Instance.AddGem(oreType);
            }
        }
    }
}