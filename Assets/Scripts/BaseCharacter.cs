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
        public bool IsBoosted {get ; set; }                                 //true когда игрок нажимает кнопку ускорения

        private List<BaseBuff> _buffList = new List<BaseBuff>();            // Список бафффов
        private List<BaseBuff> _buffListRemoveQueue = new List<BaseBuff>(); // Очередь баффов на удаление
        private List<BaseBuff> _buffListAddQueue = new List<BaseBuff>();    // Очередь на добавление


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

            
            //************** БЛОК РАБОТЫ С БАФФАМИ ПЕРСОНАЖА **************
            foreach(BaseBuff buff in _buffListRemoveQueue)  // Удаляем баффы в очереди из списка
            {
                buff.OnRemoval();
                _buffList.Remove(buff);
            }
            foreach(BaseBuff buff in _buffListAddQueue)     // Добавляем баффы в очереди в список
            {
                _buffList.Add(buff);
                buff.OnAddition();
            }
            _buffListAddQueue.Clear();
            _buffListRemoveQueue.Clear();                   // Чистим очереди
            foreach(BaseBuff buff in _buffList)
            {
                buff.TimerIncrement(Time.deltaTime);        // Увеличиваем таймер каждого баффа
                buff.Execute();                             // Бафф проверяет, не закончился ли он
            }           
            //************** КОНЕЦ БЛОКА **************

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
                if (item.bufftype == buff.bufftype)         // Проверяем наличия баффа в списке баффов
                {
                    _buffListRemoveQueue.Add(item);         //Если уже есть такой, добавляем его в очередь на удаление
                }
            }

            _buffListAddQueue.Add(buff);                    // Добавляем бафф в очередь на добавление
        }

        public void RemoveBuff(BaseBuff buff)
        {
            _buffListRemoveQueue.Add(buff);                 // Добавляем бафф в очередь на удаление
        }

    }

}