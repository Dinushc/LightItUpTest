using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissileService.MissileComponents
{
    public class MissileMovement
    {
        private float _speed;
        private Transform _transform;
        private bool _avoidObstacles = false;
        private int _rayCount = 36;
        private Vector2 currentDirection;
        private MissileTargeting _targeting;
        private LayerMask _blockLayer;
        private LayerMask _blockLayerLit;
        private int _turnFactor = 0;

        public MissileMovement(Transform transform, float speed, bool avoidObstacles)
        {
            _transform = transform;
            _speed = speed;
            _avoidObstacles = avoidObstacles;
        }

        public void Setup(LayerMask blockLayer, LayerMask blockLayerLit, MissileTargeting targeting)
        {
            _blockLayer = blockLayer;
            _blockLayerLit = blockLayerLit;
            _targeting = targeting;
        }

        public void Move(Vector3 direction)
        {
            _transform.position += direction * _speed * Time.deltaTime;
        }
        
        public Vector3 ComputeObstacleAvoidance(Vector3 currentPosition, Vector3 directionToTarget)
        {
            if (!_avoidObstacles)
            {
                return directionToTarget.normalized;
            }

            Vector3 avoidForce = Vector3.zero;

            for (int i = 0; i < _rayCount; i++)
            {
                float angle = i * (360f / _rayCount);
                Vector3 rayDirection = Quaternion.Euler(0, 0, angle) * directionToTarget;

                RaycastHit2D hit = Physics2D.Raycast(currentPosition, rayDirection, 1, _blockLayerLit | _blockLayer);

                if (hit.collider != null)
                {
                    if (hit.collider.transform != _targeting.GetCurrentTarget().transform)
                    {
                        Vector3 hitNormal = hit.normal;
                        hitNormal.y = 0; // Flatten the force to the horizontal plane
                        avoidForce += hitNormal * (10 / hit.distance);
                    }
                    else
                    {
                        return directionToTarget;
                    }
                }
            }

            Vector3 desiredDirection = directionToTarget + avoidForce;
            return desiredDirection.normalized;
        }
    }
}