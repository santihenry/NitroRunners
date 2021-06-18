using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class UltiUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture2D img;
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (img == null)
        {

            foreach (var item in FindObjectsOfType<CarModel>())
            {
                if (item.nickName == PhotonNetwork.NickName)
                {
                    img = item.statsData.UltiTex;
                }
            }
            Shader.SetGlobalTexture("_specialTex", img);
        }
    }
}
