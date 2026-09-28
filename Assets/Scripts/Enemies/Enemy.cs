using NinjaThea.Managers;
using UnityEngine;

namespace NinjaThea.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D coll;
        [SerializeField] private AudioSource deathSound;

        // Default behaviour is run - flip this in editor to change.
        [SerializeField] private bool idle;

        // Default behaviour is facing right - flip this in editor to face left.
        [SerializeField] private bool facingLeft;

        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private static readonly int DeathHash = Animator.StringToHash("Death");

        private bool dead = false;

        private void Start()
        {
            // Default behaviour is to start moving. Toggle idle in editor to keep enemy stationary.
            if (idle)
            {
                animator.SetTrigger(IdleHash);
            }
            if (facingLeft)
            {
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }

        public void Die()
        {
            deathSound.Play();
            GameManager.Instance.EnemyKilled();
            dead = true;
            coll.enabled = false;
            animator.SetTrigger(DeathHash);
        }

        public bool IsDead()
        {
            return dead;
        }

        public bool IsIdle()
        {
            return idle;
        }

        public bool IsFacingLeft()
        {
            return facingLeft;
        }
    }
}
