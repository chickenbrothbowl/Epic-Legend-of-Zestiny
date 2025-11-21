using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckLayout : MonoBehaviour
{
    public List<GameObject> cards;
    public float yIncrement = 0.2f;
    public float animationSpeed = 5f; // Speed of the animation
	public bool canDraw = true;
    private int previousChildCount = 0;
    public GameObject hand;
	private CardHandLayout handLayout;
    public CardManager cardManager;
    public PlayerDeckAsset deckAsset;

    void Start()
    {
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
		if (canDraw){
			// Shake top card
		}
    }

	void OnMouseDown(){
		if (canDraw){
			DrawCard();
		}
	}

    void UpdateCardsArray()
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

	[ContextMenu("Draw Card")]
    public void DrawCard()
    {
        if (cards.Count == 0) return;
        GameObject card = cards[cards.Count - 1];
        cards.Remove(card);
        previousChildCount--;
        card.transform.SetParent(hand.transform);
		StopAllCoroutines();
        // Reset card's animation state
        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent != null)
        {
			cardComponent.isDraggable = true;
            cardComponent.isReturning = false;
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
}