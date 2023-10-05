using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public enum PlayerState
{
    Walk,
    Run,
    Attack // Nuevo estado de ataque
}


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator animator;
    private Vector2 moveInput;
    private InputManager inputManager; // Referencia al InputManager
    private PlayerState currentState;


    public float Speed = 4;
    public float MaxSpeed = 8;
    public PlayerState defaultState = PlayerState.Walk; // Estado por defecto


    private void Awake()
    {
        currentState = defaultState;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
    }


    private void Update()
    {
        if (inputManager.IsActionButtonHold && currentState != PlayerState.Attack)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.Walk || currentState == PlayerState.Run)
        {
            UpdateAnimationAndMove();
        }
    }


    private void UpdateAnimationAndMove()
    {
        float currentSpeed = (currentState == PlayerState.Run) ? MaxSpeed : Speed;


        Vector2 moveDirection = moveInput.normalized;
        Vector2 velocity = moveDirection * currentSpeed;
        myRigidbody.velocity = velocity;


        if (moveInput.magnitude > 0)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }


    private IEnumerator AttackCo()
    {
        animator.SetBool("Attacking", true);
        currentState = PlayerState.Attack; // Cambia al estado de ataque
        yield return null;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.33f);
        currentState = defaultState; // Vuelve al estado por defecto
    }


    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }


    public void OnRun(InputValue input)
    {
        if (input.isPressed)
        {
            currentState = PlayerState.Run;
        }
        else
        {
            currentState = defaultState;
        }
    }
}

