using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    private PlayerControls playerControls;
    
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private bool attackButtonDown, isAttacking = false;


    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void Start()
    {
        // Type of subscription to a mouse click event.
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

    }

    private void Update()
    {
        Attack();
    }

    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            isAttacking=true;
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }

    public void NullifyCurrentWeapon()
    {
        CurrentActiveWeapon = null;
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }
}
