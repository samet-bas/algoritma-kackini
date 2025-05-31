using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public GameObject clonedBlock;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameObject.name[0] == 'C') return;
        parentAfterDrag = transform.parent;
        clonedBlock = Instantiate(gameObject, parentAfterDrag);
        clonedBlock.name = "Cloned_" + gameObject.name;
        image.raycastTarget = false;
        image.color = new Color(1, 1, 1, 0.5f);
        clonedBlock.transform.SetParent(transform.root);
        clonedBlock.transform.SetAsLastSibling();
        clonedBlock.GetComponent<DraggableBlock>().image.raycastTarget = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (clonedBlock != null)
        {
            clonedBlock.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (clonedBlock != null)
        {
            if (parentAfterDrag.CompareTag("BlokPanel"))
            {
                Destroy(clonedBlock);
                image.color = new Color(1, 1, 1, 1f);
                image.raycastTarget = true;
                clonedBlock = null;
                return;
            }
            clonedBlock.transform.SetParent(parentAfterDrag);
            clonedBlock.GetComponent<DraggableBlock>().image.raycastTarget = true;
            image.raycastTarget = true;
            image.color = new Color(1, 1, 1, 1f);
            clonedBlock = null;
        }
    }
}