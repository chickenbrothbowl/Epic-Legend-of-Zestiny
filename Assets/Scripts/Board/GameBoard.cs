using UnityEngine;
using System.Collections;

// GameBoard now just coordinates
public class GameBoard : MonoBehaviour
{
    public BattleSide playerSide;
    public BattleSide enemySide;
    public GameStateManager manager;

    [ContextMenu("Process Attacks")]
    public IEnumerator DoAttacks()
    {
        if (manager.isPlayerTurn)
        {
            yield return StartCoroutine(playerSide.AttackOpposingSide(enemySide));
            yield return StartCoroutine(enemySide.AttackOpposingSide(playerSide));
        }
        else
        {
            yield return StartCoroutine(enemySide.AttackOpposingSide(playerSide));
            yield return StartCoroutine(playerSide.AttackOpposingSide(enemySide));
        }
    }
}