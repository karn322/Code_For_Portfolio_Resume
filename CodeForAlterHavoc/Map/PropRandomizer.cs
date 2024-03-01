using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public TerrainChunk _Chunk;
    public List<GameObject> _PripSpawnPoint;
    public List<GameObject> _PripPrefabs;

    public List<Transform> _NextSpawnChunk;
    ObjectPool _Pool;

    void Start()
    {
        _Pool = FindObjectOfType<ObjectPool>();
        SpawnProps();
    }

    void SpawnProps()
    {
        foreach(GameObject sp in _PripSpawnPoint)
        {
            int rand = Random.Range(0, _PripPrefabs.Count);
            Vector3 inCurcle = Random.insideUnitCircle;
            GameObject prop;
            if (_PripPrefabs[rand].TryGetComponent<BreakableProp>(out BreakableProp breakable) && _Pool != null)
            {
                prop = _Pool.GetObject(_PripPrefabs[rand]);
                prop.GetComponent<BreakableProp>().Respawn();
            }
            else
            {
                prop = Instantiate(_PripPrefabs[rand]);
            }
            prop.transform.position = sp.transform.position + inCurcle;
            prop.transform.parent = sp.transform;
        }
    }
}
 