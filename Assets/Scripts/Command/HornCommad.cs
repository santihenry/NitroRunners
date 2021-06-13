using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornCommand : ICommand
{
    public void Execute(GameObject obj)
    {
        obj.GetComponent<CarController>().Horn();
    }

    public void Stop(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    public void Undo(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    public void Init(GameObject obj)
    {
        throw new System.NotImplementedException();
    }
}
