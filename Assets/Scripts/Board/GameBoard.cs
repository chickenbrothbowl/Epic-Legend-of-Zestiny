using UnityEngine;

// GameBoard now just coordinates
public class GameBoard : MonoBehaviour
{
    public BattleSide playerSide;
    public BattleSide enemySide;
    public GameStateMonitor monitor;

    [ContextMenu("Process Attacks")]
    public void DoAttacks()
    {
        if (monitor.isPlayerTurn)
        {
            playerSide.AttackOpposingSide(enemySide);
            enemySide.AttackOpposingSide(playerSide);
        }
        else
        {
            enemySide.AttackOpposingSide(playerSide);
            playerSide.AttackOpposingSide(enemySide);
        }
    }
}