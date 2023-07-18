using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 1.5f;

    private bool isGrowing = true;

    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Indestructible>() && !collision.isTrigger) {
            isGrowing = false;
        }   
    }

    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (spriteRenderer.size.x < laserRange && isGrowing) {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            // sprite resize
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f,laserRange,linearT),1f);

            // collider resize
            capsuleCollider.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT),capsuleCollider.size.y);
            capsuleCollider.offset = new Vector2((Mathf.Lerp(1f, laserRange, linearT)) / 2, capsuleCollider.offset.y);
            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = transform.position - mousePosition;

        transform.right = -direction;
    }
}
