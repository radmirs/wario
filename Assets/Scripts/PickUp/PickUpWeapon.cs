using UnityEngine;
using wario.Shooting;

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
       }
    }
}