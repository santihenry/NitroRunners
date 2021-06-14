using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPJCameraMovement : MonoBehaviour
{

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
