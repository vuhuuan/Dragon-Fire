using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;


    [Header("Sight Parameters")]
    [SerializeField] private float boxRange = 1;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float boxDistance = 0;
    [SerializeField] private LayerMask playerLayer;


    [Header("Projectile Parameters")]
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private Transform firepoint;
    [SerializeField] private AudioClip fireballSound;




    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;

    private EnemyPatrol enemyPatrol;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                // attack;
                cooldownTimer = 0;
                anim.SetTrigger("rangeAttack");
                RangeAttack();
            }
        }


        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * transform.localScale.x * boxDistance,
            new Vector3(boxRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0,
            Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * transform.localScale.x * boxDistance,
            new Vector3(boxRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void RangeAttack()
    {
        // take the prefab
        // reset position
        SoundManager.instance.PlaySound(fireballSound);
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
