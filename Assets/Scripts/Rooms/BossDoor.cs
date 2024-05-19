using System.Collections;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public MonoBehaviour[] playerMovementScripts;
    private BoxCollider2D doorCollider;
    private Animator doorAnimator;

    public Transform player;
    public float moveTime = 0.5f;
    public float doorCloseDelay = 0.5f;

    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        doorAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HandleDoorTransition());
        }
    }

    private IEnumerator HandleDoorTransition()
    {
        foreach (var script in playerMovementScripts)
        {
            script.enabled = false;
        }

        Vector3 startPosition = player.position;
        Vector3 endPosition = startPosition;
        endPosition.x += moveTime;

        float elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            player.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.position = endPosition;

        doorAnimator.SetTrigger("close");

        yield return new WaitForSeconds(doorCloseDelay);

        doorCollider.isTrigger = false;

        yield return new WaitForSeconds(doorCloseDelay);

        foreach (var script in playerMovementScripts)
        {
            script.enabled = true;
        }
    }
}
