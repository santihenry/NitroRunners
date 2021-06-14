using System;
using UnityEngine;

[Serializable]
public class NickNameData 
{
    string nick;

    public string Nick
    {
        get
        {
            return nick;
        }
    }


    public NickNameData(NetManager nM)
    {
        nick = nM.username.text.ToString();
    }

}
