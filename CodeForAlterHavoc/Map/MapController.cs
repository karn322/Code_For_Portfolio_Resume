using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TerrainChunk
{
    Normal,
    Sand,
    Snow,
    Swamp
}

public class MapController : MonoBehaviour
{
    [SerializeField] List<GameObject> _NormalChunkSpawn;
    [SerializeField] List<GameObject> _SandChunkSpawn;
    [SerializeField] List<GameObject> _SnowChunkSpawn;
    [SerializeField] List<GameObject> _SwampChunkSpawn;
    [SerializeField] Transform _SpawnAt;

    [SerializeField] GameObject _Player;
    [SerializeField] float _CheckRadius;
    [SerializeField] LayerMask _TerrainMask;

    public GameObject _CurrentChunk;
    bool _IsSpawn;

    [Header("Optimization")]
    [SerializeField] List<GameObject> _SpawndedChunks;
    GameObject _LastedChunk;
    [SerializeField] float _MaxRenderDistance;
    float _RenderDistance;
    float OPCooldawn;
    [SerializeField] float OPCooldownDuration;

    void Update()
    {
        ChunkChecker();
        ChunkOptimization();
    }

    void ChunkChecker()
    {
        if (!_CurrentChunk)
        {
            _IsSpawn = false;
            return;
        }

        if (!_IsSpawn)
        {
            List<Transform> nextPosition = _CurrentChunk.GetComponent<PropRandomizer>()._NextSpawnChunk;
            for (int i = 0; i < nextPosition.Count; i++)
            {
                CheckAndSpawnChunk(nextPosition[i]);
            }
        }
    }

    void CheckAndSpawnChunk(Transform transform)
    {
        if (!Physics2D.OverlapCircle(transform.position, _CheckRadius, _TerrainMask))
        {
            SpawnChunk(transform.position);
        }
    }

    void SpawnChunk(Vector3 Position)
    {  
        TerrainChunk chunk = _CurrentChunk.GetComponent<PropRandomizer>()._Chunk;

        switch (chunk)
        {
            case TerrainChunk.Normal:
                Spawn(_NormalChunkSpawn, Position);
                break;
            case TerrainChunk.Sand:
                Spawn(_SandChunkSpawn, Position);
                break;
            case TerrainChunk.Snow:
                Spawn(_SnowChunkSpawn, Position);
                break;
            case TerrainChunk.Swamp:
                Spawn(_SwampChunkSpawn, Position);
                break;
        }
    }

    void Spawn(List<GameObject> list, Vector3 Position)
    {
        int rand = Random.Range(0, list.Count);
        _LastedChunk = Instantiate(list[rand], Position, Quaternion.identity);
        _LastedChunk.transform.SetParent(_SpawnAt);
        _SpawndedChunks.Add(_LastedChunk);
    }

    void ChunkOptimization()
    {
        OPCooldawn -= Time.deltaTime;

        if (OPCooldawn <= 0f)
            OPCooldawn = OPCooldownDuration;
        else
            return;

        foreach (GameObject Chunk in _SpawndedChunks)
        {
            _RenderDistance = Vector3.Distance(_Player.transform.position, Chunk.transform.position);
            if(_RenderDistance > _MaxRenderDistance)
            {
                Chunk.SetActive(false);
            }
            else
            {
                Chunk.SetActive(true);
            }
        }
    }
}
