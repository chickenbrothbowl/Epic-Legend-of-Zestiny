using UnityEngine;
using System.Collections;

public class FillSlots : MonoBehaviour
{
    private EnemyDeck ed;
    public BattleSide side;

    void Start()
    {
        StartCoroutine(Deal());
    }

    IEnumerator Deal()
    {
        yield return new WaitForSeconds(1f);
        ed = GetComponentInParent<EnemyDeck>();
        foreach (var slot in side.slots)
        {
            yield return new WaitForSeconds(.3f);
            ed.DeckPlay();
        }
    }
}