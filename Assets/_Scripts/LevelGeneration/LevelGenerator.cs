using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance { get; private set; }

    //Enemies
    private List<Enemy> enemyList;

    // NavMesh
    [SerializeField] private NavMeshSurface navMeshSurface;
    [Space]

    // Level Parts
    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private List<Transform> levelParts;
    private List<Transform> currentLevelParts;
    private List<Transform> generatedLevelParts = new List<Transform>();

    // Snap Points
    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnapPoint;

    // Generation Control
    [Space]
    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        enemyList = new List<Enemy>();
        defaultSnapPoint = nextSnapPoint;
        InitializeGeneration();
    }

    private void Update()
    {
        if (generationOver)
            return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            if (currentLevelParts.Count > 0)
            {
                cooldownTimer = generationCooldown;
                GenerationNextLevelPart();
            }
            else if (generationOver == false)
            {
                FinishGeneration();
            }
        }
    }

    [ContextMenu("Restart Generation")]
    private void InitializeGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);
        DestroyOldLevelPartAndEnemies();
    }

    private void DestroyOldLevelPartAndEnemies()
    {
        foreach (Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }

        foreach (Transform part in generatedLevelParts)
        {
            Destroy(part.gameObject);
        }

        generatedLevelParts = new List<Transform>();
        enemyList = new List<Enemy>();
    }

    private void FinishGeneration()
    {
        generationOver = true;

        GenerationNextLevelPart();

        navMeshSurface.BuildNavMesh();

        foreach (Enemy enemy in enemyList)
        {
            enemy.transform.parent = null;
            enemy.gameObject.SetActive(true);
        }
    }

    [ContextMenu("Generate Next Level Part")]
    private void GenerationNextLevelPart()
    {
        Transform newPart = null;

        if (generationOver)
            newPart = Instantiate(lastLevelPart);
        else
            newPart = Instantiate(ChooseRandomPart());

        generatedLevelParts.Add(newPart);

        LevelPart levelPart = newPart.GetComponent<LevelPart>();

        levelPart.SnapAndAlignPartTo(nextSnapPoint);

        if (levelPart.IntersectionDetected())
        {
            InitializeGeneration();
            return;
        }

        nextSnapPoint = levelPart.GetExitSnapPoint();
        enemyList.AddRange(levelPart.MyEnemies());

        Debug.Log("Generated Level Part: " + newPart.name);
    }

    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);

        Transform choosePart = currentLevelParts[randomIndex];

        currentLevelParts.RemoveAt(randomIndex);

        return choosePart;
    }

    public Enemy ChooseRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemyList.Count);

        return enemyList[randomIndex];
    }

    public List<Enemy> GetEnemyList()
    {
        return enemyList;
    }
}
