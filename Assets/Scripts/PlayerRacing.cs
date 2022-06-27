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
    public bool isMain = false;

    public SpriteRenderer spriteRenderer;
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
    public float[] speeds = {};
    private float checkTime = 0f;
    private bool isFinished = false;
    private float[] mainObjectSpeeds = { 1f, 1.05f, 0.95f, 0.94f, 1.1f, 1f, 1.2f };
    private float[] firstPlaceSpeeds = { 0.98f, 0.95f, 1.1f, 1.03f, 1.05f, 1f, 1.2f };
    private float[] secondPlaceSpeeds = { 1f, 1.05f, 0.95f, 0.94f, 1.1f, 1f, 1.2f };
    private float[] thirdPlaceSpeeds = { 0.95f, 1.03f, 1.1f, 0.94f, 0.95f, 0.98f, 1.15f };
    private float[] fourthPlaceSpeeds = { 1.03f, 1.1f, 0.95f, 0.9f, 0.9f, 0.91f, 1.1f };
    private float[] fifthPlaceSpeeds = { 1.1f, 1.03f, 0.9f, 0.95f, 0.9f, 0.85f, 1.1f };
    private float[] sixthPlaceSpeeds = { 1f, 1.03f, 1f, 0.9f, 0.9f, 0.88f, 1.1f };
    private float[] seventhPlaceSpeeds = { 1.1f, 1.03f, 0.9f, 0.88f, 0.85f, 0.9f, 1.1f };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (gameObject.tag == "MainObject")
        {
            speeds = secondPlaceSpeeds;
        }
        if (tokenId == 8)
        {
            speeds = firstPlaceSpeeds;
        }
        if (tokenId == 7)
        {
            speeds = secondPlaceSpeeds;
        }
        if (tokenId == 6)
        {
            speeds = thirdPlaceSpeeds;
        }
        if (tokenId == 21)
        {
            speeds = fourthPlaceSpeeds;
        }
        if (tokenId == 24)
        {
            speeds = fifthPlaceSpeeds;
        }
        if (tokenId == 3)
        {
            speeds = sixthPlaceSpeeds;
        }
        if (tokenId == 23)
        {
            speeds = seventhPlaceSpeeds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkTime += Time.deltaTime;
        timeSinceLastChangeSeason += Time.deltaTime;
        if (timeSinceLastChangeSeason > changeSeason)
        {
            timeSinceLastChangeSeason = 0f;
            index++;
        }
        float speed = isFinished ? 0f : speeds[index];
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMain)
        {
            if (collision.tag == "FinishLine")
            {
                isFinished = true;
            }
        }
    }
}
