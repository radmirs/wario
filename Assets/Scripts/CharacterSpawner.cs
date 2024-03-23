using UnityEditor;
using UnityEngine;

namespace wario
{ 
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField]
        private BaseCharacter _characterPrefab;
        [SerializeField]
        private float _range = 2f;
        [SerializeField]
        private int _maxCount = 1;
        [SerializeField]
        private float _minSpawnIntervalSeconds = 9f;
        [SerializeField]
        private float _maxSpawnIntervalSeconds = 11f;
        [SerializeField]
        private float _spawnIntervalSeconds = 10f;
        private float _currentSpawnTimerSeconds;
        private int _currentCount;

        // vvv Тут всё переписал со спаунера пикапов vvv
        protected void Start()
        {
            if (_minSpawnIntervalSeconds > _maxSpawnIntervalSeconds)
            {
                Debug.Log("Minimal spawn interval should be less than maximal spawn interval. Values have been swaped.");
                float b = _minSpawnIntervalSeconds;
                _minSpawnIntervalSeconds = _maxSpawnIntervalSeconds;
                _maxSpawnIntervalSeconds = b;
            }

            _spawnIntervalSeconds = Random.Range(_minSpawnIntervalSeconds, _maxSpawnIntervalSeconds);

        }

        protected void Update()
        {
            if (_currentCount < _maxCount)
            {
                _currentSpawnTimerSeconds += Time.deltaTime;
                if (_currentSpawnTimerSeconds > _spawnIntervalSeconds)
                {
                    _currentSpawnTimerSeconds = 0f;
                    _currentCount++;
                    _spawnIntervalSeconds = Random.Range(_minSpawnIntervalSeconds, _maxSpawnIntervalSeconds);

                    var randomPointInsideRange = Random.insideUnitCircle * _range;
                    var randomPosition = new Vector3(randomPointInsideRange.x, 0f, randomPointInsideRange.y) + transform.position;

                    var character = Instantiate(_characterPrefab, randomPosition, Quaternion.identity, transform);
                    character.OnDeath += OnCharacterDied;
                }
            }

        }

        protected void OnCharacterDied(BaseCharacter character)
        {
            _currentCount--;
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