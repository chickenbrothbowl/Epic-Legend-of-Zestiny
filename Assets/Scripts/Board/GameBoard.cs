using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public CardSlot[] playerSlots;
    public CardSlot[] enemySlots;

    public bool isPlayerTurn;
    
    void Start()
    {
        // Initialize slots if not set in inspector
        CardSlot[] cardSlots = GetComponentsInChildren<CardSlot>();
        int numSlots = cardSlots.Length;
        playerSlots = cardSlots[0..(numSlots / 2)];
        enemySlots = cardSlots[(numSlots/2)..];
    }

    void Update()
    {
        
    }

    void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;
    }


	[ContextMenu("Proccess Attacks")]
    void DoAttacks()
    {
        if (isPlayerTurn)
        {
            DoPlayerAttacks();
            DoEnemyAttacks();
        }
        else
        {
            DoEnemyAttacks();
            DoPlayerAttacks();
        }
    }

    void DoPlayerAttacks()
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
			if (!playerSlots[i].IsEmpty){
				if (!enemySlots[i].IsEmpty){
					playerSlots[i].currentCard.AttackCard(enemySlots[i].currentCard);
				} else {
					playerSlots[i].currentCard.AttackPlayer();
				}
			}
        }
    }

    void DoEnemyAttacks()
    {
        for (int i = 0; i < enemySlots.Length; i++)
        {
                
        }
    }
    
}
