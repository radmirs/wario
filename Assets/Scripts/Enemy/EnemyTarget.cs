using UnityEngine;
using wario.Movement;

namespace wario.Enemy
{
    
    public class EnemyTarget
    {
        public GameObject Closest { get; private set; }
        public GameObject ClosestEnemy { get; private set; }
        public bool IsScared { get; private set; } // Испуган ли персонаж
        public bool IsWeaponUpgraded { get; set; } // Улучшено ли оружие

        private readonly Transform _agentTransform;
        private readonly float _viewRadius;
        //private PlayerCharacter _player;
        private CharacterMovementController _movementController;
        private BaseCharacter _character;

        private readonly Collider[] _colliders = new Collider[10];

        public EnemyTarget(Transform agent, float viewRadius)
        {
            _agentTransform = agent;
            //_player = GameObject.FindObjectOfType<PlayerCharacter>();
            _viewRadius = viewRadius;

            _movementController = 
            _agentTransform.GetComponent<CharacterMovementController>();

            _character = _agentTransform.GetComponent<BaseCharacter>();  
        }

       /* public void UpdatePlayer(PlayerCharacter character) // Переназначаем игрока
        {
            _player = character;
        } */

        
        public void FindClosest()
        {
            float minDistance = float.MaxValue;

            var count = FindAllTargets(LayerUtils.PickUpsMask | LayerUtils.EnemyMask);

            for (int i = 0; i < count; i++)
            {
                var go = _colliders[i].gameObject;
                if (go == _agentTransform.gameObject) continue;

                var distance = DistanceFromAgentTo(go);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    Closest = go;
                }
            }
            if (Closest == null)
            {
                Closest = _agentTransform.gameObject;
            }

           
        }
        
        public float DistanceToClosestFromAgent()
        {
            if (Closest != null)
            {
                DistanceFromAgentTo(Closest);
            }

            return 0;
        }

        public void CheckForEscape(float escapeTriggerHPTreshold, float escapeTriggerProbability, float scareSpeedBuff)  // Проверка на побег
        {
            var (maxHealth, health) = _character.CheckHealth();                  // Узнаём текущее и максимальное здоровье
            if ( health / maxHealth <= escapeTriggerHPTreshold)                 // Если доля хп меньше пороговой
            {
                float diceRoll = Random.Range(0f, 1f);                          // Выбираем случайное число
                if ( escapeTriggerProbability <= diceRoll)                      // Если вероятность побега меньше числа
                {
                    IsScared = true;                                            // Пугаемся
                    _movementController.BuffSpeed(scareSpeedBuff);               // Баффаем скорость
                }
            }

            if (ClosestEnemy != null)
            {
                Vector3 enemyPosition = ClosestEnemy.transform.position;
                Vector3 position = _agentTransform.position;
                if ( ( enemyPosition - position ).magnitude > _viewRadius)         // Если враг вне зоны видимости
                {
                    IsScared = false;                                               // Больше не боимся
                    _movementController.BuffSpeed(1f);                               // Возвращаем нормальную скорость
                }    
            }
        }
        
        public Vector3 FindEscape() // Используется в EnemyAiController
        {
            float minDistance = float.MaxValue;

            var count = FindAllTargets(LayerUtils.EnemyMask);

            for (int i = 0; i < count; i++)
            {
                var go = _colliders[i].gameObject;
                if (go == _agentTransform.gameObject) continue;

                var distance = DistanceFromAgentTo(go);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    ClosestEnemy = go;
                }
            }
            
            Vector3 escape;
            if (ClosestEnemy != null)
            {
                Vector3 enemyPosition = ClosestEnemy.transform.position;                           // Позиция врага
                Vector3 position = _agentTransform.position;                                        // Собственная позиция
                escape = position + (position - enemyPosition).normalized * _viewRadius;   // Ищем дальнейшую точку от врага в зоне видимости
            }
            else 
            {
                escape = _agentTransform.position;
            }
            return escape;
        }

        private int FindAllTargets(int layerMask)
        {
            var size = Physics.OverlapSphereNonAlloc(
                _agentTransform.position,
                _viewRadius,
                _colliders,
                layerMask );
            
            return size;
        }

        private float DistanceFromAgentTo(GameObject go) => (_agentTransform.position - go.transform.position).magnitude;
    }

}