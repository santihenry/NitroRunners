using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class UltiUI : MonoBehaviour
{
    
    public Texture2D img;

    Image imagen;


    CarModel _car;
    float _timeMax=200;
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

    
    void Update()
    {
        if (img == null)
        {
            foreach (var item in FindObjectsOfType<CarModel>())
            {
                if (item.nickName == PhotonNetwork.NickName)
                {
                    _car = item;
                    if (_car.statsData._name != "rayer")
                        img = item.statsData.UltiTex;
                    else
                        imagen.gameObject.SetActive(false);
                }
            }

            if (_car.statsData._name != "rayer")
                Shader.SetGlobalTexture("_specialTex", img);
        }

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
