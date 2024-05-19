using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;

    [SerializeField] private float startDelay;

    [SerializeField] private Transform firePoint;

    [SerializeField] private GameObject[] fireballs;

    [SerializeField] private AudioClip sound;



    private float cooldownTimer;

    private void OnDisable()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].SetActive(false);  
        }
    }

    private void OnEnable()
    {
        cooldownTimer = -startDelay;

    }

    private void Start()
    {
        cooldownTimer = -startDelay;
    }
    private void Attack()
    {
        SoundManager.instance.PlaySound(sound);
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
        {
            Attack();
        }
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
