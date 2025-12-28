using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterData : MonoBehaviour
{
    [Header("怪物初始资源")]
    public Monster monster;

    void Start()
    {

    }

    private void Update()
    {
        
    }

    // 怪物受到伤害
    public void TakeDamage(int damage)
    {
        monster.MonsterHP -= damage;
        if (monster.MonsterHP < 0) monster.MonsterHP = 0;

        // 更新血条
        //if (healthBar != null)
        //{
        //    healthBar.UpdateHealth(currentHealth);
        //}

        // 检查是否死亡
        if (monster.MonsterHP <= 0)
        {
            
        }
    }
}