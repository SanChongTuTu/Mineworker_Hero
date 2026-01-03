using UnityEngine;

public class CrystalCalc : MonoBehaviour
{
    [System.Serializable]
    public class Config
    {
        public int totalLayers = 3;
        public int[] baseCrystals = { 1, 2, 3 };
        public float healthPercent = 0.25f;
    }

    public Config config;

    /// <summary>
    /// 计算单层获得的超能矿石
    /// </summary>
    /// <param name="layer">当前层数（1-3）</param>
    /// <param name="monsterMaxHP">怪物最大生命值</param>
    /// <param name="monsterCurrentHP">怪物当前生命值</param>
    /// <param name="isDefeated">是否击败怪物</param>
    /// <returns>获得的超能矿石数量</returns>
    public int CalcLayerCrystals(int layer, int monsterMaxHP, int monsterCurrentHP, bool isDefeated)
    {
        if (layer < 1 || layer > config.totalLayers) return 0;

        int crystals = 0;

        // 1. 挖矿结束默认获得
        crystals += 1;

        // 2. 战斗伤害奖励
        int damageDealt = monsterMaxHP - monsterCurrentHP;
        int damageThreshold = Mathf.FloorToInt(monsterMaxHP * config.healthPercent);

        if (damageThreshold > 0)
        {
            int damageRewards = (damageDealt / damageThreshold) * config.baseCrystals[layer - 1];
            crystals += damageRewards;
        }

        // 3. 击败奖励
        if (isDefeated)
        {
            crystals += config.baseCrystals[layer - 1];
        }

        return crystals;
    }

    /// <summary>
    /// 计算整局游戏总超能矿石
    /// </summary>
    public int CalcTotalCrystals(int[] layerResults)
    {
        int total = 0;
        foreach (int crystals in layerResults)
        {
            total += crystals;
        }
        return total;
    }

    /// <summary>
    /// 示例：计算整局游戏（3层）的矿石收益
    /// </summary>
    public int CalcGameCrystals(
        int monster1MaxHP, int monster1CurrentHP, bool defeat1,
        int monster2MaxHP, int monster2CurrentHP, bool defeat2,
        int monster3MaxHP, int monster3CurrentHP, bool defeat3)
    {
        int layer1 = CalcLayerCrystals(1, monster1MaxHP, monster1CurrentHP, defeat1);
        int layer2 = CalcLayerCrystals(2, monster2MaxHP, monster2CurrentHP, defeat2);
        int layer3 = CalcLayerCrystals(3, monster3MaxHP, monster3CurrentHP, defeat3);

        return layer1 + layer2 + layer3;
    }
}

// 使用示例类
public class GameManager : MonoBehaviour
{
    public CrystalCalc crystalCalc;

    void Start()
    {
        // 示例数据：第1层
        int monster1MaxHP = 20;
        int monster1CurrentHP = 5; // 造成15点伤害
        bool defeat1 = true; // 击败了怪物

        // 计算第1层矿石
        int layer1Crystals = crystalCalc.CalcLayerCrystals(
            1, monster1MaxHP, monster1CurrentHP, defeat1);
        Debug.Log($"第1层获得超能矿石: {layer1Crystals}");

        // 计算整局游戏
        int total = crystalCalc.CalcGameCrystals(
            20, 5, true,   // 第1层
            30, 8, true,   // 第2层
            40, 0, true    // 第3层
        );
        Debug.Log($"整局游戏总超能矿石: {total}");
    }
}