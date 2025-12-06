using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    public List<GameObject> cards;
    public float yIncrement = 0.2f;
    public float animationSpeed = 5f;
    protected int previousChildCount = 0;
    public DeckAsset deckAsset;
    public GameObject card;

    protected virtual void Start()
    {
        PopulateDeck();
        UpdateCardsArray();
        ArrangeCards();
    }

    protected virtual void Update()
    {
        if (transform.childCount != previousChildCount)
        {
            UpdateCardsArray();
            ArrangeCards();
            previousChildCount = transform.childCount;
        }
    }

    protected virtual void PopulateDeck()
    {
        if (deckAsset == null)
        {
            Debug.LogError("Deck: No deck asset assigned.");
            return;
        }

        List<CardData> masterList = CardParser.ParseCsv();
        cards = new List<GameObject>();

        foreach (string id in deckAsset.cardIDs)
        {
            CardData data = masterList.Find(cd => cd.CardID == id);

            if (data == null)
            {
                Debug.LogWarning($"Card ID '{id}' not found in document.");
                continue;
            }

            GameObject cardObj = Instantiate(card, transform);
            Card c = cardObj.GetComponent<Card>();
            c.LoadFromData(data);
            cardObj.name = c.cardName;

            cards.Add(cardObj);
        }
    }

    protected virtual void UpdateCardsArray()
    {
        cards = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cardObject = transform.GetChild(i).gameObject;
            cards.Add(cardObject);
            Card card = cardObject.GetComponent<Card>();
            card.isDraggable = false;
        }
    }

    public virtual void ArrangeCards()
    {
        if (cards.Count == 0) return;

        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y += i * yIncrement;
            StartCoroutine(AnimateToPosition(cards[i], targetPosition));
        }
    }

    protected IEnumerator AnimateToPosition(GameObject card, Vector3 targetPosition)
    {
        if (card == null) yield break;

        while (Vector3.Distance(card.transform.position, targetPosition) > 0.01f)
        {
            float lerpFactor = Mathf.Min(Time.deltaTime * animationSpeed, 0.5f);
            card.transform.position = Vector3.Lerp(
                card.transform.position,
                targetPosition,
                lerpFactor
            );
            yield return null;
        }

        card.transform.position = targetPosition;
    }
}