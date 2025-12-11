using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyDeck", menuName = "Cards/Enemy Deck")]
public class EnemyDeckAsset : ScriptableObject
{
    public List<string> cardIDs;
}