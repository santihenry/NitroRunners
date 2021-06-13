using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPoint : MonoBehaviour
{


    public int id;
    public bool isStartLine;
    TMP_Text checkPointNumeTxt;

    void Start()
    {
        checkPointNumeTxt = GetComponentInChildren<TMP_Text>();
        if(id != 0 && checkPointNumeTxt != null)
            checkPointNumeTxt.text = id.ToString();
    }

   
    void Update()
    {
        
    }
}
