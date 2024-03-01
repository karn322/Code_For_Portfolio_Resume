    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenBackpack : MonoBehaviour
{
    [SerializeField] GameObject _BackpackInventory;
    [SerializeField] Transform _Open;
    [SerializeField] Transform _Close;

    private bool _IsOpen = false;
    private float _Timer;

    void Start()
    {
        _BackpackInventory.transform.position = _Close.position;
        _Timer = 0;
        _IsOpen = false;
    }

    private void Update()
    {
        if (_IsOpen)
        {
            _Timer += Time.deltaTime;
        }

        if (_Timer >= 0.5f)
        {
            StartCoroutine(MoveInventory(_Close.transform));
            _IsOpen = false;
            _Timer = 0;
        }
    }

    private void OnMouseOver()
    {
        if (!_IsOpen)
        {
            SoundManager.Instance.PlayEffect(Sound.SoundEffectName.OpenMap);
            StartCoroutine(MoveInventory(_Open.transform));
            _IsOpen = true;
        }
        _Timer = 0;
    }

    IEnumerator MoveInventory(Transform transform)
    {
        for (float t = 0f; t <= 1; t += Time.deltaTime)
        {
            _BackpackInventory.transform.position = Vector3.Lerp(_BackpackInventory.transform.position , transform.position, t);
            yield return null;
        }
    }
}
