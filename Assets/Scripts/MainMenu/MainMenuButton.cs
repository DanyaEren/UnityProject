using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Registration _registration;

    float second = 0;
    public float minute = 0;
    public int secondint = 0;

    [Header ("MenuScenes")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _crossHair;
    [SerializeField] private GameObject _setting;
    [SerializeField] private Camera _menuCamera;
    
    [Header ("Buttons")]
    [SerializeField] private GameObject _buttongame;
    [SerializeField] private GameObject _buttonsetting;
    [SerializeField] private GameObject _buttonexit;
    [SerializeField] private GameObject _secondbuttonexit;

    [Header ("ObjectScene")]
    Resolution[] rsl;
    List<string> resolutions;
    public AudioMixer audioMix;
    [SerializeField] private TMPro.TMP_Text _textVolume;
    [SerializeField] private Slider _sliderVolume;
    [SerializeField] private TMPro.TMP_Dropdown _dropdownResolutions;
    [SerializeField] private TMPro.TMP_Dropdown _dropdownQuallity;
    [SerializeField] private bool isFullScreen;

    [Header("GameTimer")]
    [SerializeField] private TMPro.TMP_Text _timer;
    bool gameStart = false;

    void Awake() //Начальные настройки игры
    {
        _player.SetActive(false);
        _crossHair.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        QualitySettings.SetQualityLevel(0);
        _secondbuttonexit.SetActive(false);

        //Разрешение экрана
        resolutions = new List<string>();
        rsl = Screen.resolutions;
        foreach (var i in rsl)
        {
            resolutions.Add(i.width + "x" + i.height);
        }
        _dropdownResolutions.ClearOptions();
        _dropdownResolutions.AddOptions(resolutions);
        //Полный экран
        isFullScreen = false;
        
        _textVolume.text = "Громкость " + Mathf.Round(_sliderVolume.value + 80).ToString() + "%";
    }

    private void Update()
    {
        if (_setting.active == false && _registration.registrationWindow.active == false && _registration.audentificationWindow.active == false)
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                _menuCamera.depth = 3;
                _setting.SetActive(true);
                _player.SetActive(false);
                _crossHair.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        if (_menu.active == false)
        {
            _secondbuttonexit.SetActive(true);
        }

        if(gameStart == true)
        {
            second += 1* Time.deltaTime;

            secondint = Mathf.RoundToInt(second);
            if( second >= 60f)
            {
                second = 0;
                minute += 1;
            }
            _timer.text = $"Время: {minute} М {secondint} С";
        }
    }

    public void StartGame() //Закрытие сцен и запуск игрового персонажа.
    {
        _player.SetActive(true);
        _crossHair.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        _menu.SetActive(false);
        _menuCamera.depth = -1;

        gameStart = true;
    }

    public void CloseSetting()
    {
        _setting.SetActive(false);
        _buttongame.SetActive(true);
        _buttonsetting.SetActive(true);
        _buttonexit.SetActive(true);
        if(_menu.active == false)
        {
            _menuCamera.depth = -1;
            _player.SetActive(true);
            _crossHair.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OpenSetting()
    {
        _buttongame.SetActive(false);
        _buttonsetting.SetActive(false);
        _buttonexit.SetActive(false);
        _setting.SetActive(true);  
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public void Quality()
    {
        int q = _dropdownQuallity.value;
        QualitySettings.SetQualityLevel(q);
    }

    public void Resolution()
    {
        int r = _dropdownResolutions.value;
        Debug.Log(r);
        Screen.SetResolution(rsl[r].width, rsl[r].height, isFullScreen);
    }

    public void AudioVolume()
    {
        float sliderValue = _sliderVolume.value;
        audioMix.SetFloat("masterVolume", sliderValue);
        _textVolume.text = "Громкость " + Mathf.Round(sliderValue + 80).ToString() + "%";
    }
}