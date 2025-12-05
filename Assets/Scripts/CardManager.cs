using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public GameObject card;
    public Transform cardParent;

    void Start()
    {
        List<CardData> cards = CardParser.ParseCsv();

        foreach (var cardData in cards)
        {
            GameObject cardObj = Instantiate(card, cardParent);
            Card cardComponent = cardObj.GetComponent<Card>();
            cardComponent.LoadFromData(cardData);
			cardObj.name = cardComponent.cardName;
        }
    }
}
