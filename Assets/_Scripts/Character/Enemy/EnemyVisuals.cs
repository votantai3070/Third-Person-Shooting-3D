using UnityEngine;

public enum EnemyType
{
    Melee,
    Range
}

public class EnemyVisuals : MonoBehaviour
{
    private Enemy enemy;

    [SerializeField] int health = 100;

    [Header("Enemy Models")]
    [SerializeField] EnemyType enemyType;

    [Header("Enemy Melee Weapon Models")]
    [SerializeField] EnemyWeaponModels enemyMeleeWeaponModel;
    [SerializeField] WeaponType enemyRangeWeaponModel;

    [Header("Enemy Range Weapon Models")]

    private EnemyWeaponModel[] enemyMeleeWeaponModels;
    private WeaponModels[] enemyRangeWeaponModels;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyMeleeWeaponModels = GetComponentsInChildren<EnemyWeaponModel>(true);
        enemyRangeWeaponModels = GetComponentsInChildren<WeaponModels>(true);

        RandomModel();
    }

    private void Start()
    {
        if (enemyType == EnemyType.Melee)
            ShowMeleeWeaponModel();
        else if (enemyType == EnemyType.Range)
            ShowRangeWeaponModel();
    }

    // Activate a random enemy melee model from the available models
    private void RandomModel()
    {
        Enemy_Models[] models = GetComponentsInChildren<Enemy_Models>();

        int randomIndex = Random.Range(0, models.Length);

        for (int i = 0; i < models.Length; i++)
        {
            if (i == randomIndex)
            {
                models[i].gameObject.SetActive(true);
            }
            else
            {
                models[i].gameObject.SetActive(false);
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    private void ShowMeleeWeaponModel()
    {
        foreach (var model in enemyMeleeWeaponModels)
        {
            if (model.weaponModel == enemyMeleeWeaponModel)
            {
                model.gameObject.SetActive(true);
            }
            else
            {
                model.gameObject.SetActive(false);
            }
        }
    }

    private void ShowRangeWeaponModel()
    {
        foreach (var model in enemyRangeWeaponModels)
        {
            if (model.weaponModelType == enemyRangeWeaponModel)
            {
                model.gameObject.SetActive(true);
            }
            else
            {
                model.gameObject.SetActive(false);
            }
        }
    }
}
