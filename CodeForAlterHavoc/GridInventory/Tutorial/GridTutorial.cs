using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTutorial : MonoBehaviour
{
    [SerializeField] RectTransform _Arrow;
    [SerializeField] RectTransform _ArrowRotation;
    [SerializeField] Vector3[] _ARotation;
    [SerializeField] Transform[] _ArrowPosition;

    [SerializeField] RectTransform _Mask;
    [SerializeField] Transform[] _MaskPosition;
    [SerializeField] int[] _MaskSize;

    int _PosIndex;
    bool _Next;

    private void Start()
    {
        _ArrowRotation.transform.Rotate(_ARotation[_PosIndex]);
        _Arrow.transform.position = _ArrowPosition[_PosIndex].position;

        _Mask.transform.position = _MaskPosition[_PosIndex].position;
        _Mask.sizeDelta = new Vector2(_MaskSize[_PosIndex], _MaskSize[_PosIndex]);

        _PosIndex++;
    }

    private void Update()
    {
        if (_PosIndex == 1)
        {
            if (GameManager._Instance._CurrentState == GameManager.GameState.UsingInventory)
            {
                _Next = true;
            }
        }

        if (_Next)
        {
            _ArrowRotation.transform.rotation = Quaternion.Euler(_ARotation[_PosIndex]);
            _Arrow.transform.position = _ArrowPosition[_PosIndex].position;

            _Mask.transform.position = _MaskPosition[_PosIndex].position;
            _Mask.sizeDelta = new Vector2(_MaskSize[_PosIndex], _MaskSize[_PosIndex]);

            _PosIndex++;
            _Next = false;
        }
    }

    public void NextTutorial()
    {
        if (_PosIndex > 1)
        {
            if (_PosIndex == _ArrowPosition.Length)
            {
                GameManager._Instance._FirstTutorial = true;
                GameManager._Instance._SaveData._FirstTutorial = true;
                GameManager._Instance._IsTutorial = false;
                gameObject.SetActive(false);
            }
            else
            {
                _Next = true;
            }
        }
    }
}
