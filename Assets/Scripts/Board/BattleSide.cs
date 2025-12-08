using UnityEngine;

public class BattleSide : MonoBehaviour
{
    public CardSlot[] slots;
    public Player player;
    public bool isPlayerSide = false;

    void Start()
    {
        slots = GetComponentsInChildren<CardSlot>();
<<<<<<< HEAD
        AudioManager.Instance.PlayBackgroundMusic();
        AudioManager.Instance.PlayBackgroundMusic();
=======

        if (player != null)
        {
            player.battleSide = this;
        }
>>>>>>> e62b6af73c97b3a3f48cdf57e5e897d2b52577a8
    }

    public int TribalCount()
    {
        int count = 0;
        foreach (CardSlot slot in slots)
        {
            if (!slot.IsEmpty && slot.currentCard.HasAbility(Ability.Tribal))
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
                if (c.HasAbility(Ability.Tribal))
                {
                    c.ApplyAttackBonus(tribalCount);
                }
            }
        }
    }

    public void AttackOpposingSide(BattleSide opponent)
    {
        Debug.Log("Attacking " + opponent + " Side");
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty)
            {
                Card attacker = slots[i].currentCard;

                if (i < opponent.slots.Length && !opponent.slots[i].IsEmpty)
                {
                    Card defender = opponent.slots[i].currentCard;
                    CombatResolver.CardVsCard(attacker, defender, opponent.player, this.player);
                }
                else
                {
                    if (isPlayerSide == true)
                    {
                        Debug.Log("Attacking Enemy");
                        CombatResolver.CardVsPlayer(attacker, opponent.player, this.player);
                    }
                    else
                    {
                        Debug.Log("Attacking Player");
                        CombatResolver.CardVsPlayer(attacker, opponent.player, this.player);
                    }
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
                if (c.HasAbility(Ability.Opportunist))
                {
                    if (deadCard.HasAbility(Ability.Opportunist)) return;
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
        if (!deadCard.HasAbility(Ability.Tribal)) return;

        foreach (CardSlot slot in slots)
        {
            if (!slot.IsEmpty)
            {
                Card c = slot.currentCard;
                if (c.HasAbility(Ability.Gluttenous))
                {
                    c.SetBaseDefense(c.GetBaseDefense() + 2);
                    c.ApplyDefenseBonus(0);
                }
            }
        }
    }
}

public static class CombatResolver
{
    // Combat keywords: acidic, corrosive, finesse, flying, hardened, pummel, reach, vampire
    // Static keywords: harvest, juicy, rotten, tribal
    // Triggered keywords: catch, gluttenous, juiced, opportunist

    public static void CardVsCard(Card attacker, Card defender, Player target, Player you)
    {
        Debug.Log("Card Vs Card");

        if (defender.HasAbility(Ability.Rotten) && 
            !attacker.HasAbility(Ability.Hardened) && 
            !attacker.HasAbility(Ability.Flying))
        {
            attacker.poisoned = true;
            Debug.Log("Attacker has been poisoned!");
        }

        if (attacker.HasAbility(Ability.Vampire))
        {
            if (!defender.HasAbility(Ability.Flying) && !defender.HasAbility(Ability.Reach))
            {
                target.DealDamage(attacker.attackValue, target);
                you.DealDamage(-attacker.attackValue, target);
            }
            else
            {
                defender.defenseValue -= attacker.attackValue;
                you.DealDamage(-attacker.attackValue, target);
            }
        }
        else if (attacker.HasAbility(Ability.Flying) && 
                 !defender.HasAbility(Ability.Flying) && 
                 !defender.HasAbility(Ability.Reach))
        {
            target.DealDamage(attacker.attackValue, target);
        }
        else if (defender.HasAbility(Ability.Finesse))
        {
            if (defender.attackValue > attacker.defenseValue)
            {
                Debug.Log("Finesse activated");
                Debug.Log("Defender Attack: " + defender.attackValue);
                Debug.Log("Attacker Defense: " + attacker.defenseValue);
                attacker.defenseValue = 0;
            }
            else
            {
                defender.defenseValue -= attacker.attackValue;
            }

            if (attacker.HasAbility(Ability.Acidic))
            {
                defender.defenseValue -= 1;
            }

            if (attacker.HasAbility(Ability.Corrosive))
            {
                defender.attackValue -= 1;
            }
        }
        else if (attacker.HasAbility(Ability.Pummel) && (attacker.attackValue > defender.defenseValue))
        {
            if (defender.HasAbility(Ability.Acidic))
            {
                attacker.defenseValue -= 1;
            }

            if (defender.HasAbility(Ability.Corrosive))
            {
                attacker.attackValue -= 1;
            }
            target.DealDamage(attacker.attackValue - defender.defenseValue, target);
            defender.defenseValue -= attacker.attackValue;
        }
        else if (defender.HasAbility(Ability.Acidic) && !attacker.HasAbility(Ability.Hardened))
        {
            attacker.defenseValue -= 1;
            defender.defenseValue -= attacker.attackValue;
        }
        else if (defender.HasAbility(Ability.Corrosive) && !attacker.HasAbility(Ability.Hardened))
        {
            attacker.attackValue -= 1;
            defender.defenseValue -= attacker.attackValue;
        }
        else
        {
            defender.defenseValue -= attacker.attackValue;
        }
    }

    public static void CardVsPlayer(Card attacker, Player target, Player you)
    {
        Debug.Log("Card Vs Player");
        Debug.Log("Dealing " + attacker.attackValue + " damage to " + target.name);

        target.DealDamage(attacker.attackValue, target);

        if (attacker.HasAbility(Ability.Vampire))
        {
            you.DealDamage(-attacker.attackValue, target);
        }
    }
}