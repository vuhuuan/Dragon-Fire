using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    public float startingHealth;
    public float currentHealth { get; private set; }

    public EnemyHealthBar enemyHealthBar;

    public static SoundManager soundManager;

    public float bottomBound = -5f  ;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    private bool invincible = false;

    [Header("Components")]
    [SerializeField] private Behaviour[] behaviorScripts;

    [Header("Sound")]
    [SerializeField] private AudioClip deadSound;
    [SerializeField] private AudioClip hurtSound;

    

    private Animator anim;
    private bool dead;
    public bool Dead => dead;
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();



    } // init

    private void Start()
    {
        if (enemyHealthBar)
        {
            enemyHealthBar.SetMaxHealth(startingHealth);
        }
    }

    public void TakeDamage(float _damage)
    {
        if (invincible) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        // for enemy;
        if (enemyHealthBar)
        {
            enemyHealthBar.UpdateHealthBar(currentHealth);
        }

        if (currentHealth > 0)
        {
            SoundManager.instance.PlaySound(hurtSound);
            anim.SetTrigger("hurt");
            StartCoroutine(Invincibility());
        }
        else
        {
            if (!dead)
            {
                dead = true;

                // Player
                if (gameObject.name.Contains("Player"))
                {
                    GetComponent<PlayerMovement>().Die();
                }

                foreach (var behavior in behaviorScripts)
                {
                    if (behavior)
                    {
                        behavior.enabled = false;
                    }
                }

                if (anim)
                {
                    anim.SetBool("grounded", true);
                    anim.SetBool("wall", false);

                    anim.SetTrigger("die");
                } else
                {
                    Deactivate();
                }

                SoundManager.instance.PlaySound(deadSound);
            }
        }
    } // call Invincible here

    public void AddHeath(float _value)
    {
        currentHealth = Math.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private void Update()
    {
        if (transform.position.y < bottomBound)
        {
            TakeDamage(startingHealth);
        }
    }

    private IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        invincible = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void ResetPlayer()
    {
        dead = false;

        AddHeath(startingHealth); // currentHealth = startingHealth is also good
        anim.SetBool("charge", false);
        anim.ResetTrigger("die");
        anim.Play("Idle");

        StartCoroutine(Invincibility());


        foreach (var behavior in behaviorScripts)
        {
            if (behavior)
            {
                behavior.enabled = true;
            }
        }

    }
}
