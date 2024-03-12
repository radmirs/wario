using UnityEngine;

namespace wario.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovementController : MonoBehaviour
    {
        private static readonly float SqrEpsilon = Mathf.Epsilon * Mathf.Epsilon;

        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private float _maxRadiansDelta = 10f;
        [SerializeField]
        private float _boost = 2f;
        private float _currentBuffTimerSeconds = 0f;
        private float _buffDurationSeconds = 0f;
        private float _buffSpeedMultiplier = 1f;
        private bool IsSpeedBuffed = false;

        public Vector3 MovementDirection {get; set; }
        public Vector3 LookDirection {get; set; }
        public bool IsBoosted {get ; set; }
        
        private CharacterController _characterController;
        
        protected void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }
        
        protected void Update()
        {
            Translate();

            if (_maxRadiansDelta > 0f && LookDirection != Vector3.zero)
            {
                Rotate();
            }

            if (IsSpeedBuffed)
            {
                _currentBuffTimerSeconds += Time.deltaTime;
                if (_currentBuffTimerSeconds > _buffDurationSeconds)
                    {
                        _buffSpeedMultiplier = 1f;
                        _currentBuffTimerSeconds = 0f;
                        _buffDurationSeconds = 0f;
                        IsSpeedBuffed = false;
                    }

            }
            
        }

        private void Translate()
        {
            var delta = MovementDirection * _speed * _buffSpeedMultiplier * Time.deltaTime;
            if (IsBoosted)
            {
               delta *= _boost;
            }
            _characterController.Move(delta); 
        }

        private void Rotate()
        {
            var currentLookDirection = transform.rotation * Vector3.forward;
            float sqrMagnitude = (currentLookDirection - LookDirection).sqrMagnitude;

            if (sqrMagnitude > SqrEpsilon)
            {
                var newRotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(LookDirection, Vector3.up),
                _maxRadiansDelta * Time.deltaTime);
                
                transform.rotation = newRotation;
            }
        }

        public void SetBuffSpeed(float buffTime, float buffMultiplier)
        {
            _buffSpeedMultiplier = buffMultiplier;
            _buffDurationSeconds = buffTime;
            _currentBuffTimerSeconds = 0f;
            IsSpeedBuffed = true;
        }
    }
}