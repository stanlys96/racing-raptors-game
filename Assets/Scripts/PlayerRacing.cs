using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRacing : MonoBehaviour
{
    public float speed = 1f;
    public int tokenId;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;

    private float dashLength = 0.3f;
    private float dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;
    private bool onDash = false;
    private float dashSpeed = 1.6f;

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
    }
}
