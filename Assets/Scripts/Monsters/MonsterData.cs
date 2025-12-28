using UnityEngine;

public class MonsterData : MonoBehaviour
{
    [Header("怪物属性")]
    public int maxHealth = 10;
    public int currentHealth;
    public int attackPower = 2;

    [Header("血条UI")]
    public LayeredHealthBar healthBar; // 分层血条脚本

    [Header("UI引用")]
    public ResultPanelUI resultPanel;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.Initialize(maxHealth);
        }
    }

    // 怪物受到伤害
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // 更新血条
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

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
        ShowVictory();
    }

    // 显示胜利面板
    // 显示胜利面板
    void ShowVictory()
    {
        if (resultPanel != null)
        {
            // 不再传递矿石数量，脚本内部固定为10
            resultPanel.ShowVictory();
        }
        else
        {
            Debug.LogError("结果面板引用未设置！");
        }
    }
    // 计算获得的超能矿石
    int CalculateOreGained()
    {
        // 根据策划案逻辑实现
        // 暂时返回随机值
        return Random.Range(10, 30);
    }
}