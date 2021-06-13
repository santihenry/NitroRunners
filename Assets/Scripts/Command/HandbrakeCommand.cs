using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandbrakeCommand : ICommand
{
    public void Execute(GameObject obj)
    {
        obj.GetComponent<CarModel>().Handbracke = true;
    }


    public void Stop(GameObject obj)
    {
        obj.GetComponent<CarModel>().Handbracke = false;
    }

    public void Undo(GameObject obj)
    {
         obj.GetComponent<CarModel>().Handbracke = false;
    }

    public void Init(GameObject obj)
    {
        throw new System.NotImplementedException();
    }
}
