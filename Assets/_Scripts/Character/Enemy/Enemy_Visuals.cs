using UnityEngine;

public enum EnemyType
{
    Melee,
    Range,
    Boss
}

public class Enemy_Visuals : MonoBehaviour
{
    private Enemy enemy;
    public GameObject currentWeaponModel { get; private set; }

    [Header("Enemy Models")]
    public EnemyType enemyType;

    [Header("Enemy Melee Weapon Models")]
    [SerializeField] EnemyWeaponModels enemyMeleeWeaponModel;
    [SerializeField] EnemyWeaponModel[] enemyMeleeWeaponModels;

    [Header("Enemy Range Weapon Models")]
    [SerializeField] WeaponType enemyRangeWeaponModel;
    [SerializeField] EnemyWeaponModel_Range[] enemyRangeWeaponModels;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyMeleeWeaponModels = GetComponentsInChildren<EnemyWeaponModel>(true);
        enemyRangeWeaponModels = GetComponentsInChildren<EnemyWeaponModel_Range>(true);

        RandomModel();
    }

    private void Start()
    {
        SetupWeapon();
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

    #region Show Weapon Models
    private void SetupWeapon()
    {
        bool thisEnemyIsMelee = GetComponent<Enemy_Melee>() != null;
        bool thisEnemyIsRange = GetComponent<Enemy_Range>() != null;


        if (thisEnemyIsRange)
            currentWeaponModel = FindRangeWeaponModel();

        if (thisEnemyIsMelee)
            currentWeaponModel = FindMeleeWeaponModel();

        currentWeaponModel.SetActive(true);
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
