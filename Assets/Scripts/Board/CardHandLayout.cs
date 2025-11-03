using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardHandLayout : MonoBehaviour
{
    public List<GameObject> cards;
    public float xSpacing = 1.5f;
    public float yIncrement = 0.2f;
    public float animationSpeed = 5f; // Speed of the animation
    public bool centerCards = true; // Center the hand around the parent position
    // New: fixed angle for the whole hand (in degrees, X axis for 2D)
	[SerializeField] float handAngleDeg = 90f;

    private int previousChildCount = 0;

    void Start()
    {
        UpdateCardsArray();
        ArrangeCards();
    }

    void Update()
    {
        // Only update the array if the number of children changed
        if (transform.childCount != previousChildCount)
        {
            UpdateCardsArray();
            ArrangeCards();
            previousChildCount = transform.childCount;
        }
    }

    void UpdateCardsArray()
    {
        cards = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            cards.Add(transform.GetChild(i).gameObject);
        }
    }

	public void ArrangeCards()
	{
    	if (cards.Count == 0) return;

    	Debug.Log($"[CardHandLayout] ArrangeCards called for {cards.Count} cards");
    	StopAllCoroutines();

    	float totalWidth = (cards.Count - 1) * xSpacing;
    	float startOffset = centerCards ? -totalWidth / 2f : 0f;

    	// New: compute the target rotation once
    	Quaternion targetRot = Quaternion.Euler(handAngleDeg, 0f, 0f);

    	for (int i = 0; i < cards.Count; i++)
    	{
        	Vector3 targetPos = transform.position;
        	targetPos.x += startOffset + (i * xSpacing);
        	targetPos.y += i * yIncrement;

        	StartCoroutine(AnimateTo(card: cards[i], targetPosition: targetPos, targetRotation: targetRot));
    	}
}

	IEnumerator AnimateTo(GameObject card, Vector3 targetPosition, Quaternion targetRotation)
	{
    	if (card == null) yield break;

    	int frameCount = 0;

    	while (card.transform.parent == this.transform &&
           (Vector3.Distance(card.transform.position, targetPosition) > 0.01f ||
            Quaternion.Angle(card.transform.rotation, targetRotation) > 0.5f))
    	{
        	float t = Mathf.Min(Time.deltaTime * animationSpeed, 0.5f);

        	card.transform.position = Vector3.Lerp(card.transform.position, targetPosition, t);
        	card.transform.rotation = Quaternion.Slerp(card.transform.rotation, targetRotation, t);

        	frameCount++;
        	yield return null;
    	}

    if (card.transform.parent == this.transform)
    	{
        	card.transform.position = targetPosition;
        	card.transform.rotation = targetRotation;
    	}

    Debug.Log($"[CardHandLayout] Animation completed for {card.name} in {frameCount} frames");
	}
}