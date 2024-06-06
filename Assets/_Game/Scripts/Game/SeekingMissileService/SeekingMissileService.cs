using LightItUp;
using LightItUp.Data;
using LightItUp.Game;
using LightItUp.Singletons;
using LightItUp.UI;

namespace _Game.Scripts.Game.SeekingMissileService
{
    public class SeekingMissileService : SingletonCreate<SeekingMissileService>
    {
        public MissileServiceConfig seekerMissileConfig;
        private SeekingMissilesController _seekingMissilesController;
        
        private GameManager _gameManager;
        private PlayerController _playerController;
        private UI_Game _uiGame;
        
        private int _usesPerLevel = 1;
        
        public void Start()
        {
            _gameManager = GameManager.Instance;
            _seekingMissilesController = new SeekingMissilesController();
        }
        
        public void OnLoad(GameLevel level)
        {
            _playerController = level.player;
            _uiGame = CanvasController.GetPanel<UI_Game>();
            _usesPerLevel = seekerMissileConfig.usesPerLevel;
            if (_usesPerLevel > 0)
            {
                _uiGame.ShowMissileButton();   
            }
        }

        private void OnUse()
        {
            _usesPerLevel--;
            if (_usesPerLevel <= 0)
            {
                _uiGame.HideMissileButton();
            }
        }
        
        //unity event -> button
        public void UseMissiles()
        {
            _seekingMissilesController.Init(_gameManager ,seekerMissileConfig);
            _seekingMissilesController.RocketLaunch();
            OnUse();
        }
    }
}