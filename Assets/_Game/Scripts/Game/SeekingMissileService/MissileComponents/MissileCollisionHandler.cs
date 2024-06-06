using System;
using System.Collections;
using LightItUp.Game;
using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissileService.MissileComponents
{
    public class MissileCollisionHandler : MonoBehaviour
    {
        private Collider2D _collider2D;
        private MissileTargeting _missileTargeting;
        private SeekingMissilesController _seekingMissilesController;

        public void Init(SeekingMissilesController controller, Collider2D collider2D, MissileTargeting missileTargeting)
        {
            _seekingMissilesController = controller;
            _collider2D = collider2D;
            _missileTargeting = missileTargeting;
        }

        public void OnCollide(Collision2D other, SeekingMissile missile)
        {
            if (other == null)
            {
                return;
            }

            if (other.gameObject == null)
            {
                return;
            }
            if (other.gameObject == _missileTargeting.GetCurrentTarget().gameObject)
            {
                Collide(missile, _missileTargeting.GetCurrentTarget());
            }
            else if (other.gameObject.GetComponent<BlockController>() != null)
            {
                if (!other.gameObject.GetComponent<BlockController>().IsLit)
                {
                    Collide(missile, other.gameObject.GetComponent<BlockController>());
                }
                else
                {
                    Physics2D.IgnoreCollision(_collider2D, other.collider);
                }
            }
            else
            {
                Physics2D.IgnoreCollision(_collider2D, other.collider);
            }
        }

        private void Collide(SeekingMissile missile, BlockController block)
        {
            block.Collide(true);
            StartCoroutine(WaitAndExecute(0.1f, () => Explode(missile)));
        }

        private IEnumerator WaitAndExecute(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        public void Explode(SeekingMissile missile)
        {
            _missileTargeting.Clear();
            _seekingMissilesController.OnExploded(missile, missile.returned);
        }
    }
}