using UnityEngine;
using wario.Shooting;

namespace wario.PickUp
{
    public class PickUpBooster : PickUpItem
    {
       [SerializeField]
       private float _boostTimeSeconds = 10f;
       [SerializeField]
       private float _speedMultiplier = 2f;

       public override void PickUp(BaseCharacter character)
       {
        base.PickUp(character);
        character.SetBuff("speed", _boostTimeSeconds, _speedMultiplier);
       }
    }
}