using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;


public class Settings : MonoBehaviour
{
    public AudioMixer mixer;

    int _musicLvl;
    int _effectLvl;
    int _uiLvl;
    int _resolution;
    Vector2 _resolucion;
    bool _fullScreen;

    public Toggle fullScreen;
    public TMP_Dropdown resoluciones;

    public Slider musicSlider;
    public Slider uiSlider;
    public Slider effectSlider;

    public TMP_Text musicValue;
    public TMP_Text uiValue;
    public TMP_Text effectValue;



    private void Start()
    {
        LoadSettings();
    }



    public int Music
    {
        get
        {
            return _musicLvl;
        }
    }

    public int Effect
    {
        get
        {
            return _effectLvl;
        }
    }
    public int Ui
    {
        get
        {
            return _uiLvl;
        }
    }

    public int Resolution
    {
        get
        {
            return _resolution;
        }
    }

    public Vector2 Resolucion
    {
        get
        {
            return _resolucion;
        }
    }

    public bool FullScreen
    {
        get
        {
            return _fullScreen;
        }
    }

    private void Update()
    {
        musicValue.text = $"{musicSlider.value + 80}";
        effectValue.text = $"{effectSlider.value + 80}";
        uiValue.text = $"{uiSlider.value + 80}";
       
    }


    public void SetLvlMusic(float nMusic)
    {
        mixer.SetFloat("Music", nMusic);
        _musicLvl = (int)musicSlider.value;
        SaveSettings();
    }
    public void SetLvlEffect(float nEffect)
    {
        mixer.SetFloat("SoundEffects", nEffect);
        _effectLvl = (int)effectSlider.value;
        SaveSettings();
    }

    public void SetLvlUi(float nUi)
    {
        mixer.SetFloat("UiEffects", nUi);
        _uiLvl = (int)uiSlider.value;
        SaveSettings();
    }


    public void SetResolution()
    {
        _fullScreen = fullScreen.isOn;
        _resolution = resoluciones.value;
        SaveSettings();
    }


    public void SaveSettings()
    {
        SaveManager.SaveSettings(this);
    }

    public void LoadSettings()
    {
        SettingsData settingData = SaveManager.LoadSettings();
        _musicLvl = settingData.MusicValue;
        _effectLvl = settingData.EffectValue;
        _uiLvl = settingData.UiValue;
        mixer.SetFloat("Music", _musicLvl);
        mixer.SetFloat("SoundEffects", _effectLvl);
        mixer.SetFloat("UiEffects", _uiLvl);
        effectSlider.value = settingData.EffectValue;
        musicSlider.value = settingData.MusicValue;
        uiSlider.value = settingData.UiValue;
        _resolution = settingData.Resolution;
        _resolucion = settingData.Resolucion.ToVector2();
        resoluciones.value = _resolution;
        Screen.SetResolution((int)_resolucion.x, (int)_resolucion.y, _fullScreen);
        fullScreen.isOn = _fullScreen;

    }
}
