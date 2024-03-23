using UnityEngine;
using wario.Enemy;

namespace wario
{ 
    public class CommonSpace : MonoBehaviour
    {
        // Данный класс предназначен для хранения данных о количестве игроков и врагов на уровне
        private int _playerCapacity = 1;
        [SerializeField]
        private int _enemyCapacity = 1;
        
        public int _currentPlayerCount = 0;
        public int _currentEnemyCount = 0;
        public bool IsPlayerNeeded = true;
        public bool IsEnemyNeeded = true;

        protected void Start()
        {
        
        }

        protected void Update()
        {
        
        }

        public void AddPlayer() // Увеличиваем счётчик игроков
        {
            _currentPlayerCount++;
            IsPlayerNeeded = false;
        }

        public void AddEnemy() // Увеличиваем счетчик врагов
        {
            _currentEnemyCount++;
            if ( _currentEnemyCount >= _enemyCapacity )
            {
                IsEnemyNeeded = false;
            }
        }
        
        public void Decount(BaseCharacter character) // Уменьшаем количество врагов или игроков
        {
            if (character.gameObject.GetComponent<PlayerCharacter>() != null)
            {
                _currentPlayerCount--;
                IsPlayerNeeded = true;
            }
            else if (character.gameObject.GetComponent<EnemyCharacter>() != null)
            {
                _currentEnemyCount--;
                if ( _currentEnemyCount < _enemyCapacity )
            {
                IsEnemyNeeded = true;
            }
            }
        }
    }
}