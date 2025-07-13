using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource deathSound;

    private bool isDead = false;
    private StageLoader stageLoader;

    private void Start()
    {
        stageLoader = FindFirstObjectByType<StageLoader>();
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
        isDead = true;
        ResetAllAnimatorTriggers();
        deathSound.Play();
        animator.SetTrigger("Death");
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void Restart()
    {
        stageLoader.LoadStageByIndex(SceneManager.GetActiveScene().buildIndex);
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
