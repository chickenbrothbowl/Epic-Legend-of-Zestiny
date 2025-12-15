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
        Shuffle();
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

    protected virtual void Shuffle()
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            GameObject temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }

        // Re-parent cards in the new order to match the list
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetSiblingIndex(i);
        }

        ArrangeCards();
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
            cardObj.name = c.Data.CardName;

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
        
        Quaternion targetRotation = transform.rotation;
        // Optionally add per-card rotation offset here, e.g.:
        // targetRotation *= Quaternion.Euler(0f, i * rotationIncrement, 0f);
        
        StartCoroutine(AnimateToPositionAndRotation(cards[i], targetPosition, targetRotation));
    }
}

protected IEnumerator AnimateToPositionAndRotation(GameObject card, Vector3 targetPosition, Quaternion targetRotation)
{
    if (card == null) yield break;

    while (Vector3.Distance(card.transform.position, targetPosition) > 0.01f ||
           Quaternion.Angle(card.transform.rotation, targetRotation) > 0.5f)
    {
        float lerpFactor = Mathf.Min(Time.deltaTime * animationSpeed, 0.5f);
        
        card.transform.position = Vector3.Lerp(
            card.transform.position,
            targetPosition,
            lerpFactor
        );
        
        card.transform.rotation = Quaternion.Slerp(
            card.transform.rotation,
            targetRotation,
            lerpFactor
        );
        
        yield return null;
    }

    card.transform.position = targetPosition;
    card.transform.rotation = targetRotation;
}
}