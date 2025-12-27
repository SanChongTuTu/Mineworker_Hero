using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterData : MonoBehaviour
{
    [Header("怪物初始资源")]
    public Monster monster;
    [Header("怪物属性")]
    public int maxHealth = 10;
    public int currentHealth;
    public int attackPower = 2;

    [Header("血条UI")]
    public Image healthbarquick; // 血条填充图片
    public Image healthbarslow;  // 血条慢速填充图片

    [Header("UI提示框")]
    public GameObject resultPanelPrefab; // 结果面板预制体

    void Start()
    {
        currentHealth = maxHealth;
        healthbarquick.rectTransform.sizeDelta = new Vector2(600,30);
        healthbarslow.rectTransform.sizeDelta = new Vector2(600, 30);
    }

    private void Update()
    {
        
    }

    public void Decreaseblood(float aimtime)
    {
        StartCoroutine(DecreaseBlood(aimtime));
    }

    IEnumerator DecreaseBlood(float aimtime)
    {
        // 更新血条UI
        float healthRatio = (float)currentHealth / maxHealth;
        healthbarquick.rectTransform.sizeDelta = new Vector2(600 * healthRatio, 30);

        float time = 0;
        float start = healthbarslow.rectTransform.sizeDelta.x;

        while (time < aimtime)
        {
                float nowhealthbarslow = Mathf.Lerp(start, healthRatio * 600,time /aimtime);
                healthbarslow.rectTransform.sizeDelta = new Vector2(nowhealthbarslow, 30);
                time += Time.deltaTime;
                yield return null;
        }

        if (healthbarslow.rectTransform.sizeDelta.x < 600 * healthRatio)
        {
            healthbarslow.rectTransform.sizeDelta = new Vector2(healthRatio * 600, 30);
        }

        yield break;
    }

    // 怪物受到伤害
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // 更新血条
        //if (healthBar != null)
        //{
        //    healthBar.UpdateHealth(currentHealth);
        //}

        // 检查是否死亡
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    // 怪物死亡
    void OnDeath()
    {
        Debug.Log("怪物死亡");
        ShowResultPanel("胜利");
    }

    // 玩家死亡（可以从其他脚本调用）
    public void PlayerDeath()
    {
        ShowResultPanel("失败");
    }

    // 显示结果面板
    void ShowResultPanel(string result)
    {
        if (resultPanelPrefab != null)
        {
            GameObject panel = Instantiate(resultPanelPrefab, Vector3.zero, Quaternion.identity);
            panel.transform.SetParent(GameObject.Find("Canvas").transform, false); // 假设Canvas在场景中

            // 设置面板文本和按钮（需要UI脚本支持）
            //ResultPanelUI panelUI = panel.GetComponent<ResultPanelUI>();
            //if (panelUI != null)
            //{
            //    panelUI.SetResultText(result);
            //}
        }
    }
}