using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 1.5f;

    private float laserRange;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (spriteRenderer.size.x < laserRange) {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            spriteRenderer.size = new Vector2(Mathf.Lerp(1f,laserRange,linearT),1f);

            yield return null;
        }
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = transform.position - mousePosition;

        transform.right = -direction;
    }
}
