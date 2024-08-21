using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
public class Gargoyle : Unit
{
    private UnitCircle unitCircleScript;
    private float healthWall;
    private GameObject unitCircle;
    private enum GargoyleState
    {
        Wall,
        Idle,
        Attack,
        Move
    }
    private GargoyleState state = GargoyleState.Idle;
    private AudioSource audioSource;
    private Animator animator;
    private bool isDeadCoroutineStarted = false;
    [SerializeField] private AudioClip deathSound;
    private SpriteRenderer sprite;
    private PlayerMovement player;
    // Propriétés booléennes pour vérifier l'état
    private bool IsWall
    {
        get => state == GargoyleState.Wall;
        set
        {
            if (value) state = GargoyleState.Wall;
            animator.SetBool("isWall", value);
            animator.SetBool("isIdle", !value);
        }
    }

    private bool IsIdle
    {
        get => state == GargoyleState.Idle;
        set
        {
            if (value) state = GargoyleState.Idle;
            animator.SetBool("isIdle", value);
            animator.SetBool("isWall", !value);
        }
    }

    private bool IsAttacking
    {
        get => state == GargoyleState.Attack;
        set
        {
            if (value) state = GargoyleState.Attack;
            animator.SetBool("isAttacking", value);
        }
    }

    private bool IsMoving
    {
        get => state == GargoyleState.Move;
        set
        {
            if (value) state = GargoyleState.Move;
            animator.SetBool("isMoving", value);
        }
    }

    void Awake()
    {
        // Obtenez la référence à l'AudioSource et à l'Animator
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource n'est pas attaché au GameObject.");
        }
        healthWall = health * 2;
        Debug.Log("Gargoyle health is " + health);
        Debug.Log("Gargoyle healthwall is " + healthWall);
        TakeDamage(100);
        Debug.Log("Gargoyle health is " + health);
        unitCircleScript = GetComponentInChildren<UnitCircle>();

        if (unitCircleScript != null)
        {
            unitCircle = unitCircleScript.gameObject;
        }
        else
        {
            Debug.LogWarning("UnitCircle script not found on any children of Gargoyle.");
        }
    } 
    override protected void Die()
    {
        if (health <= 0 && !isDeadCoroutineStarted)
        {
            StartCoroutine(HandleDeath());
        }
    }
    private IEnumerator HandleDeath()
    {
        isDeadCoroutineStarted = true;
        animator.SetBool("isDeath", true); // Assurez-vous que le trigger "Die" existe dans l'Animator
        audioSource.clip = deathSound;
        audioSource.Play();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
    protected override void Update()
    {
        base.Update();
        Die();
        if (unitCircleScript != null && unitCircleScript.isPlayerInside)
        {
            player.coffin_input.enabled = true;
            if (Input.GetButtonDown("Coffin"))
            {
                if (IsIdle)
                {
                    IsWall = true;
                    Debug.Log("Gargoyle is wall.");
                }
                else if (IsWall)
                {
                    IsIdle = true;
                    Debug.Log("Gargoyle is now idle.");
                }
            }
        }
        else
        {
            player.coffin_input.enabled = false;
        }
        if (IsWall)
        {
            unitCircleScript.triggerActive = false;
        }
        else if (IsIdle)
        {
            unitCircleScript.triggerActive = true;
        }
        else if (IsAttacking)
        {
            unitCircleScript.triggerActive = true;
        }
        else if (IsMoving)
        {
            unitCircleScript.triggerActive = true;
        }
    }
}

