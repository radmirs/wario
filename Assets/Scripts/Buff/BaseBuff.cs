using UnityEngine;

namespace wario.Buff
{
    public abstract class BaseBuff
    {
        protected float _currentBuffTimerSeconds = 0f;
        protected float _buffDurationSeconds = 0f;
        public string bufftype { get; protected set; } // Нужно для проверки на наличие одинаковых баффов в BaseCharacter

        public void TimerSet(float buffDurationSeconds)
        {
            _buffDurationSeconds = buffDurationSeconds;
            _currentBuffTimerSeconds = 0f;
        }

        public void TimerIncrement(float increment)
        {
            _currentBuffTimerSeconds += increment;
        }

        public virtual void Execute()       //Вызывается при каждом Update() из BaseCharacter
        {}

        public virtual void OnRemoval()     //Вызывается при удалении баффа
        {}

        public virtual void OnAddition()    //Вызывается при добавлении баффа
        {}

    }
}