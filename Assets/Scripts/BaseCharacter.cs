using UnityEngine;
using wario.Movement;
using wario.Shooting;
using wario.PickUp;
using wario.Buff;
using System;
using System.Collections.Generic;

namespace wario
{
    [RequireComponent(typeof(CharacterMovementController), typeof(ShootingController))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        [SerializeField]
        private Weapon _baseWeaponPrefab;
        [SerializeField]
        private Transform _hand;
        [SerializeField]
        private float _health = 2f;
        private IMovementDirectionSource _movementDirectionSource;
        private CharacterMovementController _characterMovementController;
        private ShootingController _shootingController;
        public bool IsBoosted {get ; set; }

        private List<BaseBuff> _buffList = new List<BaseBuff>();
        private List<BaseBuff> _buffListRemoveQueue = new List<BaseBuff>();
        private List<BaseBuff> _buffListAddQueue = new List<BaseBuff>();


        protected void Awake()
        {
            _movementDirectionSource = GetComponent<IMovementDirectionSource>();

            _characterMovementController = GetComponent<CharacterMovementController>();
            _shootingController = GetComponent<ShootingController>();
        }

        protected void Start()
        {
            SetWeapon(_baseWeaponPrefab);
        }

        protected void Update()
        {
            var direction = _movementDirectionSource.MovementDirection;
            var lookDirection = direction;
            if (_shootingController.HasTarget)
            {
                lookDirection = (_shootingController.TargetPosition - transform.position).normalized;
            }
            
            _characterMovementController.MovementDirection = direction;
            _characterMovementController.LookDirection = lookDirection;

            foreach(BaseBuff buff in _buffListRemoveQueue)
            {
                buff.OnRemoval();
                _buffList.Remove(buff);
            }
            foreach(BaseBuff buff in _buffListAddQueue)
            {
                _buffList.Add(buff);
                buff.OnAddition();
            }
            foreach(BaseBuff buff in _buffList)
            {
                buff.TimerIncrement(Time.deltaTime);
                buff.Execute();
            }
            
            _buffListAddQueue.Clear();
            _buffListRemoveQueue.Clear();

            if (_health <= 0f)
            {
                Destroy(gameObject);
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (LayerUtils.IsBullet(other.gameObject))
            {
                var bullet = other.gameObject.GetComponent<Bullet>();
                _health -= bullet.Damage;

                Destroy(other.gameObject);
            }
            else if (LayerUtils.IsPickUp(other.gameObject))
            {
                var pickUp = other.gameObject.GetComponent<PickUpItem>();   
                pickUp.PickUp(this);

                Destroy(other.gameObject);
            }

        }

        public void SetWeapon(Weapon weapon)
        {
            _shootingController.SetWeapon(weapon, _hand);
        }
        
        public void SetBuff(BaseBuff buff)
        {
            foreach(BaseBuff item in _buffList)
            {
                if (item.bufftype == buff.bufftype)
                {
                    _buffListRemoveQueue.Add(item);
                }
            }

            _buffListAddQueue.Add(buff);
        }

        public void RemoveBuff(BaseBuff buff)
        {
            _buffListRemoveQueue.Add(buff);
        }

    }

}