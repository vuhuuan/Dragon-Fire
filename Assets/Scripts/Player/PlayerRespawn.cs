using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{   
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Room currentCheckpointRoom;

    private Health playerHealth;

    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void Respawn()
    {
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }

        transform.position = currentCheckpoint.position;

        playerHealth.ResetPlayer();

        currentCheckpointRoom.ActivateRoom(false);
        currentCheckpointRoom.ActivateRoom(true);
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpointRoom.transform);
    }

    // Activate checkpoint

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint" && !gameObject.GetComponent<Health>().Dead)
        {
            currentCheckpoint = collision.transform;
            currentCheckpointRoom = collision.GetComponentInParent<Room>();
            SoundManager.instance.PlaySound(checkpointSound);

            collision.GetComponent<Collider2D>().enabled = false;

            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
