using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = .7f;
    [SerializeField] private float attackRate = 4f;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private LayerMask enemyLayer;

    private float nextAttackTime = 0f;
    private PlayerLife playerLife;

    private void Start()
    {
        playerLife = GetComponent<PlayerLife>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!GameManager.Instance.GamePlaying || PauseMenu.Paused || playerLife.IsDead()) return;
        // Cooldown
        if (Time.time < nextAttackTime)
            return;
        nextAttackTime = Time.time + 1f / attackRate;
        Attack();
    }

    private void Attack()
    {
        attackSound.Play();

        Vector2 velocity = rb.linearVelocity;

        if (velocity.y > .1f || velocity.y < -.1f)
            animator.SetTrigger("Attack Jump");
        else
            animator.SetTrigger("Attack");
    }

    // Animation event calls this
    public void CheckEnemyHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemyColl in hitEnemies)
        {
            Enemy enemy = enemyColl.GetComponent<Enemy>();
            if (!enemy.IsDead())
                enemy.Die();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}