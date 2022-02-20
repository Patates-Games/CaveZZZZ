using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnvironmentManager : MonoBehaviour
{
    public List<GameObject> doorPast;
    GameObject[] doorNow;
    GameObject[] doorFuture;
    Timeline timeline;
    TimeController timeController;
    public Sprite emptyDoor;

    void Start()
    {
        timeController = GetComponent<TimeController>();
        GetDoors();
    }

    void GetDoors()
    {
        timeline = GameObject.FindGameObjectWithTag("Script").GetComponent<Timeline>();
        doorNow = new GameObject[doorPast.Count];
        doorFuture = new GameObject[doorPast.Count];

        for (int i = 0; i < doorPast.Count; i++)
        {
            for(int j = 0; j < timeline.gameObjects.Count; j++)
            {
                if(timeline.gameObjects[j] == doorPast[i])
                {
                    doorNow[i] = timeline.gameObjects[j + 1];
                    doorFuture[i] = timeline.gameObjects[j + 2];
                    break;
                }
            }
        }
    }

    public void OpenDoorInTimeline(GameObject door)
    {
        GetDoors();
        int index = FindDoor(door);
        door.GetComponent<BoxCollider2D>().enabled = false;
        door.GetComponent<CapsuleCollider2D>().enabled = false;
        door.GetComponent<SpriteRenderer>().enabled = false;
        door.GetComponent<ShadowCaster2D>().enabled = false;
        if (timeController.time == TimeController.Times.Past)
        {
            doorNow[index].GetComponent<BoxCollider2D>().enabled = false;
            doorNow[index].GetComponent<CapsuleCollider2D>().enabled = false;
            doorNow[index].GetComponent<SpriteRenderer>().enabled = false;
            doorNow[index].GetComponent<ShadowCaster2D>().enabled = false;

            doorFuture[index].GetComponent<BoxCollider2D>().enabled = false;
            doorFuture[index].GetComponent<CapsuleCollider2D>().enabled = false;
            doorFuture[index].GetComponent<SpriteRenderer>().enabled = false;
            doorFuture[index].GetComponent<ShadowCaster2D>().enabled = false;
        }
        if (timeController.time == TimeController.Times.Now)
        {
            doorFuture[index].GetComponent<BoxCollider2D>().enabled = false;
            doorFuture[index].GetComponent<CapsuleCollider2D>().enabled = false;
            doorFuture[index].GetComponent<SpriteRenderer>().enabled = false;
            doorFuture[index].GetComponent<ShadowCaster2D>().enabled = false;
        }
    }

    int FindDoor(GameObject door)
    {
        for (int i = 0; i < doorPast.Count; i++)
        {
            if (timeController.time == TimeController.Times.Past)
            {
                if (ReferenceEquals(doorPast[i], door)) return i;
            }
            if (timeController.time == TimeController.Times.Now)
            {
                if (ReferenceEquals(doorNow[i], door)) return i;
            }
            if (timeController.time == TimeController.Times.Future)
            {
                if (ReferenceEquals(doorFuture[i], door)) return i;
            }
        }
        return -1;
    }
}
