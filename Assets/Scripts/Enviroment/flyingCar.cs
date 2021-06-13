using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingCar : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    Vector3 startPosition;
    FlyingCarsSpawner _owner;
    void Start()
    {

    }

    // Update is called once per frame
    public flyingCar SetOwner(FlyingCarsSpawner owner)
    {
        _owner = owner;
        return this;
    }

    public void  SetColor(Color color)
    {
        GetComponent<Renderer>().material.SetColor("Albedo",color);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    public static void TurnOff(flyingCar car)
    {
        car.transform.position = car.startPosition;
        car.gameObject.SetActive(false);
    }
    public static void TurnOn(flyingCar car)
    {
        car.startPosition = car.transform.position;
        car.gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FlyingCarsSpawner>())
        {
            _owner.Recycle(this);
        }
    }
}
