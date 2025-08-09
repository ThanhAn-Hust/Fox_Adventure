using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Fields")]
    public Transform detectionPoint;
    private const float detectionRadius = 0.2f;
    public LayerMask detectionLayer;
    public GameObject detectedObject;

    [Header("Examine Fields")]
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public bool isExamining;

    void Update()
    {
        if(DetectObject())
        {
            if(InteractInput())
            {
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
        if(obj == null)
        {
            detectedObject = null;
            return false ;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    public void PickupItem(GameObject item)
    {
        FindObjectOfType<InventorySystem>().PickUpItem(item);
    }

    public void ExamineItem(Item item)
    {
        if(isExamining)
        {
            examineWindow.SetActive(false);
            isExamining = false;
        }
        else
        {
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            examineText.text = item.descriptionText;
            examineWindow.SetActive(true);
            isExamining = true;


        }
    }
    

}
