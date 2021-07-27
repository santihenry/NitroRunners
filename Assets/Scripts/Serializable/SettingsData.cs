using System;
using UnityEngine;
using UnityEngine.Audio;


[Serializable]
public class SettingsData
{
    int valueMusic;
    int valueEffect;
    int valueUi;
    int resolution;
    int resolutionMode;
    SerializableVector2 resolucion;
    bool fullScreen;


    public int Resolution
    {
        get
        {
            return resolution;
        }
    }

    public int ResolutionMode
    {
        get
        {
            return resolutionMode;
        }
    }

    public int MusicValue
    {
        get
        {
            return valueMusic;
        }
    }

    public int EffectValue
    {
        get
        {
            return valueEffect;
        }
    }

    public int UiValue
    {
        get
        {
            return valueUi;
        }
    }


    public SerializableVector2 Resolucion
    {
        get
        {
            return resolucion;
        }
    }

    public bool FullScreen
    {
        get
        {
            return fullScreen;
        }
    }

    public SettingsData(Settings settingsD)
    {
        valueMusic = settingsD.Music;
        valueEffect = settingsD.Effect;
        valueUi = settingsD.Ui;
        resolucion = new SerializableVector2(settingsD.Resolucion);
        fullScreen = settingsD.FullScreen;
        resolution = settingsD.Resolution;
        resolutionMode = settingsD.ResolutionMode;
    }

}


[Serializable]
public class SerializableVector2
{

    public float x;
    public float y;

    public SerializableVector2(Vector2 vector2)
    {
        x = vector2.x;
        y = vector2.y;
    }

    public Vector2 ToVector2()
    {
        return new Vector3(x, y);
    }
}

