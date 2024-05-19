using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //lock player movement
            collision.GetComponent<PlayerMovement>().enabled = false;
            collision.GetComponent<PlayerAttack>().enabled = false;

            collision.GetComponent<Animator>().Play("Win");
            //player win animation
            StartCoroutine(WinTrigger());
        }
    }

    IEnumerator WinTrigger()
    {
        yield return new WaitForSeconds(1f);
        WinScreen.SetActive(true);
    }
}
