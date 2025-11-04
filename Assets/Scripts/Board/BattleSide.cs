using UnityEngine;

// Represents one side of the battlefield (player or enemy)
public class BattleSide : MonoBehaviour
{
    public CardSlot[] slots;
    public Player player; // The player/entity this side represents
    
    void Start()
    {
        slots = GetComponentsInChildren<CardSlot>();
    }
    
    public void AttackOpposingSide(BattleSide opponent)
    {
        Debug.Log("Attacking Opposing Side");
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty)
            {
                Card attacker = slots[i].currentCard;
                
                // Check if there's a card to block
                if (i < opponent.slots.Length && !opponent.slots[i].IsEmpty)
                {
                    Card defender = opponent.slots[i].currentCard;
                    CombatResolver.CardVsCard(attacker, defender);
                }
                else
                {
                    CombatResolver.CardVsPlayer(attacker, opponent.player);
                }
            }
        }
    }
}


// Separates combat logic into its own class
public static class CombatResolver
{
    public static void CardVsCard(Card attacker, Card defender)
    {
        Debug.Log("Card Vs Card");
        defender.defenseValue -= attacker.attackValue;
    }
    
    public static void CardVsPlayer(Card attacker, Player target)
    {
        Debug.Log("Card Vs Player");
        target.life -= attacker.attackValue;
    }
}