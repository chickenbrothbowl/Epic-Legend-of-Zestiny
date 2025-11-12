using UnityEngine;

public class GameStateMonitor : MonoBehaviour
{
    public int maxJuiceAmnt = 1;
    public Player player;
    public Player enemy;
	public JuiceLevel juice;
    public EnemyDeck enemyDeck;
	public DeckLayout playerDeck;
    public CardHandLayout playerHand;
    public bool isPlayerTurn = true;
    public GameBoard board;
	public int handSize = 3;

    void OnMouseDown()
    {
        if (isPlayerTurn)
        {
            AudioManager.Instance.BellTapsound();
			board.DoAttacks();
            EndTurn();
            DoEnemyTurn();
        }
    }

    void DoEnemyTurn()
    {
        enemyDeck.DeckPlay();
        EndTurn();
        maxJuiceAmnt++;
		if (maxJuiceAmnt > 10){
			maxJuiceAmnt = 10;
		}
		juice.SetJuice(maxJuiceAmnt);

        AudioManager.Instance.JuiceRefilSound();
		playerDeck.DrawCard();
    }

    void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        juice.SetJuice(maxJuiceAmnt);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerTurn && playerHand.cards.Count < handSize)
        {
            playerDeck.canDraw = true;
        }
        else
        {
            playerDeck.canDraw = false;
        }
        
    }
}
