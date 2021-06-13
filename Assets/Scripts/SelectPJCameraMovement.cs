using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPJCameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Quaternion rotate = transform.rotation;
            rotate.eulerAngles -= new Vector3(0, Input.GetAxis("Mouse X"), 0)*2;
            transform.rotation = rotate;
        }
        
    }
}
