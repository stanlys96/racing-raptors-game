using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRacing : MonoBehaviour
{
    public float maxSpeed = 1f;
    public float minSpeed = 0.9f;
    public int tokenId;
    public bool canJump = false;
    public GameObject target;
    public bool isMain = false;
    public Text uiTokenId;

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
    private float[] eighthPlaceSpeeds = { 0.9f, 0.85f, 1.1f, 0.88f, 1f, 0.85f, 1f };
    private bool isRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (uiTokenId != null)
        {
            uiTokenId.text = tokenId.ToString();
        }
        if (gameObject.tag == "MainObject")
        {
            speeds = firstPlaceSpeeds;
        }
        if (tokenId == APICall.instance.top3[0])
        {
            speeds = firstPlaceSpeeds;
        }
        if (tokenId == APICall.instance.top3[1])
        {
            speeds = secondPlaceSpeeds;
        }
        if (tokenId == APICall.instance.top3[2])
        {
            speeds = thirdPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[0])
        {
            speeds = fourthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[1])
        {
            speeds = fifthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[2])
        {
            speeds = sixthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[3])
        {
            speeds = seventhPlaceSpeeds;
        }
        if (tokenId == APICall.instance.theRestRacer[4])
        {
            speeds = eighthPlaceSpeeds;
        }
        if (tokenId == APICall.instance.fighters[0])
        {
            gameObject.tag = "Fighter1";
        }
        if (tokenId == APICall.instance.fighters[1])
        {
            gameObject.tag = "Fighter2";
        }

        foreach (KeyValuePair<int, RuntimeAnimatorController> entry in APICall.instance.tokenIdToSprite)
        {
            if (tokenId == entry.Key)
            {
                animator.runtimeAnimatorController = entry.Value;
            }
        }
    }

    IEnumerator DoRotation(float speed, float amount, Vector3 axis)
    {
        isRotating = true;
        float rot = 0f;
        while (rot < amount)
        {
            yield return null;
            float delta = Mathf.Min(speed * Time.deltaTime, amount - rot);
            transform.RotateAround(target.transform.position, axis, delta);
            rot += delta;
        }
        isRotating = false;
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
            if (index == speeds.Length)
            {
                index = 0;
            }
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
        if (tokenId == APICall.instance.fighters[0] && index == 1)
        {
            GameObject targetFighter = GameObject.FindGameObjectWithTag("Fighter2");
            transform.position = Vector2.MoveTowards(transform.position, targetFighter.transform.position, speed * Time.deltaTime);
            transform.RotateAround(targetFighter.transform.position, Vector3.one, 1f);
        } 
        else
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }

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
