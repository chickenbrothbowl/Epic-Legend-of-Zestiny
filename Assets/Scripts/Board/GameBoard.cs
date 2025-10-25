using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public CardSlot[] cardSlots;
    
    void Start()
    {
        // Initialize slots if not set in inspector
        cardSlots = GetComponentsInChildren<CardSlot>();
    }
    
    public CardSlot GetSlot(int index)
    {
        return cardSlots[index];
    }
    
    public CardSlot GetEmptySlot()
    {
        return System.Array.Find(cardSlots, slot => slot.IsEmpty);
    }
}
