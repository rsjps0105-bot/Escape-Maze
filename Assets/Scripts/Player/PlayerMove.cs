using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    public float speed = 5f;
    public float jumpPower = 3f;

    float gravity = -9.8f;
    float yVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);

        if (move.magnitude > 0.1f)
        {
            transform.forward = move;
        }

        animator.SetFloat("Speed", move.magnitude);

        // ⭐ 接地判定
        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;

            // 着地したらジャンプfalse
            animator.SetBool("Jump", false);
        }

        // ⭐ ジャンプ処理
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }

        // 重力
        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    // ⭐ ジャンプ開始
    public void JumpStart()
    {
        yVelocity = jumpPower;
    }

}
