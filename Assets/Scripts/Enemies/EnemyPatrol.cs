using UnityEngine;

namespace NinjaThea.Enemies
{
    public class EnemyPatrol : MonoBehaviour
    {
        [Header("Patrol points")]
        [SerializeField] private Transform leftEdge;
        [SerializeField] private Transform rightEdge;

        [Header("Enemy")]
        [SerializeField] private Transform enemy;
        [SerializeField] private float speed = 2;
        [SerializeField] private bool movingLeft;

        private Enemy enemyScript;
        private Vector3 initScale;

        private void Awake()
        {
            initScale = enemy.localScale;
            enemyScript = enemy.GetComponent<Enemy>();
        }

        private void Update()
        {
            if (!enemyScript.IsDead() && !enemyScript.IsIdle())
            {
                if (movingLeft)
                {
                    if (enemy.position.x >= leftEdge.position.x) Patrol(-1);
                    else ChangeDirection();
                }
                else
                {
                    if (enemy.position.x <= rightEdge.position.x) Patrol(1);
                    else ChangeDirection();
                }
            }
        }

        private void Patrol(int direction)
        {
            enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * (enemyScript.IsFacingLeft() ? -direction : direction), initScale.y, initScale.z);
            enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);
        }

        private void ChangeDirection()
        {
            movingLeft = !movingLeft;
        }

    }
}
