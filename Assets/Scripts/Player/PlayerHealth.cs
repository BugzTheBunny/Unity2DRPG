using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackThrust = 3.0f;
    [SerializeField] private float recoveryTime = 1f;

    private Knockback knonockBack;
    private Flash flash;

    private int currentHealth;
    private bool canTakeDamage;

    protected override void Awake()
    {
        base.Awake();
        knonockBack = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        canTakeDamage = true;
        currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, collision.transform);
        }
    }

    public void HealPlayer(int healthAmount)
    {
        currentHealth += healthAmount;
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }
        ScreenShakeManager.Instance.ShakeScreen();
        knonockBack.GetKnockedBack(hitTransform, knockbackThrust);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(RecoveryRoutine());

    }

    private IEnumerator RecoveryRoutine()
    {
        yield return new WaitForSeconds(recoveryTime);
        canTakeDamage = true;
    }
}
