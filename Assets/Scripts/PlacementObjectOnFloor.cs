using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class PlacementObjectOnFloor : MonoBehaviour
{
    [Range(0, 10)]
    public float offset;
    public LayerMask floorLayer;
    RaycastHit hit;



    private void Update()
    {
        var backWl = Physics.Raycast(transform.position, Vector3.down, out hit, 10, floorLayer);

        transform.position = hit.point + new Vector3(0, offset, 0);
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, hit.point);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hit.point,.2f);
    }

}
