using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfo : MonoBehaviour
{
    [SerializeField] string fileName;
    [SerializeField] string crystalFileName;
    
    public Image logo;
    public Sprite icon;
    public Image smallIcon;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text statusText;
    public TMP_Text costText;
    public string enabledText;
    public string disabledText;
    
    public Material grayscaleMaterial;

    public GameObject skill;
    
    public string title;
    public string description;
    
    [Header("Currency UI")]
    public TMP_Text globalCrystalText;

    private Boolean _status;
    
    private void UpdateGlobalCurrencyDisplay()
    {
        if (globalCrystalText == null) return;
    
        List<PowerCrystalInputEntry> powerCrystalEntries = FileHandler.LoadFromJSON<PowerCrystalInputEntry>(crystalFileName);
        var crystalEntry = powerCrystalEntries.FirstOrDefault();
        if (crystalEntry != null)
        {
            globalCrystalText.text = crystalEntry.number.ToString();
        }
    }

    public void ShowSkillInfo()
    {
        titleText.text = title;
        descriptionText.text = description;
        logo.sprite = icon;
        UpdateGlobalCurrencyDisplay();
        
        if (skill != null)
        {
            Unlock unlockScript = skill.GetComponent<Unlock>();
            if (unlockScript != null && costText != null)
            {
                costText.text = unlockScript.unlockCost.ToString();
            }
        }

        List<SkillActiveInputEntry> entries = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);
        var existingEntry = entries.FirstOrDefault(s => s.skillName == skill.name);
        
        if (existingEntry == null || !existingEntry.isActive)
        {
            statusText.text = disabledText;
            logo.material = grayscaleMaterial; 
            smallIcon.material = grayscaleMaterial;
            return;
        }
        
        statusText.text = enabledText;
        logo.material = null;
        smallIcon.material = null;
    }

    public void UpdateSmallIcon()
    {
        List<SkillActiveInputEntry> entries = FileHandler.LoadFromJSON<SkillActiveInputEntry>(fileName);
        var existingEntry = entries.FirstOrDefault(s => s.skillName == skill.name);
        
        if (existingEntry == null || !existingEntry.isActive)
        {
            smallIcon.material = grayscaleMaterial;
            return;
        }
        smallIcon.material = null;
    }

    private void Start()
    {
        smallIcon.material = grayscaleMaterial;
    }
}
