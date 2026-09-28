using NinjaThea.GameElements;
using NinjaThea.UI;
using UnityEngine;

namespace NinjaThea.Player
{
    public class PlayerLife : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private AudioSource deathSound;

        private static readonly int DeathHash = Animator.StringToHash("Death");

        private bool isDead = false;
        private FinishLine finishLine;

        private void Start()
        {
            finishLine = FindAnyObjectByType<FinishLine>();
        }

        private void Update()
        {
            if (!isDead && transform.position.y < -15) Die();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Trap")) Die();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Enemy")) Die();
        }

        private void Die()
        {
            // Die once; never after the stage is finished (would reload it over the stage-end countdown)
            if (isDead || (finishLine != null && finishLine.IsFinished())) return;
            isDead = true;
            ResetAllAnimatorTriggers();
            deathSound.Play();
            animator.SetTrigger(DeathHash);
            rb.bodyType = RigidbodyType2D.Static;
        }

        // Animation event calls this
        public void Restart()
        {
            StageLoader.Instance.ReloadCurrentStage();
        }

        public bool IsDead()
        {
            return isDead;
        }

        private void ResetAllAnimatorTriggers()
        {
            foreach (var trigger in animator.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(trigger.name);
                }
            }
        }
    }
}
