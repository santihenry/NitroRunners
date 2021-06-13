using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(transform.up * -20,ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
