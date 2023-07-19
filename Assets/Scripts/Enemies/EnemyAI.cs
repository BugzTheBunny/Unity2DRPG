using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamingDirectionChangeTime = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldownTime;
    [SerializeField] private bool stopWhileAttack = false;

    private bool canAttack = true;
    private enum State
    {
        Roaming,
        Attacking
    }

    private State state;
    private EnemyPathFinding enemyPathFinding;
    
    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default: 
            
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        enemyPathFinding.MoveTo(roamPosition);

        // If player detected, chagne state to attacking
        if (Vector2.Distance(transform.position,PlayerController.Instance.transform.position) < attackRange) { state = State.Attacking; }

        if (timeRoaming > roamingDirectionChangeTime)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange) { state = State.Roaming; }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopWhileAttack)
            {
                enemyPathFinding.StopMoving();
            } else
            {
                enemyPathFinding.MoveTo(roamPosition);
            }

        StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true;
    }
    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
