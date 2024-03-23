using UnityEngine;
using wario.FSM;

namespace wario.Enemy.States
{
    public class MoveForwardState : BaseState
    {
        private readonly EnemyTarget _target;
        private readonly EnemyDirectionController _enemyDirectionController;
        
        private Vector3 _currentPoint;

        public MoveForwardState(EnemyTarget target, EnemyDirectionController enemyDirectionController)
        {
            _target = target;
            _enemyDirectionController = enemyDirectionController;
        }
        public override void Execute()
        {
            //Debug.Log("MoveForward");
            Vector3 targetPosition = _target.Closest.transform.position;

            if (_currentPoint != targetPosition)
            {
                _currentPoint = targetPosition;
                _enemyDirectionController.UpdateMovementDirection(targetPosition);
            }
        }
    }
}