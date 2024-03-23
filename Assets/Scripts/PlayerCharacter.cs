using UnityEngine;
using wario.Movement;

namespace wario
{
    [RequireComponent(typeof(PlayerMovementDirectionController))]
    public class PlayerCharacter : BaseCharacter
    {
        public override void Die(BaseCharacter character)
        {
            base.Die(this);
        }
    }

}