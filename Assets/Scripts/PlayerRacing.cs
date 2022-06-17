using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRacing : MonoBehaviour
{
    public float maxSpeed = 1f;
    public float minSpeed = 0.9f;
    public int tokenId;
    public bool canJump = false;
    public GameObject target;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;

    private float dashLength = 0.3f;
    private float dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;
    private bool onDash = false;
    private float dashSpeed = 1.6f;
    private bool isJumping = false;
    private int index = 0;
    private float changeSeason = 10f;
    private float timeSinceLastChangeSeason = 0;
    private float[] speeds = { 1.03f, 1.05f, 0.95f, 0.98f, 1.1f, 1.04f, 0.97f, 1.03f };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastChangeSeason += Time.deltaTime;
        if (timeSinceLastChangeSeason > changeSeason)
        {
            timeSinceLastChangeSeason = 0f;
            index++;
        }
        float speed = speeds[index];
        //animator.SetBool("isMoving", true);
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    if (dashCoolCounter <= 0 && dashCounter <= 0)
        //    {
        //        if (gameObject.name == "7_0 (1)")
        //        {
        //            animator.SetTrigger("Jump");
        //            activeMoveSpeed = dashSpeed;
        //            dashCounter = dashLength;
        //            onDash = true;
        //        }

        //    }
        //}

        transform.Translate(Vector2.up * speed * Time.deltaTime);

        //if (gameObject.name == "7_0 (1)")
        //{
        //    print(activeMoveSpeed);
        //}
        //if (dashCounter > 0)
        //{
        //    dashCounter -= Time.deltaTime;

        //    if (dashCounter <= 0)
        //    {
        //        activeMoveSpeed = speed;
        //        dashCoolCounter = dashCooldown;
        //    }
        //}

        //if (dashCoolCounter > 0)
        //{
        //    dashCoolCounter -= Time.deltaTime;
        //}
        if (canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                animator.SetTrigger("jump");
            }
        }
    }

    public void ResetJump()
    {
        animator.ResetTrigger("jump");
    }
}
