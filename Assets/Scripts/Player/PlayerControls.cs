using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("Player References")]
    private CameraControls cameraControls;
    private CharacterController characterController;
    private Animator animator;
    private PlayerStats playerStats;

    [Header("Player Settings")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float playerMaxSpeed;
    [SerializeField] private float playerRotationSpeed;
    [SerializeField] private float jumpSpeed;

    private float playerYSpeed;
    private bool isJumping;
    private Vector2 moveInput;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource walkSoundEffect;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
   
    private void Awake()
    {
        cameraControls = Camera.main.GetComponent<CameraControls>();
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();

        playerInput = new PlayerInput();
        moveAction = playerInput.Player.Move;
        jumpAction = playerInput.Player.Jump;
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;
        jumpAction.performed += ctx => isJumping = true; 
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
        moveAction.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled -= ctx => moveInput = Vector2.zero;
        jumpAction.performed -= ctx => isJumping = true;
    }

    private void FixedUpdate()
    {
        Vector3 playerMove = Quaternion.Euler(0, camTransform.rotation.eulerAngles.y, 0)
            * new Vector3(moveInput.x, 0, moveInput.y).normalized;

        float moveAmount = playerMove.magnitude;

        if (characterController.isGrounded)
        {
            //Player jumps if the buttom is pressed, otherwise it resets 
            playerYSpeed = isJumping ? jumpSpeed : -2.0f;
            isJumping = false;

            characterController.Move(Vector3.down * 0.1f);
        }
        else
        {
            playerYSpeed += Physics.gravity.y * Time.fixedDeltaTime;
        }

        //float moveAmount;
        Vector3 velocity = playerMove * (moveAmount * playerMaxSpeed);
        velocity.y = playerYSpeed;

        characterController.Move(velocity * Time.fixedDeltaTime);

        if (playerMove != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(playerMove, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerRotationSpeed * Time.fixedDeltaTime);
        }

        HandleWalkSound(moveAmount);

        if (moveAmount > 0.1f && playerStats.Stamina > 0) // Only reduce stamina when moving
        {
            float playerSpeed = velocity.magnitude; // Current speed of the player

            if (playerSpeed > 6f) 
            {
                float maxStamina = playerStats.MaxStamina; // Reduce 1 stamina per frame when running
            }
        }
    }

    private void Update()
    {
        Vector3 playerMove = new(moveInput.x, 0, moveInput.y);
        float moveAmount = Mathf.Clamp01(playerMove.magnitude);
        animator.SetFloat("moveAmount", moveAmount, 0.05f, Time.deltaTime);
    }

    private void HandleWalkSound(float moveAmount)
    {
        if (moveAmount > 0 && characterController.isGrounded)
        {
            if (!walkSoundEffect.isPlaying)
            {
                walkSoundEffect.Play();
            }
        }
        else
        {
            if (walkSoundEffect.isPlaying)
            {
                walkSoundEffect.Stop();
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
