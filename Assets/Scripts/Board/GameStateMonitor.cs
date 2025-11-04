using UnityEngine;

public class GameStateMonitor : MonoBehaviour
{
    public int maxJuiceAmnt = 1;
    public Player player;
    public Player enemy;
    public EnemyDeck enemyDeck;
    public bool isPlayerTurn = true;
    public GameBoard board;

    void OnMouseDown()
    {
        if (isPlayerTurn)
        {
            EndTurn();
            DoEnemyTurn();
        }
    }

    void DoEnemyTurn()
    {
        enemyDeck.DeckPlay();
        EndTurn();
        maxJuiceAmnt++;
    }

    void EndTurn()
    {
        board.DoAttacks();
        isPlayerTurn = !isPlayerTurn;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
