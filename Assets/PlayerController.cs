using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Movement")]
    public float moveSpeed;
    public float velocity;
    public Rigidbody rb;
    public Animator anim;

    [Header("Combat")]
    private bool attacking; 

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
                rb.MovePosition(transform.position + (Vector3.right * velocity * moveSpeed * Time.deltaTime));
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
        if (attacking) return;
        anim.SetInteger("Condition", 2);
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        attacking = true;
        yield return new WaitForSeconds(1);
        attacking = false;
    }
}
