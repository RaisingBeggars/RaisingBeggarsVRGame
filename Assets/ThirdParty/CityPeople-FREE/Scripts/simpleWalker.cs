using UnityEngine;

public class SimpleWalker : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // 시작하자마자 Speed 파라미터를 1로 설정 → 걷기 애니메이션 유지
        animator.SetFloat("Speed", 1f);
    }

    void Update()
    {
        // 매 프레임 앞으로 이동
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
