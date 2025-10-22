using UnityEngine;

public class CardSlot : MonoBehaviour
{
    public Card currentCard;
    public int slotIndex;
    
    public bool IsEmpty => currentCard == null;
    
    public void PlaceCard(Card card)
    {
        if (!IsEmpty) return;
        
        currentCard = card;
        card.transform.SetParent(transform);
        card.transform.localPosition = Vector3.zero;
    }
    
    public Card RemoveCard()
    {
        Card card = currentCard;
        currentCard = null;
        return card;
    }
}
