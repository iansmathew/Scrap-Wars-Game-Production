using UnityEngine;

public class HazardManagerScript : MonoBehaviour {
    [Header("Prefabs")]
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject minePrefab;
    [Header("Miscellaneous")]
	[SerializeField] LayerMask invertGroundMask;
	[SerializeField] Transform[] hazardSpawnPoints;
	[SerializeField] GameObject emptyGO;

    public static int asteroidCount;
    public static int mineCount;
    public static int wallCount;

    [Header("Overlap Check Radii")]
    [SerializeField] float mineSpawnRadius;
	[SerializeField] float wallSpawnRadius;

    [Header("Hazard Counts")]
	[SerializeField] private int maxAsteroidCount;
	[SerializeField] private int maxWallCount;
	[SerializeField] private int maxMineCount;

    public static GameObject player;

    private float lastEvent;
    private float eventDelay = 0.2f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	private void Start()
	{
		invertGroundMask = ~invertGroundMask;
        eventDelay = eventDelay * 60.0f;
	}

    private void Update()
    {
        if (Time.time > lastEvent + eventDelay)
        {
            lastEvent = Time.time;
            SpawnEvent();
        }
    }

    private void SpawnEvent()
    {
        int eventIndex = Random.Range(0, 3);
        switch(eventIndex)
        {
            case 0:
                SpawnMines();
                break;
            case 1:
                SpawnWalls();
                break;
            case 2:
                SpawnAsteroids();
                break;
            default:
                Debug.LogError("Unusuable event index.. Check Spawn Event", gameObject);
                break;
        }
    }

	private void SpawnMines()
	{
		if (mineCount >= maxMineCount)
			return;

		for (int i = mineCount; i < maxMineCount; i++) 
		{
			bool foundPosition = false;
			int spawnGetTries = 0;
			Transform spawn = emptyGO.GetComponent<Transform>();

			while (spawnGetTries <= hazardSpawnPoints.Length) 
			{
				int index = Random.Range (0, hazardSpawnPoints.Length);
				spawn = hazardSpawnPoints[index];
				if (!Physics.CheckSphere (spawn.position, mineSpawnRadius, invertGroundMask)) {
					foundPosition = true;
					break;
				}
				spawnGetTries++;
			}

			if (!foundPosition)
				continue;

			Instantiate (minePrefab, spawn);
		}
	}

    private void SpawnWalls()
    {
        if (wallCount >= maxWallCount)
            return;

        for (int i = wallCount; i < maxWallCount; i++)
        {
            bool foundPosition = false;
            int spawnGetTries = 0;
            Transform spawn = emptyGO.GetComponent<Transform>();

            while (spawnGetTries <= hazardSpawnPoints.Length)
            {
                int index = Random.Range(0, hazardSpawnPoints.Length);
                spawn = hazardSpawnPoints[index];
                if (!Physics.CheckSphere(spawn.position, wallSpawnRadius, invertGroundMask))
                {
                    spawn.position = new Vector3(spawn.position.x, -6.0f, spawn.position.z);
                    spawn.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
                    foundPosition = true;
                    break;
                }
                spawnGetTries++;
            }

            if (!foundPosition)
                continue;

            Instantiate(wallPrefab, spawn);
        }
    }

    private void SpawnAsteroids()
    {
        if (asteroidCount >= maxAsteroidCount)
            return;

        for (int i = asteroidCount; i < maxAsteroidCount; i++)
        {
            bool foundPosition = false;
            int spawnGetTries = 0;
            Transform spawn = emptyGO.GetComponent<Transform>();

            while (spawnGetTries <= hazardSpawnPoints.Length)
            {
                int index = Random.Range(0, hazardSpawnPoints.Length);
                spawn = hazardSpawnPoints[index];
                if (!Physics.CheckSphere(spawn.position, wallSpawnRadius, invertGroundMask))
                {
                    spawn.position = new Vector3(spawn.position.x, Random.Range(20.0f, 50.0f), spawn.position.z);
                    foundPosition = true;
                    break;
                }
                spawnGetTries++;
            }

            if (!foundPosition)
                continue;

            Instantiate(asteroidPrefab, spawn);
        }
    }
}
