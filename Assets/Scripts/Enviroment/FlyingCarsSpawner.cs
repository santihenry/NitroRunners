using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCarsSpawner : MonoBehaviour
{
    //public flyingCar car;
    ObjPool<flyingCar> _pool;
    public float offset;
    public List<flyingCar> cars = new List<flyingCar>();



    float time;
    float currentTime;

    [Range(.01f, 1)] public float minDistTime;
    [Range(1, 3)] public float maxDisTime;


    public bool showGizmos;


    void Start()
    {       
        _pool = new ObjPool<flyingCar>(Factory, flyingCar.TurnOn, flyingCar.TurnOff, 0, true);
        time = Random.Range(minDistTime, maxDisTime);
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if(time - currentTime <= 0)
        {
            _pool.GetObj();
            currentTime = 0;
            time = Random.Range(minDistTime, maxDisTime);
        }
       
    }
    public flyingCar Factory()
    {
        return Instantiate(cars[Random.Range(0,cars.Count)],transform.position+transform.right*Random.Range(-offset,offset),transform.rotation).SetOwner(this);                                                                                                             
    }
    public void Recycle(flyingCar car)
    {
        _pool.Recycle(car);
    }
    private void OnDrawGizmos()
    {
        if (showGizmos){
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position ,transform.position+transform.forward*800);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + transform.right * offset ,transform.position+transform.forward*800 + transform.right * offset);
            Gizmos.DrawLine(transform.position + transform.right * -offset, transform.position+transform.forward*800 + transform.right * -offset);
        }
       
    }
}
