using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerDeck", menuName = "Cards/Player Deck")]
public class PlayerDeckAsset : ScriptableObject
{
    public List<string> cardIDs;
}