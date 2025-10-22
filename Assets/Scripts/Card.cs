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
    public Renderer cardRenderer;
    public TextMeshPro nameText;
    public TextMeshPro attackText;
    public TextMeshPro defenseText;
    
    [Header("Interaction")]
    public LayerMask slotLayerMask;
    private CardSlot currentSlot;
    private Vector3 dragOffset;
    private bool isDragging = false;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        UpdateCardVisuals();
    }
    
    void UpdateCardVisuals()
    {
        // Update card face texture
        if (cardRenderer != null && cardImage != null)
        {
            cardRenderer.material.mainTexture = cardImage.texture;
        }
        
        // Update text displays
        if (nameText != null) nameText.text = cardName;
        if (attackText != null) attackText.text = attackValue.ToString();
        if (defenseText != null) defenseText.text = defenseValue.ToString();
    }
    
    void OnMouseDown()
    {
        isDragging = true;
        
        // Calculate offset from mouse to card position
        Vector3 mousePos = GetMouseWorldPosition();
        dragOffset = transform.position - mousePos;
        
        // Remove from current slot if in one
        if (currentSlot != null)
        {
            currentSlot.RemoveCard();
            currentSlot = null;
        }
    }
    
    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            transform.position = mousePos + dragOffset;
        }
    }
    
    void OnMouseUp()
    {
        isDragging = false;
        
        // Try to find a slot under the card
        CardSlot targetSlot = FindSlotUnderCard();
        
        if (targetSlot != null && targetSlot.IsEmpty)
        {
            targetSlot.PlaceCard(this);
            currentSlot = targetSlot;
        }
        else if (currentSlot != null)
        {
            // Return to previous slot
            currentSlot.PlaceCard(this);
        }
        else
        {
            // No valid slot, could return to hand or starting position
            ReturnToHand();
        }
    }
    
    CardSlot FindSlotUnderCard()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 10f, Vector3.down);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 20f, slotLayerMask))
        {
            return hit.collider.GetComponent<CardSlot>();
        }
        
        return null;
    }
    
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;
        
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        
        return transform.position;
    }
    
    void ReturnToHand()
    {
        // Implement hand return logic
        // For now, just keep it where it is or destroy it
        Debug.Log("Card returned to hand");
    }
    
    public void SetSlot(CardSlot slot)
    {
        currentSlot = slot;
    }
}
