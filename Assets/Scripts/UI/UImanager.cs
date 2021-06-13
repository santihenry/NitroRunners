using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UImanager : MonoBehaviour
{
    public CarModel _carModel;
    public GameObject contramano;
    public TMP_Text wrongwayTxt;

    public UImanager(CarModel carModel)
    {
        _carModel = carModel;
    }

    public UImanager SetCarTgt(CarModel carModel)
    {
        _carModel = carModel;
        return this;
    }

    private void Start()
    {
        if(wrongwayTxt == null)
            wrongwayTxt = GameObject.Find(" WrongWayTxt").gameObject.GetComponent<TMP_Text>();   
    }


    private void Update()
    {
        if (_carModel.GetWrongway)
            wrongwayTxt.text = "Wrong way";
        else
            wrongwayTxt.text = "";
    }
}
