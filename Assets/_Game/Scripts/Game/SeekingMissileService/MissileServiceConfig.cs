using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissileService

{
    [CreateAssetMenu(fileName = "SeekingMissileData", menuName = "[HyperCasual]/SeekingMissileData")]
    public class MissileServiceConfig : ScriptableObject
    {
        [SerializeField] private int _missilesCount = 3;
        [SerializeField] private float _missileSpeed = 10f;
        [SerializeField] private int _usesPerLevel = 1;
        [SerializeField] private int _gapsBetweenShots = 0;
        [SerializeField] private bool _avoidObstacles;
        
        public int missilesCount => _missilesCount;
        public float missileSpeed => _missileSpeed;
        public int usesPerLevel => _usesPerLevel;
        public int gapsBetweenShots => _gapsBetweenShots;
        public bool avoidObstacles => _avoidObstacles;
    }
}