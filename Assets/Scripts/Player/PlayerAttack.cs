using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackCoolDown = 1f;
    private float coolDownTimer;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private int attackLevel;
    private bool isCharging;



    PlayerMovement playerMovement;

    Animator animator;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        coolDownTimer = attackCoolDown;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && coolDownTimer <= 0 && !isCharging)
        {
            isCharging = true;
            animator.SetBool("charge", true);
        }

        if (isCharging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isCharging = false;

                animator.SetBool("charge", false);

                coolDownTimer = attackCoolDown;
                animator.SetTrigger("attack");
            }
        }
        else
        {
            coolDownTimer -= Time.deltaTime;
        }
    }
    
    public void SetAttackLevel(int level)
    {
        attackLevel = level;
    }

    public void Attack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        
        fireballs[getFireball()].transform.position = firePoint.position;
        fireballs[getFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x), attackLevel);
    }

    int getFireball()
    {
        for (int i = 0; i < fireballs.Length; ++i)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
