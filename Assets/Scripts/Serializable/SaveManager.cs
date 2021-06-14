using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static void SaveNickName(NetManager nickName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/NickName.dll";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        NickNameData nickNameData = new NickNameData(nickName);

        bf.Serialize(fs, nickNameData);
        fs.Close();


    }

    public static NickNameData LoadNickName()
    {
        string filePath = Application.persistentDataPath + "/NickName.dll";
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePath, FileMode.Open);
            NickNameData lifeData = bf.Deserialize(fs) as NickNameData;
            fs.Close();
            return lifeData;
        }
        else
        {
            return null;
        }
    }


}
