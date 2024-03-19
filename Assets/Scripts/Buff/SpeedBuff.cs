using UnityEngine;
using wario.Movement;

namespace wario.Buff
{
    public class SpeedBuff : BaseBuff
    {
        private CharacterMovementController _characterMovementController;
        private BaseCharacter _character;
        private float _speedMultiplier;
        
        public SpeedBuff(float boostTimeSeconds, float speedMultiplier, BaseCharacter character)
        {
            _character = character;
            _characterMovementController = _character.GetComponent<CharacterMovementController>();
            bufftype = "speed";
            TimerSet(boostTimeSeconds);
            _speedMultiplier = speedMultiplier;
        }
        public override void Execute()
        {
            if (_currentBuffTimerSeconds > _buffDurationSeconds)
                    {
                        _character.RemoveBuff(this);
                    }

        }
        
        public override void OnRemoval()
        {
            _characterMovementController.BuffSpeed(1f);
        }

        public override void OnAddition()
        {
            _characterMovementController.BuffSpeed(_speedMultiplier);
        }

    }
}