using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Animator anim;

    [Header("Movement")]
    private bool canMove;
    public float movementSpeed;
    public float velocity;
    public Rigidbody rb;


    [Header("Combat")]
    private List<Transform> enemiesInRange = new List<Transform>();
    private bool canAttack = true;
    private bool attacking;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
        Move();
	}

    void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Attacking");
            Attack();
        }
        // causes our character to move left
        if (Input.GetKey(KeyCode.A))
        {
            SetVelocity(-1);
        } else if(Input.GetKeyUp(KeyCode.A))
        {
            SetVelocity(0);
            anim.SetInteger("Condition", 0);
        }
        // causes our character to move right
        if (Input.GetKey(KeyCode.D))
        {
            SetVelocity(1);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            SetVelocity(0);
            anim.SetInteger("Condition", 0);
        }
    }
    
    void Move()
    {
        if(velocity == 0) {
      //      anim.SetInteger("Condition", 0);
            return;
        }
        else {
            if(!attacking)
            {
                anim.SetInteger("Condition", 1);
                rb.MovePosition(transform.position + (Vector3.right * velocity * movementSpeed * Time.deltaTime));
            }
        }
    }
    
    void SetVelocity(float dir)
    {
        if (dir < 0) transform.LookAt(transform.position + Vector3.left);
        else if(dir > 0) transform.LookAt(transform.position + Vector3.right);
        velocity = dir;
    }

    void Attack()
    {
        if (!canAttack) return;
        anim.SetInteger("Condition", 2);
        StartCoroutine(AttackRoutine());
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackRoutine()
    {
        canMove = false;
        yield return new WaitForSeconds(0.1f);
        anim.SetInteger("Condition", 0);
        GetEnemiesInRange();
        foreach(Transform enemy in enemiesInRange)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec == null) continue;
            ec.GetHit(attackDamage);
        }

        yield return new WaitForSeconds(0.65f);
        canMove = true;
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(1 / attackSpeed);
        canAttack = true;
    }

    void GetEnemiesInRange()
    {
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * 0.5f), 0.5f))
        {
            if (c.gameObject.CompareTag("Enemey"))
            {
                enemiesInRange.Add(c.transform);
            }
        }
    }
}
