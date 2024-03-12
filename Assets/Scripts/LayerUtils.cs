using UnityEngine;

namespace wario
{
    public static class LayerUtils
    {
        public const string PlayerLayerName = "Player";
        public const string BulletLayerName = "Bullet";
        public const string EnemyLayerName = "Enemy";
        public const string PickUpWeaponLayerName = "PickUpWeapon";
        public const string PickUpBoosterLayerName = "PickUpBooster";

        //public static readonly int PlayerLayer = LayerMask.NameToLayer(PlayerLayerName);
        public static readonly int BulletLayer = LayerMask.NameToLayer(BulletLayerName);
        public static readonly int PickUpWeaponLayer = LayerMask.NameToLayer(PickUpWeaponLayerName);
        public static readonly int PickUpBoosterLayer = LayerMask.NameToLayer(PickUpBoosterLayerName);
        public static readonly int EnemyMask = LayerMask.GetMask(EnemyLayerName, PlayerLayerName);

        public static bool IsBullet(GameObject other) => other.layer == BulletLayer;
        public static bool IsPickUpWeapon(GameObject other) => other.layer == PickUpWeaponLayer;
        public static bool IsPickUpBooster(GameObject other) => other.layer == PickUpBoosterLayer;
        
    }

}