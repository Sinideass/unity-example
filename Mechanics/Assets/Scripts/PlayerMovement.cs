using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isRunning;

    public float baseSpeed = 3.0f;
    public float maxSpeed = 6.0f; 
    public Transform hand;
    private int vidaPersonaje = 6;

    [SerializeField] UIManager uIManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float currentSpeed = isRunning ? maxSpeed : baseSpeed; 
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0);
        rb.velocity = move.normalized * currentSpeed;

        if (moveInput.magnitude > 0)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            hand.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }

        Animate();
    }
    public void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    public void OnRun(InputValue input)
    {
        isRunning = input.isPressed; 
    }

    private void Animate()
    {
        if (moveInput.magnitude > 0)
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
            animator.Play("Run");
        }
        else
        {
            animator.Play("Idle");
        }
    }

    public void CausarHerida()
    {
        if (vidaPersonaje>0)
        {
            vidaPersonaje--;
            uIManager.RestaCorazones(vidaPersonaje);
            if (vidaPersonaje == 0)
            {
                Debug.Log("Hemos muerto");
            }
        }
    }
}