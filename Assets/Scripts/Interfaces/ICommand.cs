using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand 
{
    void Execute(GameObject obj);
    void Undo(GameObject obj);
    void Stop(GameObject obj);
    void Init(GameObject obj);
}
