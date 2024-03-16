using System.Collections.Generic;
using wario.FSM;

namespace wario.Enemy.States
{ 
    public class EnemyStateMachine : BaseStateMachine
    {
        private const float NavMeshTurnOffDistance = 5f;
        public EnemyStateMachine(EnemyDirectionController enemyDirectionController, NavMesher navMesher, EnemyTarget target)
        {
            var idleState = new IdleState();
            var findWayState = new FindWayState(target, navMesher, enemyDirectionController);
            var moveForwardState = new MoveForwardState(target, enemyDirectionController);

            SetInitialState(idleState);

            AddState(state: idleState, transitions: new List<Transition>
            {
                new Transition(
                    findWayState,
                    () => target.DistanceToClosestFromAgent() > NavMeshTurnOffDistance ),
                new Transition(
                    moveForwardState,
                    () => target.DistanceToClosestFromAgent() <= NavMeshTurnOffDistance )
            } 
            );

            AddState(state: findWayState, transitions: new List<Transition>
            {
                new Transition(
                    idleState,
                    () => target.Closest == null ),
                new Transition(
                    moveForwardState,
                    () => target.DistanceToClosestFromAgent() <= NavMeshTurnOffDistance )
            } 
            );

            AddState(state: moveForwardState, transitions: new List<Transition>
            {
                new Transition(
                    idleState,
                    () => target.Closest == null ),
                new Transition(
                    moveForwardState,
                    () => target.DistanceToClosestFromAgent() <= NavMeshTurnOffDistance )
            } 
            );

        }
        
    }

}