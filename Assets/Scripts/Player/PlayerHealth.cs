using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{

    public bool isDead {  get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackThrust = 3.0f;
    [SerializeField] private float recoveryTime = 1f;

    private Slider healthSlider;
    private Knockback knonockBack;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_SCENE_NAME = "Town";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

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
        isDead = false;
        canTakeDamage = true;
        currentHealth = maxHealth;
        UpdateHealthSlider();
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
        if (currentHealth < maxHealth)
        {
            currentHealth += healthAmount;
            UpdateHealthSlider();
        }
        

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
        UpdateHealthSlider();
        CheckIfPlayerDeath();

    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_SCENE_NAME);
    }

    private IEnumerator RecoveryRoutine()
    {
        yield return new WaitForSeconds(recoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
