using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class EnemyDeck : MonoBehaviour
{
    public List<GameObject> cards;
    public float yIncrement = 0.2f;
    public float animationSpeed = 5f; // Speed of the animation
    private int previousChildCount = 0;
    public BattleSide Side;
    public CardManager cardManager;
    public EnemyDeckAsset deckAsset;

    void Start()
    {
        PopulateDeck();
        UpdateCardsArray();
        ArrangeCards();
    }

    void Update()
    {
        // Only update the array if the number of children changed
        if (transform.childCount != previousChildCount)
        {
            UpdateCardsArray();
            ArrangeCards();
            previousChildCount = transform.childCount;
        }
    }

	void OnMouseDown(){
		DeckPlay();
	}

    void UpdateCardsArray()
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

	[ContextMenu("DeckPlay")]
    public void DeckPlay()
    {
        if (cards.Count == 0) return;
		Debug.Log("DeckPlayed");
        GameObject card = cards[cards.Count - 1];
        cards.Remove(card);
        previousChildCount--;
        
        // Reset card's animation state
        Card cardComponent = card.GetComponent<Card>();

        foreach (CardSlot slot in Side.slots)
        {
            if (slot.IsEmpty)
            {
                slot.PlaceCard(cardComponent);
                return;
            }
        }
    }

    public void ArrangeCards()
    {
        if (cards.Count == 0) return;
        // Calculate the offset to center the cards
        float totalHeight = (cards.Count - 1) * yIncrement;

        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y += i * yIncrement;
            // Animate to the target position
            StartCoroutine(AnimateToPosition(cards[i], targetPosition));
        }
    }

    IEnumerator AnimateToPosition(GameObject card, Vector3 targetPosition)
    {
        if (card == null) yield break;

        while (Vector3.Distance(card.transform.position, targetPosition) > 0.01f)
        {
            // Clamp lerp factor to prevent instant snapping on frame time spikes
            float lerpFactor = Mathf.Min(Time.deltaTime * animationSpeed, 0.5f);
            card.transform.position = Vector3.Lerp(
                card.transform.position,
                targetPosition,
                lerpFactor
            );
            yield return null;
        }

        // Snap to final position
        card.transform.position = targetPosition;
    }
    
     void PopulateDeck()
    {
        if (deckAsset == null)
        {
            Debug.LogError("EnemyDeck: No deck asset assigned.");
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

            GameObject cardObj = Instantiate(cardManager.card, transform);
            Card c = cardObj.GetComponent<Card>();
            c.LoadFromData(data);
            cardObj.name = c.cardName;

            cards.Add(cardObj);
        }
    }
}