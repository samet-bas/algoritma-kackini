using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public GameObject clonedBlock;
    private CameraController cc;

    private void Awake()
    {
        cc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cc.canRotate = false;
        if (gameObject.name[0] == 'C') return;
        parentAfterDrag = transform.parent;
        clonedBlock = Instantiate(gameObject, parentAfterDrag);
        clonedBlock.name = "Cloned_" + gameObject.name;
        image.raycastTarget = false;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
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
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                image.raycastTarget = true;
                clonedBlock = null;
                return;
            }
            clonedBlock.transform.SetParent(parentAfterDrag);
            clonedBlock.GetComponent<DraggableBlock>().image.raycastTarget = true;
            image.raycastTarget = true;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            clonedBlock = null;
        }
        cc.canRotate = true;
    }
}