using UnityEngine;
using System.Collections;


public class BattleSide : MonoBehaviour
{
    public CardSlot[] slots;
    public Player player;
    public bool isPlayerSide = false;

    void Start()
    {
        slots = GetComponentsInChildren<CardSlot>();
        AudioManager.Instance.PlayBackgroundMusic();

        if (player != null)
        {
            player.battleSide = this;
        }
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

    public IEnumerator AttackOpposingSide(BattleSide opponent)
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
                    yield return StartCoroutine(CombatResolver.CardVsCard(attacker, defender, opponent.player, this.player));
                }
                else
                {
                    if (isPlayerSide == true)
                    {
                        Debug.Log("Attacking Enemy");
                        yield return StartCoroutine(CombatResolver.CardVsPlayer(attacker, opponent.player, this.player));
                    }
                    else
                    {
                        Debug.Log("Attacking Player");
                        yield return StartCoroutine(CombatResolver.CardVsPlayer(attacker, opponent.player, this.player));
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

    public static IEnumerator CardVsCard(Card attacker, Card defender, Player target, Player you)
    {
        Debug.Log("Card Vs Card");
        attacker.Shake(1f, .01f);
        yield return new WaitForSeconds(1f); // Wait for .25 seconds


        if (defender.HasAbility(Ability.Rotten) && 
            !attacker.HasAbility(Ability.Hardened) && 
            !attacker.HasAbility(Ability.Flying))
        {
            attacker.poisoned = true;
            Debug.Log("Attacker has been poisoned!");
        }

        if (attacker.HasAbility(Ability.Vampire))
        {
            if (!defender.HasAbility(Ability.Flying) && !defender.HasAbility(Ability.Finesse))
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
                 !defender.HasAbility(Ability.Finesse))
        {
            target.DealDamage(attacker.attackValue, target);
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

    public static IEnumerator CardVsPlayer(Card attacker, Player target, Player you)
    {
        Debug.Log("Card Vs Player");
        Debug.Log("Dealing " + attacker.attackValue + " damage to " + target.name);
        attacker.Shake(1f, .01f);
        
        target.DealDamage(attacker.attackValue, target);
        yield return new WaitForSeconds(1f); // Wait for .25 seconds

        if (attacker.HasAbility(Ability.Vampire))
        {
            you.DealDamage(-attacker.attackValue, target);
        }
        
        
    }
}