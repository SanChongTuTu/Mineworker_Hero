using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOre : MonoBehaviour
{
    public enum OreType
    {
        Normal = 0,
        Ruby = 1,
        Blue = 2,
        Purple = 3,
        Hard = -1,
        Lava = 4,
    }

    [Header("矿石类型")]
    public OreType oreType = OreType.Normal;
    public SpriteRenderer oreUIImage;
    public Sprite[] oreSprites;
    public float baseDigTime = 1.0f;
    public bool canClickToMine = true;

    [Header("进度条设置")]
    public GameObject progressBarObject; // 进度条2D物体（带SpriteRenderer）
    public Vector2 progressBarOffset = new Vector2(0, 0.4f); // 偏移量

    private GameObject progressBarInstance;
    private SpriteRenderer progressBarSR;
    private Transform progressBarTransform;
    private Vector3 originalBarScale; // 进度条原始尺寸

    private float remainingTime = 0f;
    private bool isBeingDug = false;
    private float totalDigTime;
    public bool isMinedByMouse = false;

    void Start()
    {
        gameObject.tag = "Ore";

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        SetBaseDigTime();
        UpdateDigTimeFromPlayerSpeed();
        AutoSetupOreImage();

        //// 创建进度条
        //CreateProgressBar();
    }

    //void CreateProgressBar()
    //{
    //    if (progressBarObject != null && progressBarInstance == null)
    //    {
    //        // 创建进度条作为矿石的子物体
    //        progressBarInstance = Instantiate(progressBarObject, transform);
    //        progressBarInstance.transform.localPosition = new Vector3(
    //            progressBarOffset.x,
    //            progressBarOffset.y,
    //            -0.1f // Z轴稍微靠前，确保在矿石上方
    //        );
    //        progressBarInstance.transform.localRotation = Quaternion.identity;

    //        // 获取组件
    //        progressBarSR = progressBarInstance.GetComponent<SpriteRenderer>();
    //        progressBarTransform = progressBarInstance.transform;

    //        if (progressBarSR != null)
    //        {
    //            // 设置亮蓝色
    //            progressBarSR.color = new Color(0f, 0.7f, 1f, 0.85f);
    //            originalBarScale = progressBarTransform.localScale;
    //        }
    //        else
    //        {
    //            Debug.LogError("进度条物体没有SpriteRenderer组件！");
    //        }

    //        // 初始隐藏
    //        HideProgressBar();
    //    }
    //    else if (progressBarObject == null)
    //    {
    //        Debug.LogWarning("进度条物体未设置！");
    //    }
    //}

    void SetBaseDigTime()
    {
        baseDigTime = oreType switch
        {
            OreType.Normal => 1.0f,
            OreType.Ruby => 1.0f,
            OreType.Blue => 1.0f,
            OreType.Purple => 1.5f,
            OreType.Hard => float.MaxValue,
            OreType.Lava => 1.0f,
            _ => 1.0f
        };
    }

    public void UpdateDigTimeFromPlayerSpeed()
    {
        if (oreType == OreType.Hard)
        {
            totalDigTime = float.MaxValue;
            return;
        }

        if (GameDateController.Instance != null)
        {
            float playerMineSpeed = GameDateController.Instance.minespeed;
            totalDigTime = baseDigTime / playerMineSpeed;
        }
        else
        {
            totalDigTime = baseDigTime;
        }

        remainingTime = totalDigTime; // 初始化剩余时间
    }

    public void AutoSetupOreImage()
    {
        if (oreUIImage == null || oreSprites == null || oreSprites.Length == 0)
            return;

        int spriteIndex = (int)oreType + 1;
        if (spriteIndex >= 0 && spriteIndex < oreSprites.Length)
        {
            oreUIImage.sprite = oreSprites[spriteIndex];
        }
    }

    void Update()
    {
        if (isBeingDug && remainingTime > 0)
        {
            // 减少剩余时间
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(0, remainingTime);

            // 更新进度条（显示剩余时间比例）
            UpdateProgressBar();

            if (remainingTime <= 0)
            {
                CompleteDigging();
            }
        }
    }

    public void StartDigging()
    {
        if (oreType == OreType.Hard) return;

        if (!isBeingDug)
        {
            isBeingDug = true;
            isMinedByMouse = false;

            // 如果第一次开始挖矿，初始化时间
            if (remainingTime <= 0)
            {
                remainingTime = totalDigTime;
            }

            ShowProgressBar();
        }
    }

    public void StopDigging()
    {
        if (isBeingDug)
        {
            isBeingDug = false;
            HideProgressBar();
        }
    }

    void ShowProgressBar()
    {
        if (progressBarInstance != null)
        {
            progressBarInstance.SetActive(true);
        }
    }

    void HideProgressBar()
    {
        if (progressBarInstance != null)
        {
            progressBarInstance.SetActive(false);
        }
    }

    void UpdateProgressBar()
    {
        if (progressBarTransform != null)
        {
            // 计算剩余时间比例（0-1）
            float timeRatio = remainingTime / totalDigTime;

            // 更新X轴缩放（从右向左减少）
            Vector3 newScale = progressBarTransform.localScale;
            newScale.x = originalBarScale.x * timeRatio;
            progressBarTransform.localScale = newScale;
        }
    }

    void OnMouseDown()
    {
        if (!canClickToMine) return;

        if (FMineModeManager.Instance == null)
        {
            Debug.LogWarning("F键挖矿管理器未找到！");
            return;
        }

        if (FMineModeManager.Instance.TryUseFMouseMine())
        {
            isMinedByMouse = true;
            OreDropSpawner.Instance?.DropOreIcon(this, true);
            ParticlesController.Instance?.PlayParticle(this, 1.0f);

            if (progressBarInstance != null)
                Destroy(progressBarInstance);

            Destroy(gameObject);
        }
        else
        {
            if (!FMineModeManager.Instance.isFMouseMineActive)
                Debug.Log("请先按F键激活鼠标挖矿模式！");
            else if (FMineModeManager.Instance.hasUsedFMouseMine)
                Debug.Log("F键模式已使用，请再次按F键激活");
        }
    }

    void CompleteDigging()
    {
        isMinedByMouse = false;
        OreDropSpawner.Instance?.DropOreIcon(this, false);
        ParticlesController.Instance?.PlayParticle(this, 1.0f);

        if (progressBarInstance != null)
            Destroy(progressBarInstance);

        Destroy(gameObject);
    }

    public float GetRemainingTimeRatio()
    {
        return remainingTime / totalDigTime;
    }

    // 调试方法：强制显示进度条
    public void DebugShowProgressBar()
    {
        if (progressBarInstance != null)
        {
            progressBarInstance.SetActive(true);
            UpdateProgressBar();
        }
    }
}