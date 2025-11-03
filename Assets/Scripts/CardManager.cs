using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public GameObject Card;
    public Transform cardParent;

    void Start()
    {
        List<CardData> cards = CardParser.ParseCsv();

        foreach (var cardData in cards)
        {
            GameObject cardObj = Instantiate(Card, cardParent);
            Card cardComponent = cardObj.GetComponent<Card>();
            cardComponent.LoadFromData(cardData);
        }
    }
}
