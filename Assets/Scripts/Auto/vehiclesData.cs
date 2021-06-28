using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "VehicleData", menuName = "Vehicle")]
public class vehiclesData : ScriptableObject
{
    [Header("Engine")]
    public float accelFactor;
    public float maxPower;
    public float starPower;
    public float driftPower;
    public float increacePower;



    [Header("Direction")]
    public float trurnFactor;
    public float turnStrength;
    public float driftStrength;


    [Header("Info")]
    public Sprite pjImg;
    public VideoClip clip;
    public Texture2D UltiTex;
    public string _name;
    public string Desctiption;


}
