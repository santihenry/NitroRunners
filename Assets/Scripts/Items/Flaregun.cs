using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flaregun : MonoBehaviour
{
    public GameObject flare;
    public GameObject flaregun;
    public void SetActive(int b)
    {
        bool newb = System.Convert.ToBoolean(b);
        flaregun.SetActive(newb);

    }
    public void ShootFlare()
    {
        Instantiate(flare, flaregun.transform.position, flaregun.transform.rotation);
    }

}
