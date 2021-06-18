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
            imagen.material.SetFloat("_specialAmount", _car.timeToUlti/100);

    }




}
