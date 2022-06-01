using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    public GameObject player;
    public float attackRange = 0.1f;
    public float timeBetweenAttacks = 1f;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    bool canMove = true;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    float timeSinceLastAttack = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        timeSinceLastAttack += Time.deltaTime;
        float distance = Vector2.Distance(transform.localPosition, player.transform.localPosition);
        if (distance > attackRange) {
            Vector2 newPosition = Vector2.MoveTowards(transform.localPosition, player.transform.localPosition, Time.deltaTime);
            if (player.transform.position.x < transform.position.x && changed(player.transform.position.y, transform.position.y)) {
                spriteRenderer.flipX = true;
            } else if (changed(transform.position.y, player.transform.position.y)) {
                spriteRenderer.flipX = false;
            }
            rb.MovePosition(newPosition);
        } else {
            // if (Mathf.Abs(player.transform.position.x - transform.position.x) != 0 && changed(transform.position.x, player.transform.position.x)) {
            //     rb.velocity = new Vector2(0, 1);
            // } else if (changed(transform.position.x, player.transform.position.x)) {
            //     rb.velocity = new Vector2(0, -1);
            // } else {
                SwordAttack();
            // }
        }
        if (canMove) {
            if (movementInput != Vector2.zero) {
                bool success = TryMove(movementInput);

                if (!success && movementInput.x > 0) {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if (!success && movementInput.y > 0) {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

                animator.SetBool("isMoving", success);
            } else {
                animator.SetBool("isMoving", false);
            }

            if (movementInput.x < 0) {
                spriteRenderer.flipX = true;
            } else if (movementInput.x > 0) {
                spriteRenderer.flipX = false;
            }
        }
    }

    private bool TryMove(Vector2 direction) {
        if (direction != Vector2.zero) {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );
            
            if (count == 0) {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    void OnFire() {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack() {
        LockMovement();
        if (spriteRenderer.flipX == true) {
            if (timeSinceLastAttack > timeBetweenAttacks) {
                timeSinceLastAttack = 0;
                swordAttack.AttackLeft();
                OnFire();
            }
        } else {
            if (timeSinceLastAttack > timeBetweenAttacks) {
                timeSinceLastAttack = 0;
                swordAttack.AttackRight();
                OnFire();
            }
        }
    }

    public void EndSwordAttack() {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement() {
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }

    bool changed(float a, float b) {
        if ((int)a == (int)b) {
            return true;
        } else {
            return false;
        }
    }
}
