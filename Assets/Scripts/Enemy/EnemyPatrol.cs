using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;

    [Header ("Movement parameters")]
    [SerializeField] private float speed;
    private bool movingLeft;

    [Header("Enemy animation")]
    [SerializeField] private float idleDuration;
    private float idleTimer;
    private Animator anim;

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Awake()
    {
        anim = enemy.GetComponent<Animator>();
    }
    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
            } else
            {
                ChangeDirection();
            }
        } else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            } else
            {
                ChangeDirection();
            }
        }
    }

    private void ChangeDirection()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);
        enemy.localScale = new Vector3 (_direction, 1, 1);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
