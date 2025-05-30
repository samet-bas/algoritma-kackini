using UnityEngine;
using UnityEngine.EventSystems;
public class ProgramPanel : MonoBehaviour, IDropHandler
{
    public GameObject content;
    
    public void OnDrop(PointerEventData eventData)
    {
        
        GameObject dropped = eventData.pointerDrag;
        DraggableBlock block = dropped.GetComponent<DraggableBlock>();
        block.parentAfterDrag = content.transform;
        
    }
}
