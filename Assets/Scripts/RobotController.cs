using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RobotController : MonoBehaviour
{
    private float moveDistance = 2.5f;
    private float moveSpeed = 1f;
    public GameObject Content;
    private CodeBlocks[] codeBlocks;

    public void ExecuteCodeBlocks()
    {
        codeBlocks = Content.GetComponentsInChildren<CodeBlocks>();
        StartCoroutine(ExecuteSequence());
    }

    private IEnumerator ExecuteSequence()
    {
        foreach (CodeBlocks cb in codeBlocks)
        {
            switch (cb.type)
            {
                case CodeType.Go:
                    yield return transform.DOMove(transform.position + new Vector3(moveDistance, 0, 0), moveSpeed).WaitForCompletion();
                    break;
            }
        }
    }
}