using UnityEngine;

// GameBoard now just coordinates
public class GameBoard : MonoBehaviour
{
    public BattleSide playerSide;
    public BattleSide enemySide;
    public GameStateManager manager;

    [ContextMenu("Process Attacks")]
    public void DoAttacks()
    {
        if (manager.isPlayerTurn)
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