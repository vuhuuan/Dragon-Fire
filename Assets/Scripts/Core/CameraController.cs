using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private float nextPosX;
    private float nextPosY;


    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
      
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(nextPosX, nextPosY, transform.position.z),
          ref velocity, speed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        nextPosX = _newRoom.position.x;
        nextPosY = _newRoom.position.y;
    }
}
