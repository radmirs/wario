using UnityEngine;
using wario.Shooting;
using wario.Enemy;

namespace wario.PickUp
{
    public class PickUpWeapon : PickUpItem
    {
       [SerializeField]
       public Weapon _weaponPrefab;

       public override void PickUp(BaseCharacter character)
       {
        base.PickUp(character);
        character.SetWeapon(_weaponPrefab);
        if (character.gameObject.GetComponent<EnemyAiController>() != null)     // Проверка, является ли персонаж врагом
        {
            var ai = character.gameObject.GetComponent<EnemyAiController>();
            ai.SetIsWeaponUpgraded();                                           // Даём врагу знать, что он улучшил оружие
        }
       }
    }
}