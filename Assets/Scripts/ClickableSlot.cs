using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;


public class ClickableSlot : MonoBehaviour
{
    private CardSlot cardSlot;
	public Transform playerDeck;
	public Transform enemyDeck;

    bool triggered = false;
    public static UnityEvent clickedEvent;

    void Start()
    {
        cardSlot = GetComponentInParent<CardSlot>();
        if (clickedEvent == null)
            clickedEvent = new UnityEvent();
        clickedEvent.AddListener(OnEventTriggered);

    }

    void OnMouseDown()
    {
        if (!triggered)
        {
			StartCoroutine(DelayedSwitchScene());
        }

    }

	private IEnumerator DelayedSwitchScene() {
            triggered = true;
            PersistentStateManager.Instance.playerDeckAsset.cardIDs.Add(cardSlot.currentCard.Data.CardID);
			Card c = cardSlot.currentCard;
			cardSlot.currentCard = null;
			c.transform.parent = playerDeck;
            clickedEvent.Invoke();
			yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("SampleSceneUI", LoadSceneMode.Additive);
	}
    
    void OnEventTriggered()
    {
		if(!triggered){
			Debug.Log("Event triggered.");
        	PersistentStateManager.Instance.princeDeckAsset.cardIDs.Add(cardSlot.currentCard.Data.CardID);
			Card c = cardSlot.currentCard;
			cardSlot.currentCard = null;
			c.transform.parent = enemyDeck;
		}

    }
}