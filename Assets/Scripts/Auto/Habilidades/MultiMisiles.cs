using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;



public class MultiMisiles : MonoBehaviourPun
{
    public float radius;
    public int cant;


    List<Transform> _waypointsList = new List<Transform>();
    List<Transform> _orderList = new List<Transform>();
    List<Vector3> _locationes = new List<Vector3>();
    CarModel _car;
    List<int> _numbers;
    public LayerMask floorLayer;
    RaycastHit hitMedio;
    
    public string misilesPath;

    public float timeCountdown;
    float currentTime;
    bool canShoot;

    public MultiMisiles(CarModel car,List<Transform> waypointsList)
    {
        _car = car;
        _waypointsList = waypointsList;
        _orderList.AddRange(_waypointsList);
    }

    public MultiMisiles SetCar(CarModel car)
    {
        _car = car;
        return this;
    }

    public MultiMisiles SetWaypointsList(List<Transform> waypointsList)
    {
        _waypointsList = waypointsList;
        return this;
    }



    void Start()
    {
        if (!photonView.IsMine) return;

       _car = GetComponent<CarModel>();

    }


   

    private void Update()
    {
        if (!photonView.IsMine) return;

        currentTime += Time.deltaTime;

        if (timeCountdown - currentTime <= 0)
        {
            canShoot = true;
            currentTime = 0;
        }
    }
   
    private IEnumerable<Transform> SortWaypoints()
    {
        var primeros = _orderList.Where(n => n.GetSiblingIndex() >= _car.currentWay);
        var ultis = _orderList.Where(n => n.GetSiblingIndex() < _car.currentWay);
        var resul = primeros.Concat(ultis);

        return resul;
    }


    public void Shoot()
    {
        if (!canShoot) return;
        currentTime = 0;

        _waypointsList = _car._waypointsList;
        _orderList = _waypointsList;
        _locationes = new List<Vector3>();


        var ways = SortWaypoints();
        _orderList = new List<Transform>();
        _orderList.AddRange(ways);

        _numbers = new List<int>();
        for (int i = 0; i < _waypointsList.Count; i++)
        {
            _numbers.Add(i);
        }

        if (_locationes.Count == 0)
        {
            for (int i = 0; i < cant; i++)
            {
                var rnd = Random.Range(0, _numbers.Count);
                _locationes.Add(_waypointsList[_numbers[rnd]].position + _waypointsList[_numbers[rnd]].right * Random.Range(-10, 10));
                _numbers.RemoveRange(rnd, 2);
            }

            foreach (Vector3 location in _locationes)
            {
                var backWl = Physics.Raycast(location, Vector3.down, out hitMedio, 100, floorLayer);
                PhotonNetwork.Instantiate(misilesPath, location, Quaternion.identity).GetComponent<Misiles>().SetInitPos(location + new Vector3(0,200,0));
            }

        }
    }

}
