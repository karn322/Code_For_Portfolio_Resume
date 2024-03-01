using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _Slider;

    void Start()
    {
        SoundManager.Instance.ChangeMasterVolume(_Slider.value);
        _Slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
    }
}
