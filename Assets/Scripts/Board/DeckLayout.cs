using UnityEngine;
using System.Collections;

public class DeckLayout : Deck
{
    public bool canDraw = true;
    public GameObject hand;
    private CardHandLayout handLayout;
    private bool isShaking;
	public GameStateManager gsm;

    [Header("Shake Settings")]
    public float shakeSpeed = 8f;
    public float shakeAmount = 10f;
    private Quaternion originalRotation;

    protected override void Update()
    {
        base.Update();

        if (cards.Count == 0) return;

        GameObject topCard = cards[cards.Count - 1];

        if (canDraw)
        {
            if (!isShaking)
            {
                originalRotation = topCard.transform.localRotation;
                StartShaking(topCard);
                isShaking = true;
            }
        }
        else
        {
			if (isShaking) {
				StopShaking(topCard);
			}
        }
    }

    void OnMouseDown()
    {
        if (canDraw && gsm.isPlayerTurn)
        {
            DrawCard();
            isShaking = false;
        }
    }

    [ContextMenu("Draw Card")]
    public void DrawCard()
    {
        if (cards.Count == 0) return;
        GameObject card = cards[cards.Count - 1];
        cards.Remove(card);
        previousChildCount--;
        card.transform.SetParent(hand.transform);
        StopAllCoroutines();

        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent != null)
        {
            cardComponent.isDraggable = true;
            cardComponent.isReturning = false;
        }
    }

    void StartShaking(GameObject card)
    {
        isShaking = true;
        StartCoroutine(ShakeRoutine(card));
    }

    void StopShaking(GameObject card)
    {
        isShaking = false;
        StopAllCoroutines();
        card.transform.localRotation = originalRotation;
    }

    IEnumerator ShakeRoutine(GameObject card)
    {
        while (isShaking)
        {
            float shake = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            card.transform.localRotation = originalRotation * Quaternion.Euler(shake, shake, shake);
            yield return null;
        }
    }
}