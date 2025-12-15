using UnityEngine;

public class EnemyDeck : Deck
{
    public BattleSide Side;

    void OnMouseDown()
    {
        return;
        //DeckPlay();
    }

    [ContextMenu("DeckPlay")]
    public void DeckPlay()
    {
        if (cards.Count == 0) return;
        Debug.Log("DeckPlayed");
        GameObject card = cards[cards.Count - 1];
        cards.Remove(card);
        previousChildCount--;

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
}