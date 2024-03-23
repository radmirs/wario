using UnityEngine;
using wario.Enemy.States;

namespace wario.Enemy
{
    public class EnemyAiController : MonoBehaviour
    {
        [SerializeField]
        private float _viewRadius = 20f;
        [SerializeField]
        private float _escapeTriggerHPTreshold = 0.3f;
        [SerializeField]
        private float _escapeTriggerProbability = 0.9f;
        [SerializeField]
        private float _scareSpeedBuff = 1.1f;
        private EnemyTarget _target;
        private EnemyStateMachine _stateMachine;
        private bool IsPlayer = false; // Есть ли игрок
        private PlayerCharacter _player;

        protected void Awake()
        {
            
            var enemyDirectionController = GetComponent<EnemyDirectionController>();
            var navMesher = new NavMesher(transform);
            var navMesherEscape = new NavMesher(transform); // Отдельный навмешер для поиска пути к побегу

            _target = new EnemyTarget(transform, _viewRadius);
            _stateMachine = new EnemyStateMachine(enemyDirectionController, navMesher, _target, navMesherEscape);
            
            _player = GameObject.FindObjectOfType<PlayerCharacter>(); // Поиск игрока
            if ( _player != null )
                {
                    _player.OnDeath += OnPlayerDied;    // Подписываемся на событие смерти игрока
                    IsPlayer = true;                    // Игрок есть
                }
        }

        protected void Update()
        {
            Debug.Log(IsPlayer.ToString());
            if ( IsPlayer == true ) // Если есть игрок
            {
                _target.CheckForEscape(_escapeTriggerHPTreshold, _escapeTriggerProbability, _scareSpeedBuff); // Проверка условий побега
                _target.FindClosest();
                _stateMachine.Update();
            }
            else // Если игрока нет
            {
                _player = GameObject.FindObjectOfType<PlayerCharacter>();   // Ищем игрока
                if ( _player != null )                                      // Если нашли
                {
                    _target.UpdatePlayer(_player);                          // Заменяем игрока у EnemyTarget
                    _player.OnDeath += OnPlayerDied;                // Подписываемся на событие смерти игрока
                    IsPlayer = true;                                // Игрок есть
                }
            }
        }

        public void SetIsWeaponUpgraded()
        {
            _target.IsWeaponUpgraded = true; // Запоминаем, что оружие улучшено
        }

        public void OnPlayerDied(BaseCharacter character) // При смерти игрока
        {
            _player.OnDeath -= OnPlayerDied;    // Отписываемся от события
            IsPlayer = false;                   // Игрока нет
            _player = null;                     // Игрока нет
        }
    }

}