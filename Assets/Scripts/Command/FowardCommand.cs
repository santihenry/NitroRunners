using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FowardCommand : ICommand
{
    float _accelValue;
    float _accelFactor;

    public void Execute(GameObject obj)
    {
        if (_accelValue < 1)
            _accelValue += _accelFactor; 
        else
            _accelValue = 1;

        obj.GetComponent<CarModel>().Vertical = _accelValue;
    }

    public void Stop(GameObject obj)
    {
        _accelValue = 0;
        obj.GetComponent<CarModel>().Vertical = 0;
    }

    public void Undo(GameObject obj)
    {
        obj.GetComponent<CarModel>().Vertical = -1;
    }

    public void Init(GameObject obj)
    {
        _accelValue = 0;
        _accelFactor = obj.GetComponent<CarModel>().statsData.accelFactor;
    }
}
