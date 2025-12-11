using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameStateManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int maxJuiceAmnt = 1;
    public int handSize = 3;
    
    [Header("References")]
    public Player player;
    public Player enemy;
    public JuiceLevel juice;
    public EnemyDeck enemyDeck;
    public DeckLayout playerDeck;
    public CardHandLayout playerHand;
    public GameBoard board;
    public Button winButton;
    public Button loseButton;
    
    [HideInInspector]
    public bool isPlayerTurn = true;

    void Start()
    {
        juice.SetJuice(maxJuiceAmnt);
    }

    void Update()
    {
        handSize = 3 + HarvestCount();
        board.playerSide.ApplyTribalBuffs();
        board.enemySide.ApplyTribalBuffs();
		if (playerHand.cards.Count >= handSize){
			playerDeck.canDraw = false;
		}	
    }

    public void PlayerWin()
    {
        winButton.gameObject.SetActive(true);
        Time.timeScale = 0f;
        StopAllCoroutines();
    }

    public void PlayerLose()
    {
        loseButton.gameObject.SetActive(true);
        Time.timeScale = 0f;
        StopAllCoroutines();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMap()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene("SampleScene");
    }

    public IEnumerator EndPlayerTurn()
    {
        yield return StartCoroutine(board.DoAttacks());
        isPlayerTurn = false;
        DoEnemyTurn();
    }

    void DoEnemyTurn()
    {
        enemyDeck.DeckPlay();
        isPlayerTurn = true;
        
        // Refill juice
        maxJuiceAmnt++;
        if (maxJuiceAmnt > 10)
        {
            maxJuiceAmnt = 10;
        }
        juice.SetJuice(maxJuiceAmnt);
        AudioManager.Instance.JuiceRefilSound();
        
        playerDeck.canDraw = true;
    }

    public int HarvestCount()
    {
        int count = 0;
        foreach (CardSlot slot in board.playerSide.slots)
        {
            if (!slot.IsEmpty && slot.currentCard.HasAbility(Ability.Harvest))
            {
                count++;
            }
        }
        return count;
    }

    public void OpenMap()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleSceneUI", LoadSceneMode.Additive);
    }
}