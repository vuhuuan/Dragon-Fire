using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;

    [Header("SFX")]
    [SerializeField] private AudioClip sound;

    private Animator anim;
    private SpriteRenderer spriteRend;


    private bool triggered; // when the trap has been triggered
    private bool active; // when the trap is active and can hurt player
    Health playerHealth = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();
            if (!triggered)
            {
                // trigger
                StartCoroutine(ActivateFireTrap());
            }
            if (active)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
            }
        }        
    }


    private IEnumerator ActivateFireTrap()
    {
        triggered = true;
        spriteRend.color = Color.red; // turn to red to notify player


        // wait for delay, activate the trap, turn on animation, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(sound);
        spriteRend.color = Color.white; 
        active = true;
        anim.SetBool("activated", true);

        // wait X seconds, deactivate the trap and reset all variables and animator;
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }

    private void Update()
    {
        if (playerHealth && active)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = null;
        }
    }
}
