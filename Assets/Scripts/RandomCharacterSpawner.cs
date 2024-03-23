using UnityEditor;
using UnityEngine;

namespace wario
{ 
    public class RandomCharacterSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _level;
        private CommonSpace _commonSpace;
        [SerializeField]
        private BaseCharacter _playerPrefab;
        [SerializeField]
        private BaseCharacter _enemyPrefab;
        [SerializeField]
        private float _range = 2f;
        [SerializeField]
        private int _maxCount = 1;
        [SerializeField]
        private float _minSpawnIntervalSeconds = 9f;
        [SerializeField]
        private float _maxSpawnIntervalSeconds = 4f;
        [SerializeField]
        private float _spawnIntervalSeconds = 0f;
        private float _currentSpawnTimerSeconds;
        private int _currentCount;

        protected void Start()
        {
            _commonSpace = _level.GetComponent<CommonSpace>(); // Получаем ссылку на счетчик персонажей
            
            if (_minSpawnIntervalSeconds > _maxSpawnIntervalSeconds)
            {
                Debug.Log("Minimal spawn interval should be less than maximal spawn interval. Values have been swaped.");
                float b = _minSpawnIntervalSeconds;
                _minSpawnIntervalSeconds = _maxSpawnIntervalSeconds;
                _maxSpawnIntervalSeconds = b;
            }

            _spawnIntervalSeconds = Random.Range(_minSpawnIntervalSeconds, _maxSpawnIntervalSeconds);
            // До сюда всё как в спаунере предметов
        }

        protected void Update()
        {
            if (_currentCount < _maxCount)
            {
                _currentSpawnTimerSeconds += Time.deltaTime;
                
                if (_currentSpawnTimerSeconds > _spawnIntervalSeconds)
                {
                    if (_commonSpace.IsPlayerNeeded) // Если нужен игрок, спауним игрока
                    {
                        var randomPointInsideRange = Random.insideUnitCircle * _range;
                        var randomPosition = new Vector3(randomPointInsideRange.x, 0f, randomPointInsideRange.y) + transform.position;

                        var character = Instantiate(_playerPrefab, randomPosition, Quaternion.identity, transform);
                        character.OnDeath += OnCharacterDied;
                        _commonSpace.AddPlayer();
                    }
                    else if (_commonSpace.IsEnemyNeeded) // Если нужен враг, спауним врага
                    {
                        var randomPointInsideRange = Random.insideUnitCircle * _range;
                        var randomPosition = new Vector3(randomPointInsideRange.x, 0f, randomPointInsideRange.y) + transform.position;

                        var character = Instantiate(_enemyPrefab, randomPosition, Quaternion.identity, transform);
                        character.OnDeath += OnCharacterDied;
                        _commonSpace.AddEnemy();
                    }
                    
                    _currentSpawnTimerSeconds = 0f;
                    _spawnIntervalSeconds = Random.Range(_minSpawnIntervalSeconds, _maxSpawnIntervalSeconds);

                   
                }
            }

        }

        protected void OnCharacterDied(BaseCharacter character)
        {
            _commonSpace.Decount(character);
            character.OnDeath -= OnCharacterDied;
        }

        protected void OnDrawGizmos()
        {
            var cashedColor = Handles.color;
            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, Vector3.up, _range);
            Handles.color = cashedColor;
        }
    }
}