using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStatsInitializer : MonoBehaviour
{
    [SerializeField] private string skillFileName = "skills.json";
    [SerializeField] private string crystalFileName = "crystal.json";

    [Header("Base Stats (Starting Values)")]
    [SerializeField] private float baseHealth = 4f;
    [SerializeField] private float baseAttack = 1f;
    [SerializeField] private float baseStrength = 1.0f;
    [SerializeField] private float baseMineSpeed = 1.0f;
    [SerializeField] private float baseMoveSpeed = 1f;
    [SerializeField] private float baseCriticalChance = 1.0f;
    [SerializeField] private float baseMiningFortune = 1.0f;
    [SerializeField] private float baseAbsorptionCount = 1.0f;
    [SerializeField] private float baseDimensionalPickaxeCount = 1.0f;
    
    public enum StatType { Health, Attack, Strength, MineSpeed, MoveSpeed ,CriticalChance, MiningFortune, AbsorptionCount, DimensionalPickaxeCount }
    
    [System.Serializable]
    public class StatBonus
    {
        public StatType targetStat;
        public float multiplier = 1.1f;
    }
    
    [System.Serializable]
    public class SkillConfig
    {
        public string skillName;
        public List<StatBonus> bonuses;
    }

    [Header("Skill Configuration")]
    public List<SkillConfig> allSkills;

    public void InitializeGameStats()
    {
        List<SkillActiveInputEntry> savedSkills = FileHandler.LoadFromJSON<SkillActiveInputEntry>(skillFileName);
        
        float finalHealth = baseHealth;
        float finalAttack = baseAttack;
        float finalStrength = baseStrength;
        float finalMineSpeed = baseMineSpeed;
        float finalMoveSpeed = baseMoveSpeed;
        float finalCriticalChance = baseCriticalChance;
        float finalMiningFortune = baseMiningFortune;
        float finalAbsorptionCount = baseAbsorptionCount;
        float finalDimensionalPickaxeCount = baseDimensionalPickaxeCount;
        
        foreach (var skillConfig in allSkills)
        {
            if (IsSkillActive(savedSkills, skillConfig.skillName))
            {
                foreach (var bonus in skillConfig.bonuses)
                {
                    switch (bonus.targetStat)
                    {
                        case StatType.Health:
                            finalHealth *= bonus.multiplier;
                            break;
                        case StatType.Attack:
                            finalAttack *= bonus.multiplier;
                            break;
                        case StatType.Strength:
                            finalStrength *= bonus.multiplier;
                            break;
                        case StatType.MineSpeed:
                            finalMineSpeed *= bonus.multiplier;
                            break;
                        case StatType.MoveSpeed:
                            finalMoveSpeed *= bonus.multiplier;
                            break;
                        case StatType.CriticalChance:
                            finalCriticalChance *= bonus.multiplier;
                            break;
                        case StatType.MiningFortune:
                            finalMiningFortune *= bonus.multiplier;
                            break;
                        case StatType.AbsorptionCount:
                            finalAbsorptionCount *= bonus.multiplier;
                            break;
                        case StatType.DimensionalPickaxeCount:
                            finalDimensionalPickaxeCount *= bonus.multiplier;
                            break;
                    }
                }
                Debug.Log($"[Stats] Applied bonuses from: {skillConfig.skillName}");
            }
        }
        
        if (GameDateController.Instance != null)
        {
            GameDateController.Instance.blood = finalHealth;
            GameDateController.Instance.attack = finalAttack;
            GameDateController.Instance.strength = finalStrength;
            GameDateController.Instance.minespeed = finalMineSpeed;
            GameDateController.Instance.movespeed = finalMoveSpeed;
            GameDateController.Instance.criticalChance = finalCriticalChance;
            GameDateController.Instance.miningFortune = finalMiningFortune;
            GameDateController.Instance.absorptionCount = finalAbsorptionCount;
            GameDateController.Instance.dimensionalPickaxeCount = finalDimensionalPickaxeCount;

            Debug.Log(
                $"Stats Initialized! Health: {finalHealth}; Attack: {finalAttack}; Strength: {finalStrength}; Mining Speed: {finalMineSpeed}; Move Speed: {finalMoveSpeed}; Critical Chance: {finalCriticalChance}; Mining Fortune: {finalMiningFortune}; Absorption Count: {finalAbsorptionCount}; Dimensional Pickaxe Count: {finalDimensionalPickaxeCount}");
        }
        else
        {
            Debug.LogError("GameDateController not found!");
        }

        if(SceneController.Instance != null)
        {
            SceneController.Instance.ToScene(2);
        }
    }

    private bool IsSkillActive(List<SkillActiveInputEntry> entries, string targetSkillName)
    {
        if (entries == null || entries.Count == 0) return false;
        var entry = entries.FirstOrDefault(s => s.skillName == targetSkillName);
        return entry != null && entry.isActive;
    }
}