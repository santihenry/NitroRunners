using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class UltiUI : MonoBehaviour
{
    
    public Texture2D img;

    Image imagen;


    CarModel _car;
    float _timeMax=45;
    float _fire;
    bool _activated;

    private void Awake()
    {
        imagen = GetComponent<Image>();    
    }


    void Start()
    {
        imagen.material.SetFloat("_specialAmount", 0);
    }


    private void OnDisable()
    {
        if (img == null)
        {
            Debug.LogWarning(_car);
            _car = GameManager.Instance.MyCar;
            Debug.LogWarning(_car);
            if (_car.statsData.name != "rayer")
                img = _car.statsData.UltiTex;
            else
                gameObject.SetActive(false);

            if (_car.statsData._name != "rayer")
                Shader.SetGlobalTexture("_specialTex", img);
            else
                Destroy(gameObject);
        }
    }


    void Update()
    {
        /*
         if (img == null)
        {
            Debug.LogWarning(_car);
            _car = GameManager.Instance.MyCar;
            Debug.LogWarning(_car);
            if (_car.statsData.name != "rayer")
                img = _car.statsData.UltiTex;
            else
                gameObject.SetActive(false);

            if (_car.statsData._name != "rayer")
                Shader.SetGlobalTexture("_specialTex", img);
            else
                Destroy(gameObject);
        }
        */


        if (_car.statsData._name != "rayer") 
        {
            if (_car.timeToUlti == _timeMax &&_fire<=1)
            {
                _activated = true;
            }
            else if(_fire>1)
            {
                _activated = false;
            }
            if(_car.timeToUlti != _timeMax)
            {
                _fire = 0;
            }
            if (_activated && _fire <=1)
            {
                _fire += Time.deltaTime*1.5f;

            }
           

            Shader.SetGlobalFloat("Size", _fire);
            imagen.material.SetFloat("_specialAmount", _car.timeToUlti/_timeMax);
        } 

    }




}
