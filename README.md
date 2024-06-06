# LightItUpQubic

## Added feature:
___
- About ->
   a feature made to increase the player's chances of passing the level. This is the ability to fire projectiles at blocks and illuminate them. RayCast with steering behavior is used to avoid obstacles.
- The seeking missile feature settings `{Assets/_Game/Resources/SeekingMissileData.asset}`.
   - **Missiles Count:** shows how many missiles will be fired each time the button is pressed.
  - **Missile Speed:** shows the velocity of the projectiles and this is customizable.
  - **Uses Per Level:** shows how many times you can press the button and fire missiles in one level.
  - **Gaps Between Shots:** delay between shots. The default is 0, but you can set it so that the shells are fired one by one, for example after a second each.
  - **Avoid Obstacles:** this checkbox enables the obstacle avoidance function. If it is off, the projectiles fly in a straight line.

- All added scripts can be found at this path `{Assets/_Game/Scripts/Game/SeekingMissileService}`.
  - **MissileServiceConfig**
  - **SeekingMissile**
  - **SeekingMissilesController**
  - **SeekingMissileService**
  - **MissileCollisionHandler**
  - **MissileMovement**
  - **MissileRenderer**
  - **MissileTargeting**

- All updated scripts:
  - **BlockController**
  - **CameraFocus**
  - **GameManager**
  - **ObjectPool**
  - **UI_Game**

## Contact:
  - **Email ->** powderedlizardbrains@gmail.com
  - **Linkedin ->** [Dmitrii Efremov](https://www.linkedin.com/in/dmitrii-efremov-w/)

