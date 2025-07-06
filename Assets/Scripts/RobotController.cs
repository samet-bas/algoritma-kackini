using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class RobotController : MonoBehaviour
{
    public float moveDistance = 2.5f;
    public float moveSpeed = 1.5f;
    private float turnDegree = 90f;
    public Vector3 startPos;
    public GameObject Content;
    private CodeBlocks[] codeBlocks;

    public List<GameObject> targetObjects;
    private List<GameObject> completedTargetObjects = new List<GameObject>();

    private Animator animator;
    public GameManager gameManager;

    [SerializeField] private float rayCheckDistance = 2.5f;
    [SerializeField] private LayerMask obstacleLayer;

    private bool isRunning = false;
    private bool isFlying = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ExecuteCodeBlocks()
    {
        isRunning = true;
        gameManager.HidePanelsOnRun();

        List<CodeBlocks> topLevelBlocks = new List<CodeBlocks>();
        foreach (Transform child in Content.transform)
        {
            CodeBlocks cb = child.GetComponent<CodeBlocks>();
            if (cb != null)
                topLevelBlocks.Add(cb);
        }

        codeBlocks = topLevelBlocks.ToArray();
        StartCoroutine(ExecuteSequence());
    }



    public void StopRunning()
    {
        isRunning = false;
        gameManager.ShowPanelsOnStop();
        transform.position = startPos;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private IEnumerator ExecuteSequence()
    {
        foreach (CodeBlocks cb in codeBlocks)
        {
            if (!isRunning) yield break;
            yield return ExecuteSingleBlock(cb);
        }

        StopRunning();
    }

    private IEnumerator ExecuteSingleBlock(CodeBlocks cb)
    {
        if (!isRunning) yield break;

        switch (cb.type)
        {
            case CodeType.Go:
                if (IsPathBlocked(1f))
                {
                    Debug.Log("Ray engeli tespit etti. Hareket iptal edildi.");
                    StopRunning();
                    yield break;
                }

                Vector3 nextPos = transform.position + transform.forward * moveDistance;
                if (!isFlying) animator.SetBool("walking", true);
                
                yield return transform.DOMove(nextPos, moveSpeed).WaitForCompletion();
                animator.SetBool("walking", false);
                transform.DORotate(new Vector3(0f, 0f, 0f), moveSpeed);
                break;

            case CodeType.Turn_right:
                yield return transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, turnDegree, 0), moveSpeed).WaitForCompletion();
                break;

            case CodeType.Turn_left:
                yield return transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -turnDegree, 0), moveSpeed).WaitForCompletion();
                break;

            case CodeType.Place:
                Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
                if (Physics.Raycast(ray, out RaycastHit hit, 1f))
                {
                    if (hit.collider.CompareTag("target"))
                    {
                        GameObject hitTarget = hit.collider.gameObject;

                        if (!completedTargetObjects.Contains(hitTarget))
                        {
                            completedTargetObjects.Add(hitTarget);
                            Debug.Log("HEDEF TAMAMLANDI");
                        }

                        if (completedTargetObjects.Count >= targetObjects.Count)
                        {
                            Debug.Log("TÜM HEDEFLER TAMAMLANDI!");
                            StopRunning();
                            gameManager.EndLevel();
                            yield break;
                        }
                    }
                    else
                    {
                        Debug.Log("Altımda target tag'lı bir obje yok.");
                    }
                }
                else
                {
                    Debug.Log("Raycast aşağıda hiçbir şeye çarpmadı.");
                }
                break;

            case CodeType.Loop:
                int loopCount = 0;
                TMP_InputField inputField = cb.GetComponentInChildren<TMP_InputField>();
                if (inputField != null && int.TryParse(inputField.text, out loopCount) && loopCount > 0)
                {
                    Transform loopContent = cb.transform.Find("Scroll View/Viewport/Content");
                    if (loopContent != null)
                    {
                        CodeBlocks[] innerBlocks = loopContent.GetComponentsInChildren<CodeBlocks>();
                        for (int i = 0; i < loopCount; i++)
                        {
                            foreach (CodeBlocks innerCb in innerBlocks)
                            {
                                if (!isRunning) yield break;
                                yield return ExecuteSingleBlock(innerCb);
                            }
                        }
                    }
                }
                break;
            case CodeType.Function:
                Transform functionContent = cb.transform.Find("Scroll View/Viewport/Content");
                if (functionContent != null)
                {
                    CodeBlocks[] functionBlocks = functionContent.GetComponentsInChildren<CodeBlocks>();
                    foreach (CodeBlocks innerCb in functionBlocks)
                    {
                        if (!isRunning) yield break;
                        yield return ExecuteSingleBlock(innerCb);
                    }
                }
                break;
            case CodeType.Fly:
                isFlying = true;
                yield return transform.DOMoveY(2f, moveSpeed * 3f).WaitForCompletion();  // Yukarı çık
                break;
            case CodeType.Land:
                isFlying = false;
                yield return transform.DOMoveY(0.5f, moveSpeed * 3f).WaitForCompletion(); 
                break;
            case CodeType.If:
                Transform ifCondition = cb.transform.Find("Panel/Content");
                Transform ifContent = cb.transform.Find("Scroll View/Viewport/Content");

                CodeBlocks conditionBlock = ifCondition.GetComponentInChildren<CodeBlocks>();
                if (conditionBlock != null && conditionBlock.type == CodeType.pathCheck)
                {
                    Debug.Log("If içinde pathCheck blok var");

                    if (IsPathBlocked(1f))
                    {
                        Debug.Log("Engel var, if bloğunun içi çalıştırılıyor.");
                        foreach (Transform child in ifContent)
                        {
                            CodeBlocks innerCb = child.GetComponent<CodeBlocks>();
                            if (innerCb != null)
                            {
                                Debug.Log("Çalıştırılan iç blok: " + innerCb.gameObject.name);
                                if (!isRunning) yield break;
                                yield return ExecuteSingleBlock(innerCb);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Engel yok, if bloğu atlanıyor.");
                    }
                }
                else
                {
                    Debug.Log("If koşulu yok veya pathCheck değil.");
                }
                break;



        }
    }

    

    private bool IsPathBlocked(float duration)
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * rayCheckDistance, Color.red, duration);

        return Physics.Raycast(origin, direction, out RaycastHit hit, rayCheckDistance, obstacleLayer);
    }
}
