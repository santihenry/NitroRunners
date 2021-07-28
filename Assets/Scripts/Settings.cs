using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

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

    public Toggle fullScreenTogle;
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


    bool load;


    private void Awake()
    {
        DontDestroyOnLoad(this);       
    }


    
    private void Start()
    {


        foreach (var resolution in Screen.resolutions)
        {
            r.Add($"{resolution.width}x{resolution.height}");
        }

        rm.Add(new Tuple<FullScreenMode, string>(FullScreenMode.ExclusiveFullScreen, "ExclusiveFullScreen"));
        rm.Add(new Tuple<FullScreenMode, string>(FullScreenMode.FullScreenWindow, "FullScreenWindow"));
        rm.Add(new Tuple<FullScreenMode, string>(FullScreenMode.MaximizedWindow, "MaximizedWindow"));
        rm.Add(new Tuple<FullScreenMode, string>(FullScreenMode.Windowed, "Windowed"));

        foreach (var item in rm)
        {
            resolutionMods.Add($"{item.Item2}");
        }
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
        foreach (var item in FindObjectsOfType<Settings>())
        {
            if (item != this) Destroy(item.gameObject);
        }


        if (SceneManager.GetActiveScene().name == "Settings")
        {
            if (resolucionesDropDown == null)
            {
                resolucionesDropDown = GameObject.Find("ResolutionsDropdown").GetComponent<TMP_Dropdown>();
                resolucionesModeDropDown = GameObject.Find("ScreenModeDropdown").GetComponent<TMP_Dropdown>();
                musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
                effectSlider = GameObject.Find("EffectsSlider").GetComponent<Slider>();
                uiSlider = GameObject.Find("UiSlider").GetComponent<Slider>();
                musicValue = GameObject.Find("MusicValue").GetComponent<TextMeshProUGUI>();
                effectValue = GameObject.Find("EffectsValue").GetComponent<TextMeshProUGUI>();
                uiValue = GameObject.Find("UiValue").GetComponent<TextMeshProUGUI>();
                fullScreenTogle = GameObject.Find("FullScreenToggle").GetComponent<Toggle>();
            }

            if (!load)
            {
                fullScreenTogle.isOn = FullScreen;
                musicSlider.value = _musicLvl;
                effectSlider.value = _effectLvl;
                uiSlider.value = _uiLvl;

                effectSlider.onValueChanged.AddListener(SetLvlEffect);
                musicSlider.onValueChanged.AddListener(SetLvlMusic);
                uiSlider.onValueChanged.AddListener(SetLvlUi);
                resolucionesDropDown.onValueChanged.AddListener(SetResolution);
                resolucionesModeDropDown.onValueChanged.AddListener(SetResolutionMode);
                fullScreenTogle.onValueChanged.AddListener(SetResolution);

                resolucionesDropDown.AddOptions(r);
                resolucionesModeDropDown.AddOptions(resolutionMods);


                resolucionesDropDown.value = _resolutionIndex;
                resolucionesModeDropDown.value = _resolutionModeIndex;


                load = true;
            }

            musicValue.text = $"{_musicLvl + 80}";
            effectValue.text = $"{_effectLvl + 80}";
            uiValue.text = $"{_uiLvl + 80}";

        }
    }

    private void OnLevelWasLoaded(int level)
    {
        load = false;
        LoadSettings();       
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


    public void SetResolution(int value)
    {           
        _fullScreen = fullScreenTogle.isOn;
        _resolutionIndex = resolucionesDropDown.value;
        _resolucion = new Vector2(Screen.resolutions[resolucionesDropDown.value].width, Screen.resolutions[resolucionesDropDown.value].height);
        Screen.SetResolution((int)_resolucion.x, (int)_resolucion.y, _fullScreen);
        SaveSettings();       
    }

    public void SetResolutionMode(int value)
    {       
        _resolutionModeIndex = resolucionesModeDropDown.value;
        Screen.fullScreenMode = rm[resolucionesModeDropDown.value].Item1;
        SaveSettings();      
    }

    public void SetResolution(bool value)
    {
        _fullScreen = fullScreenTogle.isOn;
        _resolucion = new Vector2(Screen.resolutions[_resolutionIndex].width, Screen.resolutions[_resolutionIndex].height);
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

        mixer.SetFloat("Music", _musicLvl);
        mixer.SetFloat("SoundEffects", _effectLvl);
        mixer.SetFloat("UiEffects", _uiLvl);


        _resolutionModeIndex = settingData.ResolutionMode;
        _resolutionIndex = settingData.Resolution;
        _resolucion = settingData.Resolucion.ToVector2();
        _fullScreen = settingData.FullScreen;


        if(_resolucion !=Vector2.zero)
            Screen.SetResolution((int)_resolucion.x, (int)_resolucion.y, _fullScreen);
        else
            Screen.SetResolution(1920,1080,true);

        Screen.fullScreenMode = rm[_resolutionModeIndex].Item1;

    }
}
