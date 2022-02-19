using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public TimeController timeController;
    TimeController.Times time;

    public List<GameObject> gameObjects = new List<GameObject>();
    public List<TimeController.Times> times = new List<TimeController.Times>();

    void Start()
    {
        ObjectManager();
    }

    public void ObjectManager()
    {
        time = timeController.time;

        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (times[i] == time) gameObjects[i].SetActive(true);
            else gameObjects[i].SetActive(false);
        }
    }
}
