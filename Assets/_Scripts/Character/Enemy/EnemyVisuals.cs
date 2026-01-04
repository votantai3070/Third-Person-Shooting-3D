using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    private Enemy enemy;

    [SerializeField] int health = 100;

    [SerializeField] EnemyWeaponModels enemyWeaponModel;

    private EnemyWeaponModel[] enemyWeaponModels;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyWeaponModels = GetComponentsInChildren<EnemyWeaponModel>(true);

        RandomModel();
    }

    private void Start()
    {
        ShowWeaponModel();
    }

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

    private void ShowWeaponModel()
    {
        foreach (var model in enemyWeaponModels)
        {
            if (model.weaponModel == enemyWeaponModel)
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
