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
    }

    private void Start()
    {
        ShowWeaponModel();
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
