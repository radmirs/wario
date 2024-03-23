using UnityEngine;
using wario.FSM;
using wario.Enemy;

namespace wario.Enemy.States
{
    public class FindWayState : BaseState
    {
        private const float MaxDistaceBetweenRealAndCalculated = 3f;
        
        private readonly EnemyTarget _target;
        private readonly NavMesher _navMesher;
        private readonly EnemyDirectionController _enemyDirectionController;

        private Vector3 _currentPoint;

        public FindWayState(EnemyTarget target, NavMesher navMesher, EnemyDirectionController enemyDirectionController)
        {
            _target = target;
            _navMesher = navMesher;
            _enemyDirectionController = enemyDirectionController;
        }

        public override void Execute()
        {
            //Debug.Log("FindWay");
            Vector3 targetPosition = _target.Closest.transform.position;

            if (!_navMesher.IsPathCalculated  || _navMesher.DistaceToTargetPointFrom(targetPosition) > MaxDistaceBetweenRealAndCalculated)
            {
                _navMesher.CalculatePath(targetPosition);
            }

            var currentPoint = _navMesher.GetCurrentPoint();
            if (_currentPoint != currentPoint)
            {
                _currentPoint = currentPoint;
                _enemyDirectionController.UpdateMovementDirection(currentPoint);
            }
        }
    }
}