using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpect : MonoBehaviour
{
    public float speed;
    private WayPoints _waypoints;
    private Transform _currentWaypoint;
    public List<Transform> _waypointsList = new List<Transform>();

    int curr;

    public Vector3 offset;


    void Start()
    {
        _waypoints = FindObjectOfType<WayPoints>();
        _currentWaypoint = transform;
        if (_waypoints != null)
            _waypointsList = _waypoints.nodes;
    }

   
    void Update()
    {
        /*
        float dist = Vector3.Distance(transform.position + offset, _waypointsList[curr].position + offset);

        transform.position += (_waypointsList[curr].position + offset - transform.position + offset).normalized * speed * Time.deltaTime;
        transform.LookAt(_waypointsList[curr+1].position);

        if (dist < 1)
            curr++;
        if (curr >= _waypointsList.Count)
            curr = 0;
        */
        

    }



}
