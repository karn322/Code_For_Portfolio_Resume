using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DropRateManager;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public GameObject _ItemPrefab;
        public float _DropRate;
    }

    public List<Drops> _Drops;
    public bool _DontDrop;

    ObjectPool _Pool;

    private void Start()
    {
        _Pool = FindObjectOfType<ObjectPool>();
    }

    public void DropItem()
    {
        if (!gameObject.scene.isLoaded)
            return;
        if (_DontDrop)
            return;

        float rand = Random.Range(0, 100f);
        List<Drops> possibleDrop = new List<Drops>();

        foreach (Drops rate in _Drops)
        {
            if (rand <= rate._DropRate)
            {
                possibleDrop.Add(rate);
                // if want to drop all just instantiate this line 
            }
        }

        for (int i = 0; i < possibleDrop.Count; i++)
        {
            Vector3 inCurcle = Random.insideUnitCircle;
            GameObject Drop = _Pool.GetObject(possibleDrop[i]._ItemPrefab);

            Drop.transform.position = transform.position + inCurcle;

            if (Drop.TryGetComponent<BobblingAnimate>(out BobblingAnimate bobbling))
            {
                bobbling.Respawn(transform.position + inCurcle);
            }
        }
    }
}
