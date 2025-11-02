using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    [SerializeField] string fileName;
    
    public GameObject parentSkill;
    public GameObject skill;

    public void UnlockSkill()
    {
        string parentName = parentSkill.name;
        
        List<SkillActiveInputEntry> entries = new List<SkillActiveInputEntry>();
        
        entries.Add(new SkillActiveInputEntry(skill.name, true));
        
        FileHandler.SaveToJSON<SkillActiveInputEntry>(entries, fileName);
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

    private void Start()
    {
        
    }
}
