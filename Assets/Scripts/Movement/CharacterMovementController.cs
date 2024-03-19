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
        private float _buffSpeedMultiplier = 1f;

        public Vector3 MovementDirection {get; set; }
        public Vector3 LookDirection {get; set; }
        
        private CharacterController _characterController;
        private BaseCharacter _controlledCharacter;
        
        protected void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _controlledCharacter = GetComponent<BaseCharacter>();
        }
        
        protected void Update()
        {
            Translate();

            if (_maxRadiansDelta > 0f && LookDirection != Vector3.zero)
            {
                Rotate();
            }

        }

        private void Translate()
        {
            var delta = MovementDirection * _speed * _buffSpeedMultiplier * Time.deltaTime;
            if (_controlledCharacter.IsBoosted)
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

        public void BuffSpeed(float buffSpeedMultiplier)
        {
            _buffSpeedMultiplier = buffSpeedMultiplier;
        }

    }
}