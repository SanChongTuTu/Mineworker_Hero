using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PowerCrystalManager
{
    public static void InitializeFile(string fileName, int defaultAmount = 0)
    {
        List<PowerCrystalInputEntry> entries = FileHandler.LoadFromJSON<PowerCrystalInputEntry>(fileName);

        // If the list is empty, it means the file didn't exist or was empty.
        if (entries == null || entries.Count == 0)
        {
            Debug.Log($"Creating new Power Crystal file: {fileName}");
            
            // Create a new entry
            PowerCrystalInputEntry newEntry = new PowerCrystalInputEntry(defaultAmount);
            
            // Add to list and save
            entries = new List<PowerCrystalInputEntry> { newEntry };
            FileHandler.SaveToJSON(entries, fileName);
        }
    }
    
    public static void AddCrystals(string fileName, int amountToAdd)
    {
        List<PowerCrystalInputEntry> entries = FileHandler.LoadFromJSON<PowerCrystalInputEntry>(fileName);
        
        PowerCrystalInputEntry entry = entries.FirstOrDefault();
        if (entry == null)
        {
            entry = new PowerCrystalInputEntry(0);
            entries.Add(entry);
        }
        
        entry.number += amountToAdd;
        
        FileHandler.SaveToJSON(entries, fileName);
        
        Debug.Log($"Transaction complete. Added: {amountToAdd}. New Total: {entry.number}");
    }
    
    public static int GetBalance(string fileName)
    {
        List<PowerCrystalInputEntry> entries = FileHandler.LoadFromJSON<PowerCrystalInputEntry>(fileName);
        var entry = entries.FirstOrDefault();
        return entry != null ? entry.number : 0;
    }
}