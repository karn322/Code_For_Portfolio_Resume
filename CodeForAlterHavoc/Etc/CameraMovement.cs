using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform _Target;
    [SerializeField] Vector3 _Offset;

    public bool _StartShake = false;
    [SerializeField] AnimationCurve _Curve;
    [SerializeField] float _Duration;

    private void Update()
    {
        if (_StartShake)
        {
            _StartShake = false;
            StartCoroutine(Shaking());
        }
        else
        {
            transform.position = _Target.position + _Offset;
        }
    }

    IEnumerator Shaking()
    {
        float elapsedTime = 0;
        while (elapsedTime < _Duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = _Curve.Evaluate(elapsedTime / _Duration);
            transform.position = _Target.position + _Offset + Random.insideUnitSphere * strength;

            yield return null;
        }
    }
}
