using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private AudioClip attackSound;


    [Header ("Sight Parameters")]
    [SerializeField] private float boxRange = 1;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float boxDistance = 0;
    [SerializeField] private LayerMask playerLayer;


    private float cooldownTimer = Mathf.Infinity;
    private Health playerHealth;

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
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
            {
                // attack;
                SoundManager.instance.PlaySound(attackSound);
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
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

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * transform.localScale.x * boxDistance,
            new Vector3(boxRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            // Damage player health
            playerHealth.TakeDamage(damage);
        }
    }
}
