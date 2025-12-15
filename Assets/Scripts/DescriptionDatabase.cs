using System.Collections.Generic;
using UnityEngine;

public static class DescriptionDatabase
{
    private static Dictionary<string, string> idToDescription;
    
    // Lazy loading - loads only when first accessed
    private static Dictionary<string, string> Descriptions
    {
        get
        {
            if (idToDescription == null)
                LoadDescriptions();
            return idToDescription;
        }
    }
    
    private static void LoadDescriptions()
    {
        idToDescription = new Dictionary<string, string>();
        
        TextAsset csvFile = Resources.Load<TextAsset>("descriptions");
        
        string[] lines = csvFile.text.Split(new[] { "\r\n", "\n" }, 
            System.StringSplitOptions.RemoveEmptyEntries);
        
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            
            string[] values = lines[i].Split(',');
            if (values.Length >= 2)
            {
                idToDescription[values[0].Trim()] = values[1].Trim();
            }
        }
        
        var asString = string.Join("\n", idToDescription);
        
        Debug.Log(asString);
    }
    
    public static string GetDescription(string id)
    {
        return Descriptions.TryGetValue(id, out string description) 
            ? description 
            : $"Unknown ID: {id}";
    }
}