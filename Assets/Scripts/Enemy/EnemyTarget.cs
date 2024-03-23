using UnityEngine;
using wario.Movement;

namespace wario.Enemy
{
    
    public class EnemyTarget
    {
        public GameObject Closest { get; private set; }
        public bool IsScared { get; private set; } // Испуган ли персонаж
        public bool IsWeaponUpgraded { get; set; } // Улучшено ли оружие

        private readonly Transform _agentTransform;
        private readonly float _viewRadius;
        private PlayerCharacter _player;

        private readonly Collider[] _colliders = new Collider[10];

        public EnemyTarget(Transform agent, float viewRadius)
        {
            _agentTransform = agent;
            _player = GameObject.FindObjectOfType<PlayerCharacter>();
            _viewRadius = viewRadius;
        }

        public void UpdatePlayer(PlayerCharacter character) // Переназначаем игрока
        {
            _player = character;
        }

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

            if (_player != null && ( DistanceFromAgentTo(_player.gameObject) < minDistance ) || IsWeaponUpgraded ) // В случае, если оружие улучшено, ближайшей целью считается игрок
            {
                Closest = _player.gameObject;
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
            var character = _agentTransform.GetComponent<BaseCharacter>();
            var (maxHealth, health) = character.CheckHealth();                  // Узнаём текущее и максимальное здоровье
            var movementController = 
            _agentTransform.GetComponent<CharacterMovementController>();        // Получаем чтобы баффнуть скорость
            if ( health / maxHealth <= escapeTriggerHPTreshold)                 // Если доля хп меньше пороговой
            {
                float diceRoll = Random.Range(0f, 1f);                          // Выбираем случайное число
                if ( escapeTriggerProbability <= diceRoll)                      // Если вероятность побега меньше числа
                {
                    IsScared = true;                                            // Пугаемся
                    movementController.BuffSpeed(scareSpeedBuff);               // Баффаем скорость
                }
            }

            Vector3 playerPosition = _player.gameObject.transform.position;
            Vector3 position = _agentTransform.position;
            if ( ( playerPosition - position ).magnitude > _viewRadius)         // Если игрок вне зоны видимости
            {
                IsScared = false;                                               // Больше не боимся
                movementController.BuffSpeed(1f);                               // Возвращаем нормальную скорость
            }    
        }
        
        public Vector3 FindEscape() // Используется в EnemyAiController
        {
            Vector3 playerPosition = _player.gameObject.transform.position;                     // Позиция игрока
            Vector3 position = _agentTransform.position;                                        // Собственная позиция
            Vector3 escape = position + (position - playerPosition).normalized * _viewRadius;   // Ищем дальнейшую точку от игрока в зоне видимости
            
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