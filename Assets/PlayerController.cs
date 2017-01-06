using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Animator anim;

    private int level = 1;
    private Text levelText;
    public float experience { get; private set; }
    private Transform experienceBar;

    [Header("Movement")]
    private bool canMove = true;
    public float moveSpeed;
    public float velocity;
    public float jumpStrength;
    public Rigidbody rb;


    [Header("Combat")]
    private List<Transform> enemiesInRange = new List<Transform>();
    private bool canAttack = true;
    private bool attacking;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;

    [Header("Stats")]
    private float strength;
    private float vitality;
    private float agility;
    private float intelligence;
    private float dexterity;
    private float cunningness;

	// Use this for initialization
	void Start () {
     //   AnimationEvents.OnSlashAnimationHit += DealDamage;
      //  AnimationEvents.OnJumpAnimationHit += JumpCallback;
        experienceBar = UIController.instance.transform.Find("Background/Experience");
        levelText = UIController.instance.transform.Find("Background/Level_Text").GetComponent<Text>();
        SetExperience(0);
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
        if (!canAttack) return;
        anim.speed = attackSpeed;
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
        enemiesInRange.Clear();
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * 0.5f), 0.5f))
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                enemiesInRange.Add(c.transform);
            }
        }
    }

    public void SetExperience(float exp)
    {
        experience += exp;
        float experienceNeeded = GameLogic.ExperienceForNextLevel(level);
        float previousExperience = GameLogic.ExperienceForNextLevel(level - 1);
        if(experience >= experienceNeeded)
        {
            LevelUp();
            experienceNeeded = GameLogic.ExperienceForNextLevel(level);
            previousExperience = GameLogic.ExperienceForNextLevel(level - 1);
        }
        experienceBar.Find("Fill").GetComponent<Image>().fillAmount = (experience - previousExperience) / (experienceNeeded - previousExperience);
    }
    void LevelUp()
    {
        level++;
        levelText.text = "Lv. " + level.ToString("00");
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            Debug.Log("Landed");
            anim.SetInteger("Condition", 6);
            yield return new WaitForSeconds(0.05f);
            anim.SetInteger("Condition", 0);
        }
    }
}
