using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 3f;
    Vector2 rightAttackOffset;
    public Collider2D swordCollider;

    // Start is called before the first frame update
    void Start()
    {
        rightAttackOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackRight() {
        print("Attack right");
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft() {
        print("Attack left");
        swordCollider.enabled = true;
        transform.localPosition = new Vector2(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack() {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.Health -= damage;
            }
        }
    }
}
