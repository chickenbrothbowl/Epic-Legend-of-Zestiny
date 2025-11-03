using UnityEngine;
using System.Collections.Generic;

public class CardLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<CardData> cards = CardParser.ParseCsv();
        Debug.Log($"Loaded {cards.Count} cards.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
