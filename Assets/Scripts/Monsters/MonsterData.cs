using System.Collections;
using UnityEngine;
using UnityEngine.UI;

  public class MonsterData : MonoBehaviour
    {
        [Header("怪物初始资源")]
        public Monster monster;

        [Header("结算UI引用")]
        public ResultPanelUI resultPanel; 

        [Header("矿石计算器")]
        public CrystalCalc crystalCalculator;

        void Start()
        {
            // 如果没在Inspector中设置，自动查找
            if (resultPanel == null)
            {
                resultPanel = FindObjectOfType<ResultPanelUI>();
            }

            if (crystalCalculator == null)
            {
                crystalCalculator = FindObjectOfType<CrystalCalc>();
            }

            if (resultPanel == null)
            {
                Debug.LogError("未找到 ResultPanelUI！请确保场景中有该对象。");
            }
        }

      
      public void TakeDamage(int damage)
      {
        monster.MonsterHP -= damage;
        if (monster.MonsterHP < 0) monster.MonsterHP = 0;

        // 检查是否死亡
        if (monster.MonsterHP <= 0)
        {
            OnMonsterDeath();
        }
    }

    // 怪物死亡处理
    void OnMonsterDeath()
    {
        Debug.Log("怪物死亡！");

        // 显示胜利面板
        if (resultPanel != null)
        {
            // 计算获得的超能矿石
            int crystalCount = CalculateVictoryCrystals();

            // 设置并显示胜利面板
            resultPanel.number = crystalCount;
            resultPanel.gameObject.SetActive(true);
            resultPanel.ShowVictory(crystalCount);
        }
        else
        {
            Debug.LogError("ResultPanelUI 未设置！");
        }

    }

    // 计算胜利时获得的超能矿石
    int CalculateVictoryCrystals()
    {
        if (crystalCalculator != null)
        {
            // 需要获取当前层数和其他信息
            int currentLayer = 1; // 假设是第一层，根据实际情况调整
            int monsterMaxHP = monster.maxblood; // 假设Monster类有maxHP属性
            int monsterCurrentHP = 0; // 怪物死亡时当前血量为0
            bool isDefeated = true; // 击败了怪物

            return crystalCalculator.CalcLayerCrystals(
                currentLayer,
                monsterMaxHP,
                monsterCurrentHP,
                isDefeated
            );
        }

        // 如果没有计算器，返回默认值
        return 5; // 默认胜利获得5个矿石
    }

    // 如果需要，可以添加一个获取怪物信息的方法
    public Monster GetMonsterInfo()
    {
        return monster;
    }
}