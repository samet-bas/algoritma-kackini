using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ProgramPanel : MonoBehaviour, IDropHandler
{
    public GameObject content;
    
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        GameObject dropped = eventData.pointerDrag;
        DraggableBlock block = dropped.GetComponent<DraggableBlock>();
        if (block == null) return;

        block.parentAfterDrag = content.transform;

        if (block.CompareTag("loop"))
        {
            Transform scrollView = block.clonedBlock?.transform.Find("Scroll View");
            if (scrollView != null)
            {
                scrollView.gameObject.SetActive(true);
            }
        }

        if (transform.parent.name.Equals("Cloned_Loop"))
        {
            block.clonedBlock.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }

}
