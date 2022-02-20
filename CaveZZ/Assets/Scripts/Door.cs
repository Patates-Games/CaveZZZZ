using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        Kitchen,
        Coridor,
        Bedroom
    }
    public DoorType doorType;
}
