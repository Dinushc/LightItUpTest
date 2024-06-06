using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissileService.MissileComponents
{
    public class MissileRenderer
    {
        private SpriteRenderer _spriteRenderer;
        private TrailRenderer _trailRenderer;

        public MissileRenderer(SpriteRenderer spriteRenderer, TrailRenderer trailRenderer)
        {
            _spriteRenderer = spriteRenderer;
            _trailRenderer = trailRenderer;
        }

        public void ClearTrail()
        {
            _trailRenderer.Clear();
        }
    }
}