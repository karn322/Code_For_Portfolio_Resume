using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public enum WaveStyle
{
    None,
    Circle, 
    LeftToRight,
    RightToLeft,
    TopToDown,
    DownToTop
}

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string _WaveName;
        public List<EnemyGroup> _EnemyGroups;
        [Tooltip("minute")]
        public float _Duration;
        [Tooltip("second")]
        public float _SpawnEvery; //time between each spawn
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string _EnemyName;
        public EnemySctiptableObject _EnemyData;
        public bool _IsBoss;
        public WaveStyle _IsWave;
        [HideInInspector] public bool _IsSpawn;
    }

    [Header("Spawn a set of every enemy in enemy group")]
    [SerializeField] GameObject _EnemyPrefab;
    [SerializeField] GameObject _EnemyInWavePrefab;

    [System.Serializable]
    public class WavePosition
    {
       public WaveStyle _Wave;
       public Transform[] _Transforms;
    }

    [SerializeField] WavePosition[] _WaveStyle;

    [SerializeField] List<Wave> _Waves; //list all the waves in this game
    [SerializeField] int _CurrentWaveCount;

    [Header("Spawner Attributes")]
    float _SpawnTime;
    [SerializeField] int _EnemiesAlive; //alive in present
    [SerializeField] int _MaxEnemiesAllowed; 
    [SerializeField] bool _MaxEnemiesReached; //if reach limit pause spawn

    float _CurrentInterval;

    [Header("Spawn Point")]
    public List<Transform> _RelativeSpawnPoints;

    Transform _Player;

    ObjectPool _Pool;



    void Start()
    {
        _Player = FindObjectOfType<PlayerStats>().transform;
        _Pool = FindObjectOfType<ObjectPool>();
    }

    void Update()
    {
        _CurrentInterval += Time.deltaTime;
        if (_CurrentInterval >= _Waves[_CurrentWaveCount]._Duration * 60)
        {
            _CurrentWaveCount++;
            _CurrentInterval -= _Waves[_CurrentWaveCount]._Duration * 60;
        }
    
        _SpawnTime += Time.deltaTime;

        if(_SpawnTime >= _Waves[_CurrentWaveCount]._SpawnEvery)
        {
            _SpawnTime -= _Waves[_CurrentWaveCount]._SpawnEvery;
            SpawnEnemies();
        }

        //limit enemies spawn
        if (_EnemiesAlive >= _MaxEnemiesAllowed)
        {
            _MaxEnemiesReached = true;
        }
    }

    //Will stop spawning if reach limit
    //Will spawn only in a wave
    //spawn in set if enemy
    void SpawnEnemies() 
    {
        if (!_MaxEnemiesReached)
        {
            List<Transform> spawnPoint = new List<Transform>(_RelativeSpawnPoints);
            //spawn each type enemy till quota is filled
            foreach (var enemyGroup in _Waves[_CurrentWaveCount]._EnemyGroups)
            {
                //spawn Enemy
                if (!_MaxEnemiesReached)
                {
                    if(enemyGroup._EnemyData == null)
                    {
                        continue;
                    }

                    if (!enemyGroup._IsSpawn)
                    {
                        if (enemyGroup._IsWave != WaveStyle.None)
                        {
                            for (int i = 0; i < _WaveStyle[(int)enemyGroup._IsWave - 1]._Transforms.Length; i++)
                            {
                                GameObject enemyWave = _Pool.GetObject(_EnemyInWavePrefab);
                                enemyWave.transform.position = _WaveStyle[(int)enemyGroup._IsWave - 1]._Transforms[i].position + _Player.transform.position;
                                enemyWave.GetComponent<EnemyStats>().GetStats(enemyGroup._EnemyData);
                                EnemyMove move = enemyWave.GetComponent<EnemyMove>();
                                switch (_WaveStyle[(int)enemyGroup._IsWave - 1]._Wave)
                                {
                                    case WaveStyle.Circle:
                                        move._MoveTo = MoveTo.None;
                                        break;
                                    case WaveStyle.LeftToRight:
                                        move._MoveTo = MoveTo.MoveRight;
                                        break;
                                    case WaveStyle.RightToLeft:
                                        move._MoveTo = MoveTo.MoveLeft;
                                        break;
                                    case WaveStyle.TopToDown:
                                        move._MoveTo = MoveTo.MoveDown;
                                        break;
                                    case WaveStyle.DownToTop:
                                        move._MoveTo = MoveTo.MoveUp;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            int randPoint = Random.Range(0, spawnPoint.Count); //rand spawn at spawn point
                            GameObject enemy = _Pool.GetObject(_EnemyPrefab);
                            enemy.transform.position = spawnPoint[randPoint].position + _Player.transform.position;
                            enemy.GetComponent<EnemyStats>().GetStats(enemyGroup._EnemyData);

                            spawnPoint.RemoveAt(randPoint);
                            _EnemiesAlive++;
                        }
                    }              

                    if (enemyGroup._IsBoss)
                    {
                        enemyGroup._IsSpawn = true;
                    }
                }
            }
        }
    }

    public void OnEnemyKilled()
    {
        _EnemiesAlive--;

        if (_EnemiesAlive < _MaxEnemiesAllowed)
        {
            _MaxEnemiesReached = false;
        }
    }

}
