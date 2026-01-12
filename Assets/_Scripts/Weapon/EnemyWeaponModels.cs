using UnityEngine;

public enum EnemyWeaponModels
{
    OneHandedSword,
    OneHandedAxe,
    TwoHandedSword,
    TwoHandedAxe,
    Bow,
    Staff,
}

public enum EnemyWeaponTypes
{
    Melee,
    Ranged,
    Magic
}

public class EnemyWeaponModel : MonoBehaviour
{
    public EnemyWeaponModels weaponModel;
    public EnemyWeaponTypes weaponType;

    public TrailRenderer trailRenderer;
}

