using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flaregun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
