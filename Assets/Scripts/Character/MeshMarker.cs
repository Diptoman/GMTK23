using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMarker : MonoBehaviour
{
    public ClassFlag classFlag;

    [System.Flags]
    public enum ClassFlag
    {
        Warrior = 1 << 1,
        Mage = 1 << 2,
        Support = 1 << 3,
        Rogue = 1 << 4
    }
}
