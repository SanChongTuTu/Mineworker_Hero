using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DepthProgressBar : MonoBehaviour
{
    [Header("深度进度条")]
    public RectTransform depthBar;
    public Transform player;
    public float maxDepth = 30f;
    public float barMaxHeight = 350f;

    [Header("时间进度条引用")]
    public TimeProgressBar timeProgressBar; // 引用时间进度条

    private float startY;
    private float originalBarHeight;
    private bool hasTriggered = false;

    void Start()
    {
        if (player != null)
        {
            startY = player.position.y;
        }
        if (depthBar != null)
        {
            originalBarHeight = depthBar.sizeDelta.y;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float currentDepth = Mathf.Max(0, startY - player.position.y);
            float progress = currentDepth / maxDepth;

            // 检查深度是否达到最大值（进度条为0）
            if (progress >= 1f && !hasTriggered)
            {
                hasTriggered = true;
               

                // 1. 触发相机移动
                if (CameraController.Instance != null)
                {
                    CameraController.Instance.StartMoveToTarget();
                }

                // 2. 将时间进度条也归零
                ForceTimeToZero();
            }

            if (depthBar != null)
            {
                float bary = (1 - progress) * barMaxHeight;
                bary=Mathf.Clamp(bary, 0, barMaxHeight);
                Vector2 size = new Vector2(depthBar.sizeDelta.x, bary);
                depthBar.sizeDelta = size;
            }
        }
    }

    // 强制时间进度条归零
    void ForceTimeToZero()
    {
        if (timeProgressBar != null)
        {
            
            
            timeProgressBar.ForceTimeZero();
        }
        else
        {
            Debug.LogWarning("时间进度条引用未设置，无法同步归零");
        }
    }

    public void ResetDepth()
    {
        if (player != null)
        {
            startY = player.position.y;
        }
        hasTriggered = false;
    }
}