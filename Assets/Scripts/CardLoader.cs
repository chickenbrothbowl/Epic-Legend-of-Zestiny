using UnityEngine;
using System.Collections.Generic;

public class CardLoader : MonoBehaviour
{
    void Start()
    {
        List<CardData> cards = CardParser.ParseCsv();
        Debug.Log($"Loaded {cards.Count} cards.");
    }

    void Update()
    {
        
    }
}
