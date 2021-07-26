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




    public static void SaveSettings(Settings setting)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/Settings.dll";
        FileStream fs = new FileStream(filePath, FileMode.Create);

        SettingsData settingData = new SettingsData(setting);

        bf.Serialize(fs, settingData);
        fs.Close();
    }


    public static SettingsData LoadSettings()
    {
        string filepath = Application.persistentDataPath + "/Settings.dll";
        if (File.Exists(filepath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filepath, FileMode.Open);
            SettingsData settingData = bf.Deserialize(fs) as SettingsData;
            fs.Close();
            return settingData;
        }
        else
        {
            return null;
        }


    }



}
