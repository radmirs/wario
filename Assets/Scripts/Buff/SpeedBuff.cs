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
        
        public override void Execute()                                  //Вызывается при каждом Update() из BaseCharacter
        {
            if (_currentBuffTimerSeconds > _buffDurationSeconds)
                    {
                        _character.RemoveBuff(this);                    //Добавляется в очередь на удаление BaseCharacter'а
                    }

        }
        
        public override void OnRemoval()                                //Вызывается при удалении баффа
        {
            _characterMovementController.BuffSpeed(1f);
        }

        public override void OnAddition()                               //Вызывается при добавлении баффа
        {
            _characterMovementController.BuffSpeed(_speedMultiplier);
        }

    }
}