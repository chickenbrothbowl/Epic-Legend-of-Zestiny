using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    [Header("Card Data")]
    public CardData Data { get; private set; }
    public Sprite cardImage;

    [Header("Runtime Stats")]
    private int baseAttack;
    private int baseDefense;
    public int attackValue;
    public int defenseValue;

    [Header("Runtime Status Effects")]
    public bool poisoned; // Runtime status, not a permanent ability

    [Header("Visual Components")]
    public SpriteRenderer spriteRenderer;
    public TextMeshPro nameText;
    public TextMeshPro attackText;
    public TextMeshPro defenseText;

    [Header("Interaction")]
    public LayerMask slotLayerMask;
    public CardSlot currentSlot;
    private Vector3 dragOffset;
    public float boardHeight = -1f;
    public float dragLiftSpeed = 15f;
    private bool isDragging = false;
    public bool isReturning = false;
    public bool isDraggable = true;
    private Camera mainCamera;
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    public CardSlot slotUnderCard;
    public CardSlot oldSlotUnderCard;

    // Cache for visual change detection
    private string oldName;
    private int oldAtk;
    private int oldDef;
    private Sprite oldImage;

    public static event System.Action<Card, BattleSide> OnCardDied;

    // Convenience accessors
    public string CardName => Data?.CardName ?? string.Empty;
    public string CardID => Data?.CardID ?? string.Empty;
    public int Cost => Data?.Cost ?? 0;
    public Ability Abilities => Data?.Abilities ?? Ability.None;
    public bool HasAbility(Ability ability) => Data?.HasAbility(ability) ?? false;
    public IEnumerable<Ability> GetActiveAbilities() => Data.GetActiveAbilities();

    void Start()
    {
        mainCamera = Camera.main;
        UpdateCardVisuals();
    }

    void UpdateCardVisuals()
    {
        if (spriteRenderer != null && cardImage != null)
        {
            spriteRenderer.sprite = cardImage;
        }

        if (nameText != null) nameText.text = CardName;
        if (attackText != null) attackText.text = attackValue.ToString();
        if (defenseText != null) defenseText.text = defenseValue.ToString();

        if (defenseValue <= 0)
        {
            BattleSide side = GetComponentInParent<BattleSide>();
            OnCardDied?.Invoke(this, side);
            Destroy(gameObject);
        }

        oldName = CardName;
        oldAtk = attackValue;
        oldDef = defenseValue;
        oldImage = cardImage;
    }

    public void RefreshVisuals() => UpdateCardVisuals();

    public void SetBaseAttack(int atk)
    {
        baseAttack = atk;
        attackValue = baseAttack;
    }

    public void SetBaseDefense(int def)
    {
        baseDefense = def;
        defenseValue = baseDefense;
    }

    public void ApplyAttackBonus(int bonus)
    {
        attackValue = baseAttack + bonus;
        UpdateCardVisuals();
    }

    public void ApplyDefenseBonus(int bonus)
    {
        defenseValue = baseDefense + bonus;
        UpdateCardVisuals();
    }

    public int GetBaseAttack() => baseAttack;
    public int GetBaseDefense() => baseDefense;

    void OnMouseDown()
    {
        if (isDraggable)
        {
            isDragging = true;
            targetPosition = transform.position;
            initialPosition = transform.position;

            Vector3 mousePos = GetMouseWorldPositionAtHeight(transform.position.y);
            dragOffset = new Vector3(
                transform.position.x - mousePos.x,
                0,
                transform.position.z - mousePos.z
            );

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
            Vector3 mousePos = GetMouseWorldPositionAtHeight(boardHeight);
            targetPosition = new Vector3(
                mousePos.x + dragOffset.x,
                boardHeight,
                mousePos.z + dragOffset.z
            );
            slotUnderCard = FindSlotUnderCard();
            if (oldSlotUnderCard != slotUnderCard)
            {
                if (oldSlotUnderCard != null)
                {
                    oldSlotUnderCard.SetBorderGlow(oldSlotUnderCard.normalColor, 0);
                }
                if (slotUnderCard != null)
                {
                    slotUnderCard.SetBorderGlow(slotUnderCard.hoverColor, slotUnderCard.glowIntensity);
                }
                oldSlotUnderCard = slotUnderCard;
            }
        }
    }

    void Update()
    {
        if (isDragging || isReturning)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                dragLiftSpeed * Time.deltaTime
            );
        }

        if (CardName != oldName || defenseValue != oldDef || attackValue != oldAtk || cardImage != oldImage)
        {
            UpdateCardVisuals();
        }
    }

    void OnMouseUp()
    {
        if (isDraggable)
        {
            isDragging = false;

            CardSlot targetSlot = FindSlotUnderCard();
            Transform parent = transform.parent;
            CardHandLayout hand = parent.GetComponent<CardHandLayout>();

            if (targetSlot != null && targetSlot.CanPlay(this))
            {
                if (hand)
                {
                    hand.cards.Remove(transform.gameObject);
                }
                targetSlot.PlaceCard(this);
                currentSlot = targetSlot;
            }
            else if (currentSlot != null)
            {
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
        CardSlot result = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 20f, slotLayerMask))
        {
            result = hit.collider.GetComponent<CardSlot>();
        }

        if (cardCollider != null) cardCollider.enabled = true;
        return result;
    }

    Vector3 GetMouseWorldPositionAtHeight(float height)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return transform.position;
    }

    public void SetSlot(CardSlot slot) => currentSlot = slot;

    public void LoadFromData(CardData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Card data is null");
            return;
        }

        Data = data;
        cardImage = Resources.Load<Sprite>($"CardIcons/{data.CardID}");

        SetBaseAttack(data.Damage);
        SetBaseDefense(data.Health);
        UpdateCardVisuals();
    }
}