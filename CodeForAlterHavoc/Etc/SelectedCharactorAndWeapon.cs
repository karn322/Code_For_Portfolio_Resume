using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCharactorAndWeapon : MonoBehaviour
{
    bool _IsSeletedCharactor;
    [SerializeField] GameObject _HighlightCharactor;
    [SerializeField] Transform[] _CharactorPos;
    int _CharactorIndex;

    [SerializeField] GameObject _LockWeapon;
    [SerializeField] GameObject[] _selectedWeapon;
    int _SelectetingWeapon;
    int _WeaponIndex = -1;

    bool _IsSeletedWeapon;
    [SerializeField] GameObject _HighlightWeapon;
    [SerializeField] Transform[] _WeaponPos;

    int _LastWeaponSelectedID;
    WeaponScriptableObject _LastWeaponSelectedWeapon;

    [SerializeField] Image _SelectedButton;
    [SerializeField] GameObject _SelectedButtonHighlight;

    [SerializeField] GameObject[] _CharactorImage;
    Vector3[] _CharactorImagePos;
    [SerializeField] Color _FadeBlack;

    [SerializeField] GameObject _WeaponData;
    [SerializeField] TMP_Text _WeaponName;
    [SerializeField] TMP_Text _WeaponDescription;
    [SerializeField] TMP_Text _WeaponDamage;

    private void Start()
    {
        _CharactorImagePos = new Vector3[_CharactorImage.Length];
        for (int i = 0; i < _CharactorImagePos.Length; i++)
        {
            _CharactorImagePos[i] = _CharactorImage[i].transform.position;
        }
    }

    #region Charactor
    public void SelectedCharactor(int num)
    {
        if (_IsSeletedCharactor)
        {
            _HighlightCharactor.transform.SetParent(_CharactorPos[num]);
            _HighlightCharactor.transform.SetAsFirstSibling();
            _HighlightCharactor.transform.position = _CharactorPos[num].position;
        }
        else
        {
            _IsSeletedCharactor = true; 
            _LockWeapon.SetActive(false);
        }
        _IsSeletedWeapon = false;
        _HighlightWeapon.SetActive(false);
        ShowWeapon(num);
        _CharactorIndex = num;
        ChangeCharactorImagePos(num);
        ChangeSelectedIcon();
    }

    public void HighlightUnselectedCharactor(int num)
    {
        _HighlightCharactor.transform.SetParent(_CharactorPos[num]);
        _HighlightCharactor.transform.SetAsFirstSibling();
        _HighlightCharactor.transform.position = _CharactorPos[num].position;
        _HighlightCharactor.SetActive(true);
        ChangeCharactorImagePos(num);
        ShowWeapon(num);
    }

    public void UnHighlightSelectedCaractor()
    {
        if (!_IsSeletedCharactor)
        {
            _HighlightCharactor.SetActive(false);
        }
        else
        {
            _HighlightCharactor.transform.SetParent(_CharactorPos[_CharactorIndex]);
            _HighlightCharactor.transform.SetAsFirstSibling();
            _HighlightCharactor.transform.position = _CharactorPos[_CharactorIndex].position;
            ChangeCharactorImagePos(_CharactorIndex);
            ShowWeapon(_CharactorIndex);
        }
    }
    #endregion
    #region Weapon
    public void ShowWeapon(int num)
    {
        for (int i = 0; i < _selectedWeapon.Length; i++)
        {
            _selectedWeapon[i].SetActive(false);
        }
        _SelectetingWeapon = num;
        _selectedWeapon[num].SetActive(true);
    }

    public void SelectedWeapon(int num)
    {
        if (_IsSeletedWeapon)
        {
            _HighlightWeapon.transform.position = _WeaponPos[num].position;
        }
        else
        {
            _IsSeletedWeapon = true;
        }
        _WeaponIndex = _SelectetingWeapon;
        _LastWeaponSelectedID = num;
        ChangeSelectedIcon();
    }

    public void SetSelectedWeapon(WeaponScriptableObject weapon)
    {
        _LastWeaponSelectedWeapon = weapon;
    }

    public void HighlightUnselectedWeapon(int num)
    {
        _HighlightWeapon.transform.position = _WeaponPos[num].position;
        _HighlightWeapon.SetActive(true);
    }

    public void UnHighlightUnselectedWeapon()
    {
        if (!_IsSeletedWeapon)
        {
            _HighlightWeapon.SetActive(false);
        }
        else
        {
            _HighlightWeapon.transform.position = _WeaponPos[_LastWeaponSelectedID].position;
            ChangeWeaponDescription(_LastWeaponSelectedWeapon);
        }
    }
    #endregion
    public void UsedThisWeapon(string name)
    {
        if(_CharactorIndex == _WeaponIndex)
        {
            GetComponent<SceneController>().SceneChange(name);
        }
    }
    #region Ui
    void ChangeCharactorImagePos(int num)
    {
        _CharactorImage[num].transform.SetAsLastSibling();
        for (int i = 0; i < _CharactorImage.Length; i++)
        {
            int posIndex = i + num;
            if(posIndex >= _CharactorImage.Length)
                posIndex-=_CharactorImage.Length;

            _CharactorImage[i].transform.position = _CharactorImagePos[posIndex];
            if(i != num)
            {
                _CharactorImage[i].GetComponent<Image>().color = _FadeBlack;
            }
            else
            {
                _CharactorImage[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void ChangeWeaponDescription(WeaponScriptableObject weapon)
    {
        _WeaponData.SetActive(true);
        _WeaponName.text = weapon.Name;
        _WeaponDescription.text = weapon.Description;
        _WeaponDamage.text = " Attack : " + weapon.Damage.ToString() + Environment.NewLine + " Every : " + weapon.CooldownDuration.ToString() + " sec";
    }

    void ChangeSelectedIcon()
    {
        if (_CharactorIndex == _WeaponIndex)
        {
            _SelectedButton.color = Color.white;
        }
        else
        {
            _SelectedButton.color = new Color(1, 1, 1, .5f);
            _WeaponData.SetActive(false);
        }
    }

    public void HilightSelectedButton()
    {
        if(_SelectedButton.color == Color.white)
        {
            _SelectedButtonHighlight.SetActive(true);
        }
    }
    #endregion
}
