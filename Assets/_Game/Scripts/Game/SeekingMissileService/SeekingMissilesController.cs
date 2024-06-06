using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightItUp.Data;
using LightItUp.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Game.SeekingMissileService
{
    public class SeekingMissilesController
    {
        private MissileServiceConfig _seekingMissileConfig;
        private GameLevel _gameLevel;
        private List<SeekingMissile> _missiles = new List<SeekingMissile>();
        private LayerMask _blockLayer;
        private LayerMask _blockLayerLit;
        public void Init(GameManager gameManager, MissileServiceConfig seekingMissileConfig) {
           _gameLevel = gameManager.currentLevel;
           _seekingMissileConfig = seekingMissileConfig;
        }
        
        private List<BlockController> GetClosestUnlitBlocks()
        {
            var playerPosition = _gameLevel.player.transform.position;
            var unlitBlocks = new List<BlockController>();

            // Select unlit blocks
            foreach (var block in _gameLevel.blocks)
            {
                if (!block.IsLit)
                {
                    unlitBlocks.Add(block);
                }
            }

            unlitBlocks.Sort((a, b) => 
                (a.transform.position - playerPosition).sqrMagnitude
                .CompareTo((b.transform.position - playerPosition).sqrMagnitude));

            // Weed out common and unusual blocks from the list 
            var blocks = unlitBlocks.AsParallel()
                .GroupBy(block => block.IsRegularBlock());

            var regularBlocks = blocks
                                     .FirstOrDefault(group => group.Key)?
                                     .ToList() ??
                                 new List<BlockController>();
            var unusualBlocks = blocks
                                    .FirstOrDefault(group => !group.Key)?
                                    .ToList() ??
                                new List<BlockController>();

            // merge two lists
            regularBlocks.AddRange(unusualBlocks);

            return regularBlocks;
        }

        public void SetLayers(LayerMask blockLayer, LayerMask blockLayerLit)
        {
            _blockLayer = blockLayer;
            _blockLayerLit = blockLayerLit;
        }
        
        public async Task RocketLaunch()
        {
            var targetList = GetClosestUnlitBlocks();
            var spawnPosition = _gameLevel.player.transform.position;
            spawnPosition.y += 0.8f;
    
            for (var i = 0; i < _seekingMissileConfig.missilesCount; i++)
            {
                var delay = i == 0 ? 0 : _seekingMissileConfig.gapsBetweenShots * 1000;
                await Task.Delay(delay);
                var missile = ObjectPool.GetSeekerMissile();
                spawnPosition.x += Random.Range(-2f, 2f);
                if (!IsPositionFree(spawnPosition))
                {
                    spawnPosition.x += Random.Range(-2f, 2f);
                }
                missile.transform.position = spawnPosition;
                missile.Init(
                    this, 
                    targetList, 
                    _seekingMissileConfig
                    );
                
                _missiles.Add(missile);
                _gameLevel.player.camFocus.AddMissile(missile);

                if (targetList.Count <= 0)
                {
                    continue;
                }
                
                var firstTarget = targetList[0];
                targetList.RemoveAt(0); 
                targetList.Add(firstTarget);
            }
        }
        
        private bool IsPositionFree(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(position, 2, _blockLayer | _blockLayerLit);
            return colliders.Length == 0;
        }
        
        public void OnExploded(SeekingMissile missile, bool returned)
        {
            _missiles.Remove(missile);
            _gameLevel.player.camFocus.RemoveMissile(missile);
            if (returned)
            {
                return;
            }
            ObjectPool.ReturnSeekerMissile(missile);
        }
    }
}
