using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float velocity;
    public Rigidbody rb;
    public Animator anim;

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
        // causes our character to move left
        if (Input.GetKey(KeyCode.A))
        {
            SetVelocity(-1);
        } else if(Input.GetKeyUp(KeyCode.A))
        {
            SetVelocity(0);
        }
        // causes our character to move right
        if (Input.GetKey(KeyCode.D))
        {
            SetVelocity(1);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            SetVelocity(0);
        }
    }
    
    void Move()
    {
        if(velocity == 0) {
            anim.SetInteger("Condition", 0);
            return;
        }
        else {
            anim.SetInteger("Condition", 1);
        }
        rb.MovePosition(transform.position + (Vector3.right * velocity * moveSpeed * Time.deltaTime));
    }
    
    void SetVelocity(float dir)
    {
        if (dir < 0) transform.LookAt(transform.position + Vector3.left);
        else if(dir > 0) transform.LookAt(transform.position + Vector3.right);
        velocity = dir;
    }
}
