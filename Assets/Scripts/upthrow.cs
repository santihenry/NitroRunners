using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upthrow : MonoBehaviour
{
    // Start is called before the first frame update
    public float impulse;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarControllerV2>())
        {
            var _rb = other.gameObject.GetComponent<CarControllerV2>().rb;
            
            _rb.drag = 0f;
            _rb.angularDrag = 0;
            _rb.AddForce(transform.up*impulse,ForceMode.Impulse);
            Debug.Log("Jump");
        }
    }
}
