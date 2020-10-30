using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdminLegs", menuName = "AdminLegs")]
public class AdminLegs : ScriptableObject
{
    [Header("Settings leg")]
    public float timeWaitShowPartLeg = 0;
    public float sizePartLeg = 1;
    public GameObject prefPartLeg = null;
}
