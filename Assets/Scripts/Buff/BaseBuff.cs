using UnityEngine;

namespace wario.Buff
{
    public abstract class BaseBuff
    {
        protected float _currentBuffTimerSeconds = 0f;
        protected float _buffDurationSeconds = 0f;
        public string bufftype { get; protected set; }

        public void TimerSet(float buffDurationSeconds)
        {
            _buffDurationSeconds = buffDurationSeconds;
            _currentBuffTimerSeconds = 0f;
        }

        public void TimerIncrement(float increment)
        {
            _currentBuffTimerSeconds += increment;
        }

        public virtual void Execute()
        {
            
        }

        public virtual void OnRemoval()
        {
            
        }

        public virtual void OnAddition()
        {
            
        }

    }
}