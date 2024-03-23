using UnityEngine;
using wario.FSM;

namespace wario.Enemy.States
{
    public class EscapeState : BaseState
    {
        private const float MaxDistaceBetweenRealAndCalculated = 3f;
        private readonly EnemyTarget _target;
        private readonly NavMesher _navMesher;
        private readonly EnemyDirectionController _enemyDirectionController;
        
        private Vector3 _currentPoint;

        public EscapeState(EnemyTarget target, NavMesher navMesher, EnemyDirectionController enemyDirectionController)
        {
            _target = target;
            _navMesher = navMesher;
            _enemyDirectionController = enemyDirectionController;
        }
        public override void Execute()
        {
            //Debug.Log("Escape");
            Vector3 targetPosition = _target.FindEscape(); // Получаем точку для поиска пути побега

            if (!_navMesher.IsEscapePathCalculated  || _navMesher.DistaceToEscapePointFrom(targetPosition) > MaxDistaceBetweenRealAndCalculated)
            {
                _navMesher.CalculateEscapePath(targetPosition);
            }
            var currentPoint = _navMesher.GetCurrentEscapePoint();
            if (_currentPoint != currentPoint)
            {
                _currentPoint = currentPoint;
                _enemyDirectionController.UpdateMovementDirection(currentPoint);
            }
           // То что снизу вроде не работает, а то что сверху работает, не совсем понял
           /* if (_currentPoint != targetPosition)
            {
                _currentPoint = targetPosition;
                _enemyDirectionController.UpdateMovementDirection(targetPosition);
            } */ 
        }
    }
}



            