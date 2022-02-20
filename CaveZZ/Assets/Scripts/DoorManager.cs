using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool[] doorOpened = new bool[3];

    public void OpenDoor(Door.DoorType doorType)
    {
        if (doorType == Door.DoorType.Kitchen)
        {
            doorOpened[0] = true;
        }
        else if (doorType == Door.DoorType.Coridor)
        {
            doorOpened[1] = true;
        }
        else if (doorType == Door.DoorType.Bedroom)
        {
            doorOpened[2] = true;
        }
    }

    public bool IsUnlockedBefore(Door.DoorType doorType)
    {
        if (doorType == Door.DoorType.Kitchen)
        {
            return doorOpened[0];
        }
        else if (doorType == Door.DoorType.Coridor)
        {
            return doorOpened[1];
        }
        else if (doorType == Door.DoorType.Bedroom)
        {
            return doorOpened[2];
        }
        else return false;
    }
}
