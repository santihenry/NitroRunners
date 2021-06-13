using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    int _currentWaypoint;
    public float speed;
    float _time;
    public List<Transform> waypoints;
    public float delay;
    float _frecuency;
    float _amplitude;
    Rigidbody _rb;
    float _sintime;
    public LayerMask road;
    
    void Start()
    {
        speed = 0;
        _rb = GetComponent<Rigidbody>();
        GetComponentInChildren<Animator>().SetFloat("MoveVertical", 1);
    }


    void Update()
    { _sintime += Time.deltaTime;
        if (speed == 0)
        {
            _time += Time.deltaTime;
            if (_time >= delay)
            {
                speed = Random.Range(9000,15000);
                _frecuency= Random.Range(0,4 ); 
                _amplitude= Random.Range(2000, 5000); ; ;
                _time = 0;
            }
        }
        if (Vector3.Distance(transform.position, waypoints[_currentWaypoint].position) <30)
        {
            _currentWaypoint += 1;
            _time = 0;
        }
        if (_currentWaypoint > waypoints.Count - 1)
        {
            StartCoroutine(Wait(5));
            _currentWaypoint = 0;
            transform.position = waypoints[0].position;
        }
            Mathf.Clamp01(_time += Time.deltaTime);
         //transform.position -= (transform.position - waypoints[_currentWaypoint].position).normalized*speed*Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward,
            waypoints[_currentWaypoint < waypoints.Count - 1 ? _currentWaypoint + 1 : _currentWaypoint].position - transform.position, _time);
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position+transform.up*5, transform.up*-10, out hit,1,road))
        {
            transform.up = transform.position+hit.point;
          
        }
        else
        {
            _rb.AddForce(transform.up * -900);
        }
    }
    private void FixedUpdate()
    {
        
        _rb.AddForce((waypoints[_currentWaypoint].position - transform.position).normalized * speed);
        _rb.AddForce(transform.right * (Mathf.Sin(_sintime * _frecuency) * _amplitude));
    }
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
       
    }
  
}
