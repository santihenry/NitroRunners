using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WayPoints : MonoBehaviour
{
    public Color linecolor;
    [Range(0, 1)] public float SphereRadius;
    public List<Transform> nodes = new List<Transform>();

    RaycastHit hitMedio;
    RaycastHit hitRigth;
    RaycastHit hitLeft;

    public LayerMask floorLayer;
    public LayerMask wallLayer;

    [Range(2,8)]
    public float offset;

    [Range(30, 100)]
    public float width;


    public bool PlacementFloor;


    public bool showWidth;

    private void OnDrawGizmos()
    {      
        Transform[] path = GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();
        for (int i = 1; i < path.Length; i++)
        {
            nodes.Add(path[i]);
        }


        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentWaypoint = nodes[i].position;
            Vector3 previousWaypoint = Vector3.zero;

            if (i != 0) previousWaypoint = nodes[i - 1].position;
            else if (i == 0) previousWaypoint = nodes[nodes.Count - 1].position;


            var backWl = Physics.Raycast(currentWaypoint, Vector3.down, out hitMedio, 10, floorLayer);

            if(PlacementFloor)
                nodes[i].transform.position = hitMedio.point + new Vector3(0,offset,0);

  
            if(i < nodes.Count-1)
                nodes[i].LookAt(nodes[i + 1]);
            nodes[nodes.Count - 1].LookAt(nodes[0]);
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitMedio.point, .5f);

            Gizmos.color = linecolor;
            Gizmos.DrawLine(previousWaypoint, currentWaypoint);
            Gizmos.DrawLine(currentWaypoint, hitMedio.point);
            Gizmos.DrawSphere(currentWaypoint, SphereRadius);

            if (showWidth)
            {
                var rigth = Physics.Raycast(currentWaypoint, nodes[i].right, out hitRigth, 100, wallLayer);
                var left = Physics.Raycast(currentWaypoint, -nodes[i].right, out hitLeft, 100, wallLayer);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(nodes[i].transform.position + nodes[i].right * 10, nodes[i].transform.position - nodes[i].right * 10);

                Gizmos.DrawLine(hitRigth.point, hitLeft.point);
                Gizmos.DrawSphere(hitRigth.point, .5f);
                Gizmos.DrawSphere(hitLeft.point, .5f);

            }
            
        }
    }


  

}
