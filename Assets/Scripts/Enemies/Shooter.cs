using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [Header("Bursts")]
    [SerializeField] private float projectilesPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBurts;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate;

    private bool isShooting = false;

    private void OnValidate()
    {
        if (oscillate) { stagger = true; }
        if (!oscillate) { stagger = false; }
        if (projectilesPerBurst < 1) { projectilesPerBurst = 1;}
        if (burstCount< 1) { burstCount = 1; }
        if (timeBetweenBurts < 0.1f) { timeBetweenBurts = 0.1f; }


    }

    public void Attack()
    {
        if (!isShooting) {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger)
        {
            timeBetweenProjectiles = timeBetweenBurts / projectilesPerBurst;
        }

        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate) {
            TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

            } 
            
            if (oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            } else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            // Creating a cluster of projectiles.
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 position = FindBulletSpawnPosition(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, position, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;


                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);

                }

                currentAngle += angleStep;

                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }

            }
            currentAngle = startAngle;

            if (!stagger) { yield return new WaitForSeconds(timeBetweenBurts); }
                
        }

        yield return new WaitForSeconds(restTime);

        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targeAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targeAngle;
        endAngle = targeAngle;
        currentAngle = targeAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targeAngle - halfAngleSpread;
            endAngle = targeAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPosition(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 position = new Vector2(x,y);

        return position;
    }

}
