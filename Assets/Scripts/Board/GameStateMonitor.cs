using UnityEngine;

public class GameStateMonitor : MonoBehaviour
{
    public int maxJuiceAmnt = 1;
    public Player player;
    public Player enemy;
	public JuiceLevel juice;
    public EnemyDeck enemyDeck;
    public bool isPlayerTurn = true;
    public GameBoard board;

    void OnMouseDown()
    {
        if (isPlayerTurn)
        {
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
        
    }
}
