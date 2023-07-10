using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float attackCD = 0.5f;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    private bool attackButtonDown, isAttacking = false;

    private GameObject slashAnim;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    void Start()
    {
        // Type of subscription to a mouse click event.
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private IEnumerator AttackCDRoutine() 
    {
        yield return new WaitForSeconds(attackCD);
        isAttacking = false;
    }

    private void Attack()
    {   if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
            slashAnim = Instantiate(slashAnimationPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
            StartCoroutine(AttackCDRoutine());
        }

    }


    public void DoneAttackingAnimationEvent() 
    {
        weaponCollider.gameObject.SetActive(false);

    }

    public void SwingUpFlipAnimationEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180,0,0);
        
        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimationEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        } 
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y,mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x) { 
            activeWeapon.transform.rotation = Quaternion.Euler(0,-180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        if (mousePos.x > playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }

}
