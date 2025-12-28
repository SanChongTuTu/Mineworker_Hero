using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    [SerializeField] string fileName;
    [SerializeField] private string crystalFileName;

    public GameObject notEnoughPCPanel;
    
    [Header("Currency UI")]
    public TMP_Text globalCrystalText;
    
    [Header("Skill Settings")]
    public int unlockCost;
    
    public List<GameObject> parentSkills = new List<GameObject>();
    public GameObject skill;
    
    public List<GameObject> childSkills = new List<GameObject>();

    private int _powerCrystalNumber;
    
    private ShowInfo _showInfo;
    
    private void RefreshCurrencyUI()
    {
        if (globalCrystalText == null) return;

        List<PowerCrystalInputEntry> powerCrystalEntries = FileHandler.LoadFromJSON<PowerCrystalInputEntry>(crystalFileName);
        var crystalEntry = powerCrystalEntries.FirstOrDefault();
    
        if (crystalEntry != null)
        {
            globalCrystalText.text = crystalEntry.number.ToString();
        }
    }

    public async void UnlockSkill()
    {
        if (CheckStatus(skill))
        {
            Debug.Log("Skill is already active");
            return;
        }

        if (!IsAnyParentUnlocked())
        {
            Debug.Log("No valid parent skill is unlocked. Cannot unlock.");
            return;
        }
        
        List<PowerCrystalInputEntry> powerCrystalEntries = FileHandler.LoadFromJSON<PowerCrystalInputEntry>(crystalFileName);
        var crystalEntry = powerCrystalEntries.FirstOrDefault();
        
        if (crystalEntry != null)
        {
            if (crystalEntry.number < unlockCost)
            {
                Debug.Log($"Not enough Power Crystals! Cost: {unlockCost}, Current: {crystalEntry.number}");
                notEnoughPCPanel.SetActive(true);
                await Task.Delay(3000);
                notEnoughPCPanel.SetActive(false);
                RefreshCurrencyUI();
                return;
            }
            
            crystalEntry.number -= unlockCost;
            _powerCrystalNumber = crystalEntry.number;
            
            FileHandler.SaveToJSON<PowerCrystalInputEntry>(powerCrystalEntries, crystalFileName);
            RefreshCurrencyUI();
            Debug.Log($"Spent {unlockCost} crystals. Remaining: {crystalEntry.number}");
            // 1. Load the current list of unlocked skills
            List<SkillActiveInputEntry> skillList = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);

            SkillActiveInputEntry newSkillEntry = new SkillActiveInputEntry(skill.name, true);
            
            skillList.Add(newSkillEntry);
            FileHandler.SaveToJSON<SkillActiveInputEntry>(skillList, fileName);

            Debug.Log($"{skill.name} has been successfully unlocked and saved to {fileName}!");
        }
        else
        {
            Debug.LogError("No PowerCrystalInputEntry found in save file!");
            RefreshCurrencyUI();
            return;
        }
    }

    public void LockSkill()
    {
        List<SkillActiveInputEntry> entries = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);
        
        var skillEntry = entries.FirstOrDefault(s => s.skillName == skill.name);
        
        if (skillEntry != null)
        {
            entries.Remove(skillEntry);
            FileHandler.SaveToJSON<SkillActiveInputEntry>(entries, fileName);
            Debug.Log($"Removed skill: {skill.name}");

            // Give the money back
            
            if (unlockCost > 0)
            {
                List<PowerCrystalInputEntry> powerCrystalEntries = FileHandler.LoadFromJSON<PowerCrystalInputEntry>(crystalFileName);
                var crystalEntry = powerCrystalEntries.FirstOrDefault();
                
                if (crystalEntry != null)
                {
                    crystalEntry.number += unlockCost;
                    FileHandler.SaveToJSON<PowerCrystalInputEntry>(powerCrystalEntries, crystalFileName);
                    Debug.Log($"Refunded {unlockCost} crystals. New Total: {crystalEntry.number}");
                    RefreshCurrencyUI();
                }
            }
            
        }
        
        foreach (GameObject child in childSkills)
        {
            if (child != null)
            {
                Unlock childUnlockScript = child.GetComponent<Unlock>();
                if (childUnlockScript != null)
                {
                    childUnlockScript.LockSkill();
                }
                
                ShowInfo childInfoScript = child.GetComponent<ShowInfo>();
                if (childInfoScript != null)
                { 
                    childInfoScript.UpdateSmallIcon();
                }
            }
        }
    }

    public void TestRead()
    {
        List<SkillActiveInputEntry> skillList = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);
        
        var skillLoaded = skillList.FirstOrDefault(s => s.skillName == skill.name);

        if (skillLoaded != null)
        {
            bool isActive = skillLoaded.isActive;
            Debug.Log($"{skill.name} is active: {isActive}");
        }
        else
        {
            Debug.Log($"Skill '{skill.name}' not found");
        }
        
    }

    public Boolean CheckStatus(GameObject skill)
    {
        if (skill.name == "ParentNull")
        {
            return true;
        }
        
        List<SkillActiveInputEntry> skillList = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);
        
        var skillLoaded = skillList.FirstOrDefault(s => s.skillName == skill.name);

        if (skillLoaded != null)
        {
            bool isActive = skillLoaded.isActive;
            return isActive;
        }
        else
        {
            return false;
        }
    }

    private bool IsAnyParentUnlocked()
    {
        if (parentSkills.Count == 0) return false; 

        foreach (GameObject parent in parentSkills)
        {
            if (parent != null)
            {
                if (CheckStatus(parent))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
}
