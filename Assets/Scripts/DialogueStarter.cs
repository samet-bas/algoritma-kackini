using System.Collections;
using UnityEngine;
using DialogueEditor;
using DG.Tweening;

public class DialogueStarter : MonoBehaviour
{
    public NPCConversation conversation;
    public RectTransform conversationPanel;
    public RectTransform conversationIcon;
    
    private void Start()
    {
        StartCoroutine(StartDialogue());
        
    }

    private IEnumerator StartDialogue()
    {
        
        yield return new WaitForSeconds(1.5f);
        conversationIcon.DOAnchorPosY(-250f, 1f).From();
        conversationPanel.DOAnchorPosY(-250f, 1f).From();
        ConversationManager.Instance.StartConversation(conversation);
    }
}