using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool GettingKnockedBack { get; private set; }

    [SerializeField] private float knockBackTime = 0.25f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockbackThrust)
    {
        GettingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockbackThrust * rb.mass; // calculating the difference for the knockback power
        rb.AddForce(difference,ForceMode2D.Impulse); // Adding force to the rb with the difference to create the knockback itself.
        StartCoroutine(KnockRoutine()); // Starting the routine of how much the the player will be knocked
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime); // Waiting 
        rb.velocity = Vector2.zero; // Setting velocity of object back to 0
        GettingKnockedBack = false;
    }
}
