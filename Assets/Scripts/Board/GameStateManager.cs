using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int maxJuiceAmnt = 1;
    public int handSize = 3;
    
    [Header("References")]
    public Player player;
    public Player enemy;
    public JuiceLevel juice;
    public EnemyDeck enemyDeck;
    public DeckLayout playerDeck;
    public CardHandLayout playerHand;
    public GameBoard board;
    
    [HideInInspector]
    public bool isPlayerTurn = true;

    void Start()
    {
        juice.SetJuice(maxJuiceAmnt);
    }

    void Update()
    {
        handSize = 3 + HarvestCount();
        board.playerSide.ApplyTribalBuffs();
        board.enemySide.ApplyTribalBuffs();
		if (playerHand.cards.Count >= handSize){
			playerDeck.canDraw = false;
		}	
    }

    public void EndPlayerTurn()
    {
        board.DoAttacks();
        isPlayerTurn = false;
        DoEnemyTurn();
    }

    void DoEnemyTurn()
    {
        enemyDeck.DeckPlay();
        isPlayerTurn = true;
        
        // Refill juice
        maxJuiceAmnt++;
        if (maxJuiceAmnt > 10)
        {
            maxJuiceAmnt = 10;
        }
        juice.SetJuice(maxJuiceAmnt);
        AudioManager.Instance.JuiceRefilSound();
        
        playerDeck.canDraw = true;
    }

    public int HarvestCount()
    {
        int count = 0;
        foreach (CardSlot slot in board.playerSide.slots)
        {
            if (!slot.IsEmpty && slot.currentCard.harvest)
            {
                count++;
            }
        }
        return count;
    }
}