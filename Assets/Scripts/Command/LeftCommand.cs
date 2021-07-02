using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCommand : ICommand
{
    float _turnValue;
    float _turnlFactor;

    public void Execute(GameObject obj)
    {
        if (_turnValue < 1)
            _turnValue += _turnlFactor;
        else
            _turnValue = 1;

        obj.GetComponent<CarModel>().Horizontal = -_turnValue;
    }


    public void Stop(GameObject obj)
    {
        obj.GetComponent<CarModel>().Horizontal = 0;
    }

    public void Undo(GameObject obj)
    {
        obj.GetComponent<CarModel>().Horizontal = 1;
    }

    public void Init(GameObject obj)
    {
        _turnValue = 0;
        _turnlFactor = obj.GetComponent<CarModel>().statsData.trurnFactor;
    }
}
