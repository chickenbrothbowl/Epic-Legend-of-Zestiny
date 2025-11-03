using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    [Header("Card Data")]
    public string cardName;
    public Sprite cardImage;
    public int attackValue;
    public int defenseValue;
    
    [Header("Visual Components")]
    public Renderer cardImageRenderer;
    public TextMeshPro nameText;
    public TextMeshPro attackText;
    public TextMeshPro defenseText;
    
    [Header("Interaction")]
    public LayerMask slotLayerMask;
    public CardSlot currentSlot;
    private Vector3 dragOffset;
    public float boardHeight = -1f; // Changed to float and made it the actual Y position
    public float dragLiftSpeed = 15f; // Speed for lifting animation
    private bool isDragging = false;
	public bool isReturning = false;
    public bool isDraggable = true;
    private Camera mainCamera;
    private Vector3 targetPosition;
	private Vector3 initialPosition;
    private string oldName;
    private int oldAtk;
    private int oldDef;
	private Sprite oldImage;
	
	public CardSlot slotUnderCard;
	public CardSlot oldSlotUnderCard;
    
    void Start()
    {
        mainCamera = Camera.main;
        UpdateCardVisuals();
    }
    
    void UpdateCardVisuals()
    {
        // Update card face texture
        if (cardImageRenderer != null && cardImage != null)
        {
            cardImageRenderer.material.mainTexture = cardImage.texture;
        }
        
        // Update text displays
        if (nameText != null) nameText.text = cardName;
        if (attackText != null) attackText.text = attackValue.ToString();
        if (defenseText != null) defenseText.text = defenseValue.ToString();
		// If defense is 0, destroy the card
		if (defenseValue <= 0){
			Destroy(gameObject);
		}
        oldName = cardName;
        oldAtk = attackValue;
        oldDef = defenseValue;
		oldImage = cardImage;
    }
    
	void OnMouseDown()
	{
        if (isDraggable)
        {
            isDragging = true;
            targetPosition = transform.position;
            initialPosition = transform.position;

            // Calculate offset using the card's CURRENT height (not boardHeight yet)
            Vector3 mousePos = GetMouseWorldPositionAtHeight(transform.position.y);
            dragOffset = new Vector3(
                transform.position.x - mousePos.x,
                0,
                transform.position.z - mousePos.z
            );
    
            // Remove from current slot if in one
            if (currentSlot != null)
            {
                currentSlot.RemoveCard();
                currentSlot = null;
            }
        }
}
    
    void OnMouseDrag()
    {
        if (isDraggable)
        {
            // Get mouse position at the fixed board height
            Vector3 mousePos = GetMouseWorldPositionAtHeight(boardHeight);
            targetPosition = new Vector3(
                mousePos.x + dragOffset.x,
                boardHeight,
                mousePos.z + dragOffset.z
            );
			slotUnderCard = FindSlotUnderCard();
			if (oldSlotUnderCard != slotUnderCard){
    			// Turn off the old slot's glow if it exists
    			if (oldSlotUnderCard != null){
        		oldSlotUnderCard.SetBorderGlow(oldSlotUnderCard.normalColor, 0);
    			}
    			// Turn on the new slot's glow if it exists
    			if (slotUnderCard != null){
        			slotUnderCard.SetBorderGlow(slotUnderCard.hoverColor, slotUnderCard.glowIntensity);
    			} 
    			oldSlotUnderCard = slotUnderCard;
			}
			
        }
    }
    
    void Update()
    {
        // Smoothly animate to target position during drag
        if (isDragging || isReturning)
        {
            transform.position = Vector3.Lerp(
                transform.position, 
                targetPosition, 
                dragLiftSpeed * Time.deltaTime
            );
        }
		
        if (cardName != oldName || defenseValue != oldDef || attackValue != oldAtk || cardImage != oldImage)
        {
            UpdateCardVisuals();
        }
    }
    
    void OnMouseUp()
    {
        if (isDraggable)
        {
            isDragging = false;
        
            // Try to find a slot under the card
            CardSlot targetSlot = FindSlotUnderCard();
            Transform parent = transform.parent;
            CardHandLayout hand = parent.GetComponent<CardHandLayout>();
        
            if (targetSlot != null && targetSlot.IsEmpty)
            {
                
                if (hand)
                {
                    hand.cards.Remove(transform.gameObject);
                }
                targetSlot.PlaceCard(this);
                currentSlot = targetSlot;
                // isDraggable = false;
            }
            else if (currentSlot != null)
            {
                // Return to previous slot
                currentSlot.PlaceCard(this);
            }
            else
            {
                if (hand)
                {
                    hand.ArrangeCards();
                }
            }
        }
    }
    
    CardSlot FindSlotUnderCard()
    {
        Collider cardCollider = GetComponent<Collider>();
        if (cardCollider != null) cardCollider.enabled = false;
    
        Ray ray = new Ray(transform.position + Vector3.up * 10f, Vector3.down);
        RaycastHit hit;
        CardSlot result = null;
    
        if (Physics.Raycast(ray, out hit, 20f, slotLayerMask))
        {
            result = hit.collider.GetComponent<CardSlot>();
        }
    
        if (cardCollider != null) cardCollider.enabled = true;
        return result;
    }
    
    // New method: Get mouse position at a FIXED height
    Vector3 GetMouseWorldPositionAtHeight(float height)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // Use a fixed plane at the specified height
        Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));
        float distance;
        
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        
        return transform.position;
    }
    
    public void SetSlot(CardSlot slot)
    {
        currentSlot = slot;
    }

	public void AttackCard(Card card){
		// TODO: Do animations
		card.defenseValue -= attackValue;
		return;
	}
	public void AttackPlayer(){
	}
    
    public void LoadFromData(CardData data)
    {
    if (data == null)
    {
        Debug.LogWarning("Card data is null");
        return;
    }

    cardName = data.CardName;
    attackValue = data.Damage;
    defenseValue = data.Health;
    // cardImage = ???
    // How to deal with card properties like Acidic?

    UpdateCardVisuals();
    }
}