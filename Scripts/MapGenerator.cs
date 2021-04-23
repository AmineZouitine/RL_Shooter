using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DisallowMultipleComponent]
public class MapGenerator : MonoBehaviour
{
    #region Inspector Variables
    [Header("Random")]
    [SerializeField] private bool m_isRandom;
    [SerializeField, Range(1, 100)] private int m_minMapSize;
    [SerializeField, Range(1, 100)] private int m_maxMapSize;
    [SerializeField, Range(0.01f, 10)] private float m_timeBetweenSpawn;
    [SerializeField, Range(0, 0.2f)] private float m_minObstacleFactor;
    [SerializeField, Range(0, 0.2f)] private float m_maxObstacleFactor;

    [Header("Prefabs")]
    [SerializeField] private GameObject m_tilePrefab;
    [SerializeField] private GameObject m_agentPrefab;
    [SerializeField] private GameObject m_enemyPrefab;
    [SerializeField] private GameObject m_obstaclePrefab;
    [SerializeField] private GameObject m_decordPrefab;
    [SerializeField] private GameObject m_navMeshPrefab;

    [Header("Map Information")]
    [SerializeField] private Vector2Int m_mapSize;
    [SerializeField, Range(1, 10)] private float m_objectSize;
    [SerializeField, Range(0, 5)] private float m_distanceBetweenTiles;
    [SerializeField, Range(0, 20)] private float m_minHeightObstacle;
    [SerializeField, Range(0, 20)] private float m_maxHeightObstacle;
    [SerializeField, Range(1, 50)] private int m_borderSize;
    [SerializeField] private Color m_startColor;
    [SerializeField] private Color m_endColor;


    //If not random 
    [Header("Pos")]
    [SerializeField] private Vector2Int m_agentPos;
    [SerializeField] private Vector2Int[] m_enemyPos;
    [SerializeField] private Vector2Int[] m_obstaclePos;
    [SerializeField] private Transform m_spawnpos;

    [Header("Transform Content")]
    [SerializeField] private Transform m_tileContent;
    [SerializeField] private Transform m_obstacleContent;
    [SerializeField] private Transform m_enemyContent;
    [SerializeField] private Transform m_agentContent;
    [SerializeField] private Transform m_decordContent;

    #endregion



    #region Private Variables
    private Vector3 m_modelPosOffSetAgent = new Vector3(2.74f, 0.7f, -0.23f); // Delete this if your model have good transform
    private Vector3 m_modelPosOffSetEnemy = new Vector3(-0.059f, 0.95f, -0.23f); // Delete this if your model have good transform
    private bool m_once;
    private Vector3 m_startPos;
    private List<Vector2Int> disponibleTilesPos = new List<Vector2Int>();
    private List<GameObject> allObjects = new List<GameObject>();


    #endregion


    public void Reset()
    {
        ClearAllObjects();
        StartEpisode();
    }

    private void SetInitialReferences()
    {
        if (m_isRandom) m_mapSize = new Vector2Int(Random.Range(m_minMapSize, m_maxMapSize + 1), Random.Range(m_minMapSize, m_maxMapSize + 1));
        m_startPos = m_spawnpos.position;
        SetPrefabsSize();
        Instantiate(m_navMeshPrefab, new Vector3(10 + m_startPos.x, 1, 10 + m_startPos.z), Quaternion.identity);
        GenerateTiles();
        GenerateBorder();
       
    }
    private void StartEpisode()
    {
        if (!m_once)
        {
            SetInitialReferences();
            m_once = true;
        }
        StopAllCoroutines();
        GetDisponiblesPosRandom();
        if (m_isRandom) { SetMapRandom(); }
        else { SetMap(); }
    }

    private void SetMap()
    {
        SpawnAgent();
        SpawnEnemies();
        SpawnObstacles();
    }



    private void ClearAllObjects()
    {
        foreach (GameObject obj in allObjects.ToList())
        {
            Destroy(obj);
        }
        allObjects.Clear();
    }

    private void SetMapRandom()
    {
        SpawnPlayerRandom();
        SpawnObstaclesRandom();
        SpawnEnemiesRandom();
    }

    private void SpawnEnemiesRandom()
    {
        StartCoroutine(SpawnRandomEnemies());
    }

    private void SpawnObstaclesRandom()
    {
        float randomObstacleFactor = Random.Range(m_minObstacleFactor, m_maxObstacleFactor);
        int obstacleNumber = (int)((m_mapSize.x * m_mapSize.y) * randomObstacleFactor);
        for (int i = 0; i < obstacleNumber; i++)
        {
            float obstacleHeight = Random.Range(m_minHeightObstacle, m_maxHeightObstacle);
            Vector3 offSetObstacle = new Vector3(0, obstacleHeight / 2 + 0.5f, 0);
            GameObject currentObstacle = Spawn(m_obstaclePrefab, GetRandomPosition(), offSetObstacle);
            currentObstacle.transform.localScale += new Vector3(0, obstacleHeight, 0);
            currentObstacle.transform.parent = m_obstacleContent;
        }
    }

    private void SpawnPlayerRandom()
    {
        m_agentPos = GetRandomPosition();
        m_agentPrefab.transform.position = new Vector3(m_startPos.x + m_objectSize * m_agentPos.x, 0, m_startPos.z + m_objectSize * m_agentPos.y) + m_modelPosOffSetAgent;
    }

    private void GenerateTiles()
    {
        Vector3 posToSpawn = m_startPos;
        for (int i = 0; i < m_mapSize.x; i++)
        {
            for (int j = 0; j < m_mapSize.y; j++)
            {
                GameObject currentTileSpawn = Instantiate(m_tilePrefab, posToSpawn, Quaternion.identity);
                currentTileSpawn.transform.parent = m_tileContent;
                posToSpawn += new Vector3(m_objectSize, 0, 0);
            }
            posToSpawn = m_startPos + new Vector3(0, 0, m_objectSize * (i + 1) + m_distanceBetweenTiles);
        }
    }


    private void SpawnObstacles()
    {
        foreach (Vector2Int pos in m_obstaclePos)
        {
            GameObject currentObstacle = Spawn(m_obstaclePrefab, pos, Vector3.zero);
            float obstacleHeight = Random.Range(m_minHeightObstacle, m_maxHeightObstacle);
            currentObstacle.transform.localScale += new Vector3(0, obstacleHeight, 0);
            currentObstacle.transform.parent = m_obstacleContent;
        }
    }

    private IEnumerator SpawnRandomEnemies()
    {
        while (true)
        {
            GameObject currentEnemySpawn = Spawn(m_enemyPrefab, GetRandomPosition(), m_modelPosOffSetEnemy);
            currentEnemySpawn.transform.parent = m_enemyContent;
            yield return new WaitForSeconds(m_timeBetweenSpawn);
        }
    }
    private void SpawnEnemies()
    {
        foreach (Vector2Int pos in m_enemyPos)
        {
            GameObject currentEnemySpawn = Spawn(m_enemyPrefab, pos, m_modelPosOffSetEnemy);
            currentEnemySpawn.transform.parent = m_enemyContent;
        }
    }
    private void SpawnAgent()
    {
        GameObject currentAgentSpawn = Spawn(m_agentPrefab, m_agentPos, m_modelPosOffSetAgent);
        currentAgentSpawn.transform.parent = m_agentContent;
    }
    private GameObject Spawn(GameObject _gameobject, Vector2Int _pos, Vector3 _offSet)
    {
        Vector3 posToSpawn = new Vector3(m_startPos.x + m_objectSize * _pos.x, 0, m_startPos.z + m_objectSize * _pos.y) + _offSet;
        GameObject currentObject = Instantiate(_gameobject, posToSpawn, Quaternion.identity);
        allObjects.Add(currentObject);
        return currentObject;
    }

