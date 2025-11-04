using UnityEngine;

public class GameStateMonitor : MonoBehaviour
{
    public int juiceAmnt = 1;
    public int playerLife = 5;
    public int enemyLife = 5;
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
        juiceAmnt++;
        EndTurn();
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
