using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;

    [SerializeField] private CameraController cam;
    [SerializeField] bool isVertical;
    [SerializeField] bool isSameRoom;



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isVertical)
            {
                if (collision.transform.position.x > transform.position.x)
                {
                    cam.MoveToNewRoom(nextRoom);

                    if (!isSameRoom)
                    {
                        previousRoom.gameObject.GetComponent<Room>().ActivateRoom(false);
                        nextRoom.gameObject.GetComponent<Room>().ActivateRoom(true);
                    }
                }
                else
                {
                    cam.MoveToNewRoom(previousRoom);

                    if (!isSameRoom)
                    {
                        previousRoom.gameObject.GetComponent<Room>().ActivateRoom(true);
                        nextRoom.gameObject.GetComponent<Room>().ActivateRoom(false);
                    }
                }
            } else
            {
                if (collision.transform.position.y > transform.position.y )
                {
                    cam.MoveToNewRoom(nextRoom);

                    if (!isSameRoom)
                    {
                        previousRoom.gameObject.GetComponent<Room>().ActivateRoom(false);
                        nextRoom.gameObject.GetComponent<Room>().ActivateRoom(true);
                    }
                }
                else if (collision.transform.position.y < transform.position.y)
                {
                    cam.MoveToNewRoom(previousRoom);

                    if (!isSameRoom)
                    {
                        previousRoom.gameObject.GetComponent<Room>().ActivateRoom(true);
                        nextRoom.gameObject.GetComponent<Room>().ActivateRoom(false);
                    }
                }
            }
        }
    }
}