    private void SetPrefabsSize()
    {
        m_tilePrefab.transform.localScale = new Vector3(m_objectSize, m_objectSize, m_objectSize);
        m_agentPrefab.transform.localScale = new Vector3(m_objectSize / 3, m_objectSize / 3, m_objectSize / 3);
        m_enemyPrefab.transform.localScale = new Vector3(m_objectSize / 3, m_objectSize / 3, m_objectSize / 3);
        m_obstaclePrefab.transform.localScale = new Vector3(m_objectSize, m_objectSize, m_objectSize);
    }




    private void GetDisponiblesPosRandom()
    {
        for (int i = 0; i < m_mapSize.x; i++)
        {
            for (int j = 0; j < m_mapSize.y; j++)
            {
                disponibleTilesPos.Add(new Vector2Int(i, j));
            }
        }
        Shuffle();
    }
    private void Shuffle()
    {
        Vector2Int myPos = new Vector2Int();
        System.Random _random = new System.Random();
        int n = disponibleTilesPos.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(_random.NextDouble() * (n - i));
            myPos = disponibleTilesPos[r];
            disponibleTilesPos[r] = disponibleTilesPos[i];
            disponibleTilesPos[i] = myPos;
        }
    }
    private Vector2Int GetRandomPosition()
    {
        int RandomIndex = Random.Range(0, disponibleTilesPos.Count);
        Vector2Int elemTemp = disponibleTilesPos[RandomIndex];
        Vector2Int elem = new Vector2Int(elemTemp.x, elemTemp.y);
        disponibleTilesPos.Remove(elemTemp);
        return elem;
    }

    private void GenerateBorder()
    {
        int offSetCount = 2;
        for (int i = 0; i < m_borderSize; i++)
        {
            GenerateCube(m_mapSize.x + offSetCount, i + 1);
            offSetCount += 2;
        }
    }
    private void GenerateCube(int _size, int _count)
    {
        Vector3 initialPos = new Vector3
                       (m_spawnpos.position.x - (_count * m_objectSize),
                       m_spawnpos.position.y,
                       m_spawnpos.position.z - (_count * m_objectSize));
        List<GameObject> decordSpawn = new List<GameObject>();

        for (int i = 0; i < _size; i++)
        {
            Vector3 currentPos = initialPos + new Vector3(0, 0, i * m_objectSize);

            for (int j = 0; j < _size; j++)
            {

                if (i == 0 || i == _size - 1)
                {
                    decordSpawn.Add(Instantiate(m_decordPrefab, currentPos, Quaternion.identity));
                }
                else
                {
                    decordSpawn.Add(Instantiate(m_decordPrefab, currentPos, Quaternion.identity));
                    currentPos += new Vector3(_size - 1 * m_objectSize, 0, 0);
                    decordSpawn.Add(Instantiate(m_decordPrefab, currentPos, Quaternion.identity));
                    break;
                }
                currentPos += new Vector3(m_objectSize, 0, 0);
            }

        }

        AddAllToDecordContent(decordSpawn);
        SetAllDecordSize(_count, decordSpawn);
    }

    private void SetAllDecordSize(int _count, List<GameObject> decordSpawn)
    {
        foreach (GameObject obj in decordSpawn)
        {
            SetDecordSize(obj, _count);
        }
    }

    private void AddAllToDecordContent(List<GameObject> _decordSpawn)
    {
        foreach (GameObject obj in _decordSpawn)
        {
            obj.transform.parent = m_decordContent;
        }
    }
    private void SetDecordSize(GameObject _decord, int _count)
    {
        float minSize = 3 + (_count) * 1.3f - 0.2f * _count;
        float maxSize = 3 + 3 + (_count) * 1.8f;
        float size = Random.Range(minSize, maxSize);
        _decord.transform.localScale = new Vector3(m_objectSize, size, m_objectSize);
    }


}
