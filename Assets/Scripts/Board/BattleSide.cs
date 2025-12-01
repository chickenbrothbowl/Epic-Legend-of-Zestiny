using UnityEngine;

// Represents one side of the battlefield (player or enemy)
public class BattleSide : MonoBehaviour
{
    public CardSlot[] slots;
    public Player player; // The player/entity this side represents
    public bool isPlayerSide = false;
   
   
    void Start()
    {
        slots = GetComponentsInChildren<CardSlot>();
    }

    public int TribalCount()
    {
        int count = 0;
        foreach (CardSlot slot in slots)
        {
            if (!slot.IsEmpty && slot.currentCard.tribal)
                count++;
        }
        return count;
    }

    public void ApplyTribalBuffs()
    {
        int tribalCount = TribalCount();
        foreach (CardSlot slot in slots)
        {
            if (!slot.IsEmpty)
            {
                Card c = slot.currentCard;
                if (c.tribal)
                {
                    c.ApplyAttackBonus(tribalCount);
                }
            }
        }
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

    void OnEnable()
    {
        Card.OnCardDied += HandleAllyDeath;
        Card.OnCardDied += HandleAllyDeathGluttonous;
    }

    void OnDisable()
    {
        Card.OnCardDied -= HandleAllyDeath;
        Card.OnCardDied -= HandleAllyDeathGluttonous;

    }

    private void HandleAllyDeath(Card deadCard, BattleSide side)
    {
        if (side != this) return;

        foreach (CardSlot slot in slots)
        {
            if (!slot.IsEmpty)
            {
                Card c = slot.currentCard;
                if (c.opportunist)
                {
                    if (deadCard.opportunist) return;
                    c.SetBaseAttack(c.GetBaseAttack() + 1);
                    c.ApplyAttackBonus(0);
                    c.ApplyDefenseBonus(0);
                }
            }
        }
    }

    private void HandleAllyDeathGluttonous(Card deadCard, BattleSide side)
    {
        if (side != this) return;
        if (!deadCard.tribal) return;

        foreach (CardSlot slot in slots)
        {
            if (!slot.IsEmpty)
            {
                Card c = slot.currentCard;
                if (c.gluttenous)
                {
                    c.SetBaseDefense(c.GetBaseDefense() + 2);
                    c.ApplyDefenseBonus(0);
                }
            }
        }
    }


}

// Separates combat logic into its own class
public static class CombatResolver
// Combat keywords: acidic, corrosive, finesse, flying, hardened, pummel, reach, vampire
// Static keywords: harvest, juicy, rotten, tribal
// Triggered keywords: catch, gluttenous, juiced, opportunist
{
    public static void CardVsCard(Card attacker, Card defender, Player target, Player you)
    {
        Debug.Log("Card Vs Card");

        if (defender.rotten && !attacker.hardened && !attacker.flying)
        {
            attacker.poisoned = true;
            Debug.Log("Attacker has been poisoned!");
        }

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
        else if (defender.finesse)
        {
            if (defender.attackValue > attacker.defenseValue) // No >= as temp fix
            {
                // KNOWN ISSUE: Finesse sometimes sets defense=2 defender defense to 1 if = is present? Will look into it
                Debug.Log("Finesse activated");
                Debug.Log("Defender Attack: " + defender.attackValue);
                Debug.Log("Attacker Defense: " + attacker.defenseValue);
                attacker.defenseValue = 0;
            }
            else
            {
                defender.defenseValue -= attacker.attackValue;
            }

            if (attacker.acidic)
            {
                defender.defenseValue -= 1;
            }

            if (attacker.corrosive)
            {
                defender.attackValue -= 1;
            }
        }
        else if (attacker.pummel && (attacker.attackValue > defender.defenseValue))
        {
            if (defender.acidic)
            {
                attacker.defenseValue -= 1;
            }

            if (defender.corrosive)
            {
                attacker.attackValue -= 1;
            }  
            target.life -= attacker.attackValue - defender.defenseValue;
            defender.defenseValue -= attacker.attackValue;
        }
        else if (defender.acidic && !attacker.hardened)
        {
            attacker.defenseValue -= 1;
            defender.defenseValue -= attacker.attackValue;
        }
        else if (defender.corrosive && !attacker.hardened)
        {
            attacker.attackValue -= 1;
            defender.defenseValue -= attacker.attackValue;
        }
        else { defender.defenseValue -= attacker.attackValue; }
    }

    public static void CardVsPlayer(Card attacker, Player target, Player you)
    {
        Debug.Log("Card Vs Player");
        target.DealDamage(attacker.attackValue);
       
        if (attacker.vampire)
        {
        you.life += attacker.attackValue;
        }
    }
}

