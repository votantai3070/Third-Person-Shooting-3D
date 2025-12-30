using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [SerializeField] EnemyWeaponModels enemyWeaponModel;

    private EnemyWeaponModel[] enemyWeaponModels;

    private void Awake()
    {
        enemyWeaponModels = GetComponentsInChildren<EnemyWeaponModel>(true);
    }

    private void Start()
    {
        ShowWeaponModel();
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
