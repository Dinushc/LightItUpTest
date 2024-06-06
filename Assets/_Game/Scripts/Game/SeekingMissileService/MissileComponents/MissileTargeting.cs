using System.Collections.Generic;
using LightItUp.Game;

namespace _Game.Scripts.Game.SeekingMissileService.MissileComponents
{
    public class MissileTargeting
    {
        private List<BlockController> _targets;
        private BlockController _currentTarget;

        public void AssignTargets(List<BlockController> targets)
        {
            _targets = targets;
            PickTarget();
        }

        private void PickTarget()
        {
            if (_targets.Count == 0)
            {
                _currentTarget = null;
                return;
            }

            _currentTarget = _targets[0];
        }
        
        public BlockController TryGetNextTarget()
        {
            PickTarget();
            if (_currentTarget == null)
            {
                return null;
            }
            return _currentTarget;
        }

        public BlockController GetCurrentTarget()
        {
            return _currentTarget;
        }

        public void Clear()
        {
            _targets = null;
            _currentTarget = null;
        }
    }
}