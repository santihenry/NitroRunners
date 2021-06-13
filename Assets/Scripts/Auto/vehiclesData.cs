using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "VehicleData", menuName = "Vehicle")]
public class vehiclesData : ScriptableObject
{
    public float accelFactor;
    public float maxSpeed;
    public float trurnFactor;
    public float turnStrength;
    public float driftStrength;
    public Sprite pjImg;
    public string _name;
    public string Desctiption;
    public VideoClip clip;
}
