using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{

    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    [Header("- Movement")]
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float dashSpeedMultiplier = 4.0f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCD = 0.65f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private Knockback knockback;

    private bool facingLeft = false;
    private bool isDashing = false;
    private float baseMoveSpeed;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        // Subscribing to dash
        playerControls.Combat.Dash.performed += _ => Dash();

        baseMoveSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        AdjustPlayerFacingDirection();
        PlayerInput();
    }

    private void FixedUpdate() {

        Move();        
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void Move() {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) { return; }
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);

    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePosition.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            facingLeft = true;

        }
        else
        {
            mySpriteRenderer.flipX = false;
            facingLeft = false;

        }
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeedMultiplier;
            trailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = baseMoveSpeed;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

}
