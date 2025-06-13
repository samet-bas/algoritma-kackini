using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class RobotController : MonoBehaviour
{
    public float moveDistance = 2.5f;
    public float moveSpeed = 1.5f;
    private float turnDegree = 90f;
    public GameObject Content;
    private CodeBlocks[] codeBlocks;

    public List<GameObject> targetObjects; 
    private List<GameObject> completedTargetObjects = new List<GameObject>();

    [Header("Engeller")]
    public List<Vector3> blockedPositions = new List<Vector3>();

    
    private Animator animator;
    public GameManager gameManager;

    [SerializeField] private float completionTolerance = 0.1f;

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
        transform.position = new Vector3(0f, 0.5f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private IEnumerator ExecuteSequence()
    {
        foreach (CodeBlocks cb in codeBlocks)
        {
            switch (cb.type)
            {
                case CodeType.Go:
                    Vector3 nextPos = transform.position + transform.forward * moveDistance;

                    // Sadece XZ düzleminde kontrol etmek için Y'yi sıfırla
                    Vector3 gridPos = new Vector3(Mathf.Round(nextPos.x), 0f, Mathf.Round(nextPos.z));

                    bool isBlocked = blockedPositions.Exists(p =>
                        Mathf.Round(p.x) == Mathf.Round(gridPos.x) &&
                        Mathf.Round(p.z) == Mathf.Round(gridPos.z)
                    );

                    if (isBlocked)
                    {
                        Debug.Log("Hedef pozisyon engelli! Hareket iptal edildi.");
                        StopRunning();
                        break; // hareket etmesin
                    }

                    animator.SetBool("walking", true);
                    yield return transform.DOMove(nextPos, moveSpeed).WaitForCompletion();
                    animator.SetBool("walking", false);
                    break;


                case CodeType.Turn_right:
                    yield return transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, turnDegree, 0), moveSpeed).WaitForCompletion();
                    break;

                case CodeType.Turn_left:
                    yield return transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -turnDegree, 0), moveSpeed).WaitForCompletion();
                    break;

                case CodeType.Place:
                    foreach (GameObject target in targetObjects)
                    {
                        if (completedTargetObjects.Contains(target))
                            continue;

                        Vector3 robotFlatPos = new Vector3(transform.position.x, 0f, transform.position.z);
                        Vector3 targetFlatPos = new Vector3(target.transform.position.x, 0f, target.transform.position.z);

                        if ((robotFlatPos - targetFlatPos).sqrMagnitude <= completionTolerance * completionTolerance)
                        {
                            completedTargetObjects.Add(target);
                            Debug.Log("HEDEF TAMAMLANDI");
                        }

                    }

                    if (completedTargetObjects.Count >= targetObjects.Count)
                    {
                        Debug.Log("TÜM HEDEFLER TAMAMLANDI!");
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
