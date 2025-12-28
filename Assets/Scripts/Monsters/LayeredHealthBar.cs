using UnityEngine;
using UnityEngine.UI;

public class LayeredHealthBar : MonoBehaviour
{
    [Header("血条层级")]
    public Image foregroundBar; // 前景条（深红色，立即响应）
    public Image middleBar;     // 中间条（浅红色，延迟减少）
    public Text healthText;     // 血量文本（可选）

    private int maxHealth;
    private float currentForegroundHealth;
    private float currentMiddleHealth;
    private float delayTimer = 0f;
    private float delayDuration = 0.5f; // 延迟时间

    void Update()
    {
        // 如果中间条比前景条高，则延迟减少
        if (currentMiddleHealth > currentForegroundHealth)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delayDuration)
            {
                currentMiddleHealth = Mathf.Lerp(currentMiddleHealth, currentForegroundHealth, Time.deltaTime * 5);
                UpdateBars();
            }
        }
        else
        {
            delayTimer = 0f;
        }
    }

    // 初始化血条
    public void Initialize(int maxHP)
    {
        maxHealth = maxHP;
        currentForegroundHealth = maxHP;
        currentMiddleHealth = maxHP;
        UpdateBars();
    }

    // 更新血量
    public void UpdateHealth(int newHealth)
    {
        currentForegroundHealth = newHealth;
        UpdateBars();
        delayTimer = 0f; // 重置延迟计时器
    }

    // 更新UI显示
    void UpdateBars()
    {
        float foregroundFill = currentForegroundHealth / maxHealth;
        float middleFill = currentMiddleHealth / maxHealth;

        foregroundBar.fillAmount = foregroundFill;
        middleBar.fillAmount = middleFill;

        if (healthText != null)
        {
            healthText.text = currentForegroundHealth + "/" + maxHealth;
        }
    }
}