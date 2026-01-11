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
    public GameObject currentMeleeWeaponModel { get; private set; }
    [SerializeField] EnemyWeaponModels enemyMeleeWeaponModel;
    private EnemyWeaponModel[] enemyMeleeWeaponModels;

    [Header("Enemy Range Weapon Models")]
    public GameObject currentRangeWeaponModel { get; private set; }
    [SerializeField] WeaponType enemyRangeWeaponModel;
    public EnemyWeaponModel_Range[] enemyRangeWeaponModels;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyMeleeWeaponModels = GetComponentsInChildren<EnemyWeaponModel>(true);
        enemyRangeWeaponModels = GetComponentsInChildren<EnemyWeaponModel_Range>(true);

        RandomModel();
    }

    private void Start()
    {
        if (enemyType == EnemyType.Melee)
            ShowMeleeWeaponModel();
        else if (enemyType == EnemyType.Range)
            ShowRangeWeaponModel();
    }

    // Set the weight of a specific animation layer
    public void SetLayerAnimation(int layerIndex, float weight)
    {
        enemy.anim.SetLayerWeight(layerIndex, weight);
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

    #region Show Weapon Models
    private void ShowMeleeWeaponModel()
    {
        foreach (var model in enemyMeleeWeaponModels)
        {
            if (model.weaponModel == enemyMeleeWeaponModel)
            {
                model.gameObject.SetActive(true);
                currentMeleeWeaponModel = FindMeleeWeaponModel();
                break;
            }

        }
    }

    private void ShowRangeWeaponModel()
    {
        foreach (var model in enemyRangeWeaponModels)
        {
            bool shouldBeActive = (model.weaponType == enemyRangeWeaponModel);

            if (model.gameObject.activeSelf != shouldBeActive)
            {
                model.gameObject.SetActive(shouldBeActive);
            }

            if (shouldBeActive)
            {
                currentRangeWeaponModel = FindRangeWeaponModel();
            }
        }
    }

    private GameObject FindRangeWeaponModel()
    {
        foreach (var weaponModel in enemyRangeWeaponModels)
        {
            if (weaponModel.weaponType == enemyRangeWeaponModel)
            {
                return weaponModel.gameObject;
            }
        }

        return null;
    }

    private GameObject FindMeleeWeaponModel()
    {
        foreach (var weaponModel in enemyMeleeWeaponModels)
        {
            if (weaponModel.weaponModel == enemyMeleeWeaponModel)
            {
                return weaponModel.gameObject;
            }
        }

        return null;
    }


    #endregion
}
