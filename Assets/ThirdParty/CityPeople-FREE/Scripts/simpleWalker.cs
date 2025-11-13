using UnityEngine;

public class SimpleWalker : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float roadStart = 117f;
    public float roadEnd = 21f;
    public bool useZAxis = true;

    //캐릭터가 뒤로 걷는다면 체크, 평소에는 비워두기
    public bool reverseDirection = false;
    //걷기 애니메이션 클립 이름 (비워두면 자동)
    public string walkAnimationName = "";

    private Animator animator;
    private bool hasFlipped = false;
    private float maxPos;
    private float minPos;
    private Vector3 moveDirection;

    void Start()
    {
        animator = GetComponent<Animator>();

        // 최적화: 최대/최소값 미리 계산
        maxPos = Mathf.Max(roadStart, roadEnd);
        minPos = Mathf.Min(roadStart, roadEnd);

        // 최적화: 이동 방향 미리 계산
        moveDirection = reverseDirection ? Vector3.back : Vector3.forward;

        // 걷기 애니메이션 재생
        if (animator != null && !string.IsNullOrEmpty(walkAnimationName))
        {
            animator.CrossFadeInFixedTime(walkAnimationName, 0.2f);
        }
    }

    void Update()
    {
        // 이동
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 현재 위치
        float currentPos = useZAxis ? transform.position.z : transform.position.x;

        // 경계 체크 및 방향 전환
        if (!hasFlipped)
        {
            if (currentPos >= maxPos || currentPos <= minPos)
            {
                transform.Rotate(0f, 180f, 0f);
                hasFlipped = true;
            }
        }
        else if (currentPos < maxPos - 1f && currentPos > minPos + 1f)
        {
            hasFlipped = false;
        }
    }
}