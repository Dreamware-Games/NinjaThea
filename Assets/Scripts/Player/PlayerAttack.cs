using System.Collections.Generic;
using NinjaThea.Enemies;
using NinjaThea.Managers;
using NinjaThea.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NinjaThea.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange = .7f;
        [SerializeField] private float attackRate = 4f;
        [SerializeField] private AudioSource attackSound;
        [SerializeField] private LayerMask enemyLayer;

        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int AttackJumpHash = Animator.StringToHash("Attack Jump");

        private float nextAttackTime = 0f;
        private PlayerLife playerLife;
        private ContactFilter2D enemyFilter;
        private readonly List<Collider2D> hitEnemies = new List<Collider2D>();

        private void Start()
        {
            playerLife = GetComponent<PlayerLife>();
            enemyFilter = new ContactFilter2D { useTriggers = Physics2D.queriesHitTriggers };
            enemyFilter.SetLayerMask(enemyLayer);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (!GameManager.Instance.GamePlaying || PauseMenu.Paused || playerLife.IsDead())
                return;
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
                animator.SetTrigger(AttackJumpHash);
            else
                animator.SetTrigger(AttackHash);
        }

        // Animation event calls this
        public void CheckEnemyHit()
        {
            Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyFilter, hitEnemies);

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
}
