using UnityEngine;
using wario.Shooting;

namespace wario.PickUp
{
    public class PickUpWeapon : MonoBehaviour
    {
       [field: SerializeField]
       public Weapon WeaponPrefab { get; private set; }
    }
}