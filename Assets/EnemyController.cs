using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Animator anim;
    private bool dead;
    public float totalHealth;
    public float currentHealth;
    public float expGranted;
    public float atkDamage;
    public float atkSpeed;
    public float moveSpeed;

    private GameObject[] players;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        currentHealth = totalHealth;

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void GetHit(float damage)
    {
        if (dead) return;
        anim.SetInteger("Condition", 3);
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(RecoverFromHit());

    }

    void Die()
    {
        dead = true;
        DropLoot();
        foreach(GameObject go in players)
        {
            go.GetComponent<PlayerController>().SetExperience(expGranted/players.Length);
        }
        anim.SetInteger("Condition", 4);
        GameObject.Destroy(this.gameObject, 5);

    }

    void DropLoot()
    {
        print("Get the bounty");
    }
    IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetInteger("Condition", 0);
    }
}
