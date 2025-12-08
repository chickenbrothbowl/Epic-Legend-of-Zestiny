using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "DeckAsset", menuName = "Cards/Deck")]
public class DeckAsset : ScriptableObject
{
    public List<string> cardIDs;

}