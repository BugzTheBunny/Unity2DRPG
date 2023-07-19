using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particlePrefabVFX;
    [SerializeField] private bool IsEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;
    
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(startPosition, transform.position) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = collision.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (!collision.isTrigger &&  (enemyHealth || indestructible || player))
        {

            if ((player && IsEnemyProjectile) ||(enemyHealth && !IsEnemyProjectile))
            {
                player?.TakeDamage(1,transform);
                Instantiate(particlePrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            } else if (!collision.isTrigger && indestructible) {
                Instantiate(particlePrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }


        }


    }
}
