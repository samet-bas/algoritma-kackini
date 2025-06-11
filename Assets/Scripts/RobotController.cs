using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class RobotController : MonoBehaviour
{
    private float moveDistance = 2.5f;
    private float moveSpeed = 1.5f;
    private float turnDegree = 90f;
    public GameObject Content;
    private CodeBlocks[] codeBlocks;

    public List<Vector3> targetPositions;
    private List<Vector3> completedTargetPositions = new List<Vector3>();


    private Animator animator;

    public GameManager gameManager;

    [SerializeField] private float completionTolerance = 0.1f; // Pozisyon eşleşmesi hassasiyeti

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ExecuteCodeBlocks()
    {
        gameManager.HidePanelsOnRun();
        codeBlocks = Content.GetComponentsInChildren<CodeBlocks>();
        StartCoroutine(ExecuteSequence());
    }

    public void StopRunning()
    {
        gameManager.ShowPanelsOnStop();
        transform.position = new Vector3(-1f, 0.5f, -6f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private IEnumerator ExecuteSequence()
    {
        foreach (CodeBlocks cb in codeBlocks)
        {
            switch (cb.type)
            {
                case CodeType.Go:
                    animator.SetBool("walking", true);
                    yield return transform.DOMove(transform.position + transform.forward * moveDistance, moveSpeed).WaitForCompletion();
                    animator.SetBool("walking", false);
                    break;

                case CodeType.Turn_right:
                    yield return transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, turnDegree, 0), moveSpeed).WaitForCompletion();
                    break;

                case CodeType.Turn_left:
                    yield return transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -turnDegree, 0), moveSpeed).WaitForCompletion();
                    break;

                case CodeType.Place:
                    for (int i = 0; i < targetPositions.Count; i++)
                    {
                        Vector3 target = targetPositions[i];

                       
                        if (completedTargetPositions.Contains(target))
                            continue;

                        if (Vector3.Distance(transform.position, target) <= completionTolerance)
                        {
                            completedTargetPositions.Add(target);
                            
                        }
                    }

                    
                    if (completedTargetPositions.Count >= targetPositions.Count)
                    {
                        
                        StopRunning();
                        gameManager.EndLevel();
                        yield break;
                    }
                    break;

            }
        }

        StopRunning();
    }
}
