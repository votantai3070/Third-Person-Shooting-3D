using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [SerializeField] EnemyWeaponModels enemyWeaponModel;
    [HideInInspector] public EnemyIdleType idleType;

    private EnemyWeaponModel[] enemyWeaponModels;

    private void Awake()
    {
        enemyWeaponModels = GetComponentsInChildren<EnemyWeaponModel>(true);
    }

    private void Start()
    {
        ShowWeaponModel();
    }

    private void OnValidate()
    {
        switch (enemyWeaponModel)
        {
            case EnemyWeaponModels.OneHandedSword:
                idleType = EnemyIdleType.OneHand_MeleeIdle;
                break;
            case EnemyWeaponModels.OneHandedAxe:
                idleType = EnemyIdleType.OneHand_MeleeIdle;
                break;
            case EnemyWeaponModels.TwoHandedSword:
                idleType = EnemyIdleType.TwoHand_MeleeIdle;
                break;
            case EnemyWeaponModels.TwoHandedAxe:
                idleType = EnemyIdleType.TwoHand_MeleeIdle;
                break;
        }
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
