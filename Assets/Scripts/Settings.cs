using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System;

public class Settings : MonoBehaviour
{
    public AudioMixer mixer;

    int _musicLvl;
    int _effectLvl;
    int _uiLvl;
    int _resolutionIndex;
    int _resolutionModeIndex;
    Vector2 _resolucion;
    bool _fullScreen;

    public Toggle fullScreen;
    public TMP_Dropdown resolucionesDropDown;
    public TMP_Dropdown resolucionesModeDropDown;

    public Slider musicSlider;
    public Slider uiSlider;
    public Slider effectSlider;

    public TMP_Text musicValue;
    public TMP_Text uiValue;
    public TMP_Text effectValue;

    public List<string> r = new List<string>();
    public List<string> resolutionMods = new List<string>();
    public List<Tuple<FullScreenMode, string>> rm = new List<Tuple<FullScreenMode, string>>();


    private void Start()
    {
        resolucionesDropDown.ClearOptions();
        foreach (var resolution in Screen.resolutions)
        {
            r.Add($"{resolution.width}x{resolution.height}");
        }

        resolucionesDropDown.AddOptions(r);

        rm.Add(new Tuple<FullScreenMode, string>(FullScreenMode.ExclusiveFullScreen, "ExclusiveFullScreen"));
        rm.Add(new Tuple<FullScreenMode, string>(FullScreenMode.FullScreenWindow, "FullScreenWindow"));
        rm.Add(new Tuple<FullScreenMode, string>(FullScreenMode.MaximizedWindow, "MaximizedWindow"));
        rm.Add(new Tuple<FullScreenMode, string>(FullScreenMode.Windowed, "Windowed"));

        foreach (var item in rm)
        {
            resolutionMods.Add($"{item.Item2}");
        }

        resolucionesModeDropDown.AddOptions(resolutionMods);
        
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
            return _resolutionIndex;
        }
    }

    public int ResolutionMode
    {
        get
        {
            return _resolutionModeIndex;
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
        _resolutionIndex = resolucionesDropDown.value;
        _resolucion = new Vector2(Screen.resolutions[resolucionesDropDown.value].width, Screen.resolutions[resolucionesDropDown.value].height);
        _resolutionModeIndex = resolucionesModeDropDown.value;
        Screen.fullScreenMode = rm[resolucionesModeDropDown.value].Item1;
        Screen.SetResolution((int)_resolucion.x, (int)_resolucion.y, _fullScreen);
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
        effectSlider.value = _effectLvl;
        musicSlider.value = _musicLvl;
        uiSlider.value = _uiLvl;
        mixer.SetFloat("Music", _musicLvl);
        mixer.SetFloat("SoundEffects", _effectLvl);
        mixer.SetFloat("UiEffects", _uiLvl);

        fullScreen.isOn = settingData.FullScreen;
        _fullScreen = fullScreen.isOn;

        _resolutionIndex = settingData.Resolution;
        _resolutionModeIndex = settingData.ResolutionMode;
        _resolucion = settingData.Resolucion.ToVector2();
        resolucionesDropDown.value = settingData.Resolution;
        resolucionesModeDropDown.value = settingData.ResolutionMode;

        if(_resolucion !=Vector2.zero)
            Screen.SetResolution((int)_resolucion.x, (int)_resolucion.y, _fullScreen);
        else
            Screen.SetResolution(1920,1080,true);

        Screen.fullScreenMode = rm[_resolutionModeIndex].Item1;


        
    }
}
