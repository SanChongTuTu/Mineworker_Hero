using TMPro;
using UnityEngine;
using static SimpleOre;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI组件 - TextMeshProUGUI")]
    public TextMeshProUGUI bloodText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI rubyCountText;
    public TextMeshProUGUI blueCountText;
    public TextMeshProUGUI purpleCountText;

    // 宝石数量统计
    private int rubyCount = 0;
    private int blueCount = 0;
    private int purpleCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateAllUI();
    }

    public void UpdateAllUI()
    {
        UpdateBloodUI();
        UpdateAttackUI();
        UpdateGemCountUI();
    }

    public void UpdateBloodUI()
    {
        if (bloodText != null && GameDateController.Instance != null)
        {
            bloodText.text = GameDateController.Instance.blood.ToString();
        }
    }

    public void UpdateAttackUI()
    {
        if (attackText != null && GameDateController.Instance != null)
        {
            attackText.text = GameDateController.Instance.attack.ToString();
        }
    }

    public void UpdateGemCountUI()
    {
        if (rubyCountText != null)
            rubyCountText.text =  rubyCount.ToString();
        if (blueCountText != null)
            blueCountText.text =  blueCount.ToString();
        if (purpleCountText != null)
            purpleCountText.text = purpleCount.ToString();
    }

    public void AddGem(OreType oreType)
    {
        switch (oreType)
        {
            case OreType.Ruby:
                rubyCount++;
                break;
            case OreType.Blue:
                blueCount++;
                break;
            case OreType.Purple:
                purpleCount++;
                break;
        }
        UpdateGemCountUI();
    }
}