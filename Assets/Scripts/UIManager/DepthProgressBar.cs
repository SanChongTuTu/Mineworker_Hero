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
                Debug.Log("深度到达底部！");

                // 触发相机移动
                if (CameraController.Instance != null)
                {
                    CameraController.Instance.StartMoveToTarget();
                }
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

    public void ResetDepth()
    {
        if (player != null)
        {
            startY = player.position.y;
        }
        hasTriggered = false;
    }
}