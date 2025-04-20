using UnityEngine;

public class BulletUI : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Disappear") &&
            stateInfo.normalizedTime >= 1f &&
            !animator.IsInTransition(0))
        {
            Destroy(gameObject);
        }
    }
}
