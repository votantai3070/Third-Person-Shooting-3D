using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zombie_Attack : MonoBehaviour
{
    private Enemy_Melee enemyMelee;

    private HashSet<GameObject> damagedEntities = new HashSet<GameObject>();

    private void Start()
    {
        enemyMelee = GetComponentInParent<Enemy_Melee>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject root = other.transform.root.gameObject;
        IDamageable damageable = other.GetComponentInParent<IDamageable>();


        if (damageable != null)
        {
            //if (!damagedEntities.Add(root))
            //{
            //    return;
            //}

            damageable.TakeDamage(enemyMelee.damaged);
        }
    }

    public void OnAttackAnimationEnd()
    {
        damagedEntities.Clear();
    }
}

