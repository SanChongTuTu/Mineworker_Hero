using UnityEngine;
using UnityEngine.UI;

public class TimeProgressBar : MonoBehaviour
{
    [Header("时间进度条")]
    public RectTransform timeBar;
    public float totalTime = 60f;
    public float barMaxWidth = 400f;

    [Header("角色瞬移设置")]
    public GameObject playerObject; // 直接拖拽玩家GameObject
    public float playerTargetY = -25f; // 角色要移动到的Y位置（和相机一样）

    private float currentTime;
    private bool isTimerRunning = true;
    private bool hasTriggered = false;

    void Start()
    {
        currentTime = totalTime;
        UpdateProgressBar();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(0, currentTime);
            UpdateProgressBar();

            if (currentTime <= 0 && !hasTriggered)
            {
                OnTimeUp();
            }
        }
    }

    void UpdateProgressBar()
    {
        if (timeBar != null)
        {
            float progress = currentTime / totalTime;
            Vector2 size = new Vector2(progress * barMaxWidth, timeBar.sizeDelta.y);
            timeBar.sizeDelta = size;
        }
    }

    void OnTimeUp()
    {
        isTimerRunning = false;
        hasTriggered = true;
        Debug.Log("时间到！开始处理角色瞬移...");

        // 1. 先触发相机移动
        if (CameraController.Instance != null)
        {
            CameraController.Instance.StartMoveToTarget();
        }

        // 2. 强制角色瞬移到指定位置
        MovePlayerToTarget();
    }

    void MovePlayerToTarget()
    {
        if (playerObject != null)
        {
            // 直接设置角色的位置
            Vector3 newPosition = playerObject.transform.position;
            newPosition.y = playerTargetY; // 移动到目标Y位置
            playerObject.transform.position = newPosition;

            Debug.Log($"角色已瞬移到 Y={playerTargetY}，位置: {newPosition}");

            // 强制输出位置信息，确保确实移动了
            Debug.Log($"移动前位置: {playerObject.transform.position}");
            Debug.Log($"移动后位置: {newPosition}");
        }
        else
        {
            Debug.LogError("TimeProgressBar: 玩家对象未分配！请拖拽玩家到playerObject字段");
        }
    }

    public void ResetTimer()
    {
        currentTime = totalTime;
        isTimerRunning = true;
        hasTriggered = false;
        UpdateProgressBar();
    }

    
    public void ForceTimeZero()
    {
        currentTime = 0f;
        isTimerRunning = false;
        UpdateProgressBar();

        if (!hasTriggered)
        {
            OnTimeUp();
        }

        Debug.Log("时间进度条强制归零");
    }
}