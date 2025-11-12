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
                    CombatResolver.CardVsCard(attacker, defender, opponent.player, this.player);
                }
                else
                {
                    CombatResolver.CardVsPlayer(attacker, opponent.player, this.player);
                }
            }
        }
    }
}


// Separates combat logic into its own class
public static class CombatResolver
// Combat keywords: acidic, corrosive, finesse, flying, hardened, pummel, reach, vampire
// ETB keywords:
{
    public static void CardVsCard(Card attacker, Card defender, Player target, Player you)
    {
        Debug.Log("Card Vs Card");

        if (attacker.vampire)
        {
            if (!defender.flying && !defender.reach)
            {
                target.life -= attacker.attackValue;
                you.life += attacker.attackValue;
            }
            else
            {
                defender.defenseValue -= attacker.attackValue;
                you.life += attacker.attackValue;
            }
        }
        else if (attacker.flying && !defender.flying && !defender.reach)
        {
            target.life -= attacker.attackValue;
        }
        else if (attacker.pummel && (attacker.attackValue > defender.defenseValue))
        {
            target.life -= attacker.attackValue - defender.defenseValue;
            defender.defenseValue -= attacker.attackValue;
        }
        else { defender.defenseValue -= attacker.attackValue; }
    }

    public static void CardVsPlayer(Card attacker, Player target, Player you)
    {
        Debug.Log("Card Vs Player");
        target.life -= attacker.attackValue;
        
        if (attacker.vampire)
        {
        you.life += attacker.attackValue;
        }
    }
}