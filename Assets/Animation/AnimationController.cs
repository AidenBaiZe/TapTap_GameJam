using UnityEngine;

namespace Animation
{
    public class AnimationController: MonoBehaviour
    {
            public Animator animator;

            private int xSpeedHash;

            private int ySpeedHash;

            private int onGroundHash;

            public Rigidbody2D rb;
            
            public PlayerMovement playerMovement;

            public void Awake()
            {
                animator = GetComponent<Animator>();
                rb = GetComponent<Rigidbody2D>();
                playerMovement = GetComponent<PlayerMovement>();

                xSpeedHash = Animator.StringToHash("XSpeed");
                onGroundHash = Animator.StringToHash("OnGround");
                ySpeedHash = Animator.StringToHash("YSpeed");
            }

            void Update()
            {
                animator.SetFloat(xSpeedHash, Mathf.Abs(rb.linearVelocityX));
                animator.SetBool(onGroundHash, playerMovement.isGrounded);
                animator.SetFloat(ySpeedHash, playerMovement.Gravity > 0 ? rb.linearVelocityY : -1 * rb.linearVelocityY);
            }
    }
}