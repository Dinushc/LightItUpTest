using System.Collections.Generic;
using _Game.Scripts.Game.SeekingMissileService.MissileComponents;
using LightItUp.Data;
using LightItUp.Game;
using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissileService
{
    public class SeekingMissile : PooledObject
    {
        private SeekingMissilesController _seekingMissilesController;
        private MissileMovement _movement;
        private MissileTargeting _targeting;
        private MissileCollisionHandler _collisionHandler;
        private MissileRenderer _renderer;
        private bool _returned = false;
        private Vector2 _currentDirection;
        private float _turnSpeed = 15;
        private float _speed = 0;
        
        [SerializeField] private LayerMask _blockLayer;
        [SerializeField] private LayerMask _blockLayerLit;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private Collider2D _collider2D;

        public bool returned => _returned;

        public void Init(SeekingMissilesController seekingMissilesController, List<BlockController> targets, MissileServiceConfig config)
        {
            _seekingMissilesController = seekingMissilesController;

            _movement = new MissileMovement(transform, config.missileSpeed, config.avoidObstacles);
            _speed = config.missileSpeed;
            _targeting = new MissileTargeting();
            _collisionHandler = gameObject.AddComponent<MissileCollisionHandler>();
            _collisionHandler.Init(_seekingMissilesController, _collider2D, _targeting);
            _renderer = new MissileRenderer(_spriteRenderer, _trailRenderer);

            _targeting.AssignTargets(targets);
            _renderer.ClearTrail();
            
            _movement.Setup(_blockLayer, _blockLayerLit, _targeting);
            _currentDirection = (_targeting.GetCurrentTarget().transform.position - transform.position).normalized;
            _seekingMissilesController.SetLayers(_blockLayer, _blockLayerLit);
        }

        private void Update()
        {
            var target = _targeting.GetCurrentTarget();
            if (target.IsLit)
            {
                target = _targeting.TryGetNextTarget();
                if (target == null)
                {
                    return;
                }
            }
            
            var directionToTarget = (target.transform.position - transform.position).normalized;
            var avoidDirection = _movement.ComputeObstacleAvoidance(transform.position, directionToTarget);
            _currentDirection = Vector3.Slerp(_currentDirection, avoidDirection, _turnSpeed * Time.deltaTime).normalized;
            _movement.Move(_currentDirection);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_collisionHandler == null)
            {
                return;
            }
            _collisionHandler.OnCollide(other, this, ref _currentDirection);
        }

        private void Clear()
        {
            _seekingMissilesController = null;
            _movement = null;
            _targeting = null;
            _collisionHandler = null;
            _renderer = null;
        }

        private void OnDisable()
        {
            Clear();
        }
    }
}
