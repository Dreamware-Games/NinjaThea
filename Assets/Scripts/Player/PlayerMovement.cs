using NinjaThea.GameElements;
using NinjaThea.Managers;
using NinjaThea.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NinjaThea.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private BoxCollider2D coll;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource jumpSound;
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private bool facingRight = true;
        [SerializeField] private float jumpForce = 14f;
        [SerializeField] private LayerMask terrain;

        private static readonly int StateHash = Animator.StringToHash("State");

        private PlayerLife playerLife;
        private FinishLine finishLine;

        private float horizontalMove = 0f;
        private float bufferedMove = 0f;
        private bool jump = false;
        private float coyoteTime = .1f;
        private float coyoteTimeCounter;
        private float jumpBufferTime = .1f;
        private float jumpBufferCounter;

        private void OnEnable()
        {
            GameManager.OnGameStarted += OnGameplayStart;
        }

        private void Start()
        {
            playerLife = GetComponent<PlayerLife>();
            finishLine = FindAnyObjectByType<FinishLine>();
        }

        public void OnGameplayStart()
        {
            horizontalMove = bufferedMove;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed || context.started)
            {
                Vector2 input = context.ReadValue<Vector2>();
                bufferedMove = input.x;
                if (GameManager.Instance.GamePlaying && !PauseMenu.Paused)
                {
                    horizontalMove = bufferedMove;
                }
            }
            if (context.canceled)
            {
                bufferedMove = 0f;
                horizontalMove = 0f;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (!GameManager.Instance.GamePlaying || PauseMenu.Paused || playerLife.IsDead())
                return;
            jumpBufferCounter = jumpBufferTime;
        }

        private void Update()
        {
            if (playerLife.IsDead() || finishLine.IsFinished())
            {
                horizontalMove = 0f;
                jump = false;
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
                animator.SetInteger(StateHash, (int)MovementState.Idle);
                return;
            }

            if (!GameManager.Instance.GamePlaying || PauseMenu.Paused)
                return;

            if (isGrounded())
                coyoteTimeCounter = coyoteTime;
            else
                coyoteTimeCounter -= Time.deltaTime;

            jumpBufferCounter -= Time.deltaTime;

            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
                jump = true;

            UpdateAnimationState();
        }

        private void FixedUpdate()
        {
            if (playerLife.IsDead()) return;

            rb.linearVelocity = new Vector2(horizontalMove * moveSpeed, rb.linearVelocity.y);

            if (jump)
            {
                jumpSound.Play();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jump = false;
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
        }

        private void UpdateAnimationState()
        {
            MovementState state;

            if (horizontalMove != 0f)
            {
                state = MovementState.Running;

                if ((horizontalMove < 0f && facingRight) ||
                    (horizontalMove > 0f && !facingRight))
                {
                    facingRight = !facingRight;
                    transform.Rotate(0, 180, 0);
                }
            }
            else
            {
                state = MovementState.Idle;
            }

            if (rb.linearVelocity.y > .1f)
                state = MovementState.Jumping;
            else if (rb.linearVelocity.y < -.1f)
                state = MovementState.Falling;

            animator.SetInteger(StateHash, (int)state);
        }

        private bool isGrounded()
        {
            return Physics2D.BoxCast(
                coll.bounds.center,
                coll.bounds.size,
                0f,
                Vector2.down,
                .1f,
                terrain
            );
        }

        private void OnDisable()
        {
            GameManager.OnGameStarted -= OnGameplayStart;
        }

    }
}
