using UnityEngine;
using TMPro;

public class AbilityIcon : MonoBehaviour
{
    public SpriteRenderer sprite;
    public TextMeshPro descrText;

    private string abilityName;
    private bool hasDict = false;
    private string desc;

    void Awake()
    {
        Debug.Log($"Awake - TMP enabled: {descrText.enabled}");

    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        descrText.text = " ";
        abilityName = sprite.sprite.name;
    }

    void OnMouseEnter()
    {
        if (!descrText.enabled)
        {
            descrText.enabled = true;
        }
        abilityName = sprite.sprite.name;
        

        desc = DescriptionDatabase.GetDescription(abilityName);
        // Make first letter uppercase
        abilityName = char.ToUpper(abilityName[0]) + abilityName.Substring(1);
        
        descrText.text = $"{abilityName}: {desc}";
        Debug.Log(desc);
    }

    void OnMouseExit()
    {
        descrText.text = " ";
    }
}
