using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, Pickup, Examine}
    public enum ItemType { Static, Consumable }

    public InteractionType interactionType;
    public ItemType itemType;
    [Header("Examine")]
    public string descriptionText;

    [Header("Custom Event")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.layer = 7;
    }

    public void Interact()
    {
        switch (interactionType)
        {
            case InteractionType.Pickup:
                FindObjectOfType<InventorySystem>().PickUpItem(gameObject);
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                break;
            default:
                break;
        }

        customEvent.Invoke();
    }
}
