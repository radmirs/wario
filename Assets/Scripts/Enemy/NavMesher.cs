using UnityEngine;
using UnityEngine.AI;

namespace wario.Enemy
{
    
    public class NavMesher
    {
        private const float DistaceEps = 1.5f;

        public bool IsPathCalculated { get; private set; }
        public bool IsEscapePathCalculated { get; private set; }

        private readonly NavMeshQueryFilter _filter;
        private readonly Transform _agentTransform;
        
        private NavMeshPath _navMeshPath;
        private NavMeshPath _navMeshEscapePath;
        private NavMeshHit _targetHit;
        private NavMeshHit _targetEscapeHit;
        private int _currentPathPointIndex;
        private int _currentEscapePathPointIndex;
        public NavMesher(Transform agentTransform)
        {
            _filter = new NavMeshQueryFilter { areaMask = NavMesh.AllAreas };
            IsPathCalculated = false;
            IsEscapePathCalculated = false;
            _navMeshEscapePath = new NavMeshPath();

            _agentTransform = agentTransform;
        }
        
        public void CalculatePath(Vector3 targetPosition)
        {
            NavMesh.SamplePosition(_agentTransform.position, out var agentHit, 10f, _filter);
            NavMesh.SamplePosition(targetPosition, out var _targetHit, 10f, _filter);

            IsPathCalculated = NavMesh.CalculatePath(agentHit.position, _targetHit.position, _filter, _navMeshPath);
            _currentPathPointIndex = 0;
        }

        public void CalculateEscapePath(Vector3 targetPosition)
        {
            NavMesh.SamplePosition(_agentTransform.position, out var agentHit, 10f, _filter);
            NavMesh.SamplePosition(targetPosition, out var _targetEscapeHit, 10f, _filter);

            IsEscapePathCalculated = NavMesh.CalculatePath(agentHit.position, _targetEscapeHit.position, _filter, _navMeshEscapePath);
            _currentEscapePathPointIndex = 0;
        }

        public Vector3 GetCurrentPoint()
        {
            var currentPoint = _navMeshPath.corners[_currentPathPointIndex];
            var distance = (_agentTransform.position - currentPoint).magnitude;

            if (distance < DistaceEps)
            {
                _currentPathPointIndex++;
            }

            if (_currentPathPointIndex >= _navMeshPath.corners.Length)
            {
                IsPathCalculated = false;
            }
            else
            {
                currentPoint = _navMeshPath.corners[_currentPathPointIndex];
            }

            return currentPoint;
        }

        public Vector3 GetCurrentEscapePoint()
        {
            var currentPoint = _navMeshEscapePath.corners[_currentEscapePathPointIndex];
            var distance = (_agentTransform.position - currentPoint).magnitude;

            if (distance < DistaceEps)
            {
                _currentEscapePathPointIndex++;
            }

            if (_currentEscapePathPointIndex >= _navMeshEscapePath.corners.Length)
            {
                IsEscapePathCalculated = false;
            }
            else
            {
                currentPoint = _navMeshEscapePath.corners[_currentEscapePathPointIndex];
            }

            return currentPoint;
        }
        
        public float DistaceToTargetPointFrom(Vector3 position) => (_targetHit.position - position).magnitude;
        public float DistaceToEscapePointFrom(Vector3 position) => (_targetEscapeHit.position - position).magnitude;
    }

}