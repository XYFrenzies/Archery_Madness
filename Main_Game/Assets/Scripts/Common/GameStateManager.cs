using System.Collections.Generic;
using UnityEngine.XR;

public class GameStateManager : Singleton< GameStateManager >
{
    public HandController LeftController;
    public HandController RightController;
    public BowController Bow;
    public TargetSpawner TargetSpawn;
    public ArrowSpawner ArrowSpawner_Broadhead;
    public ArrowSpawner ArrowSpawner_Hammerhead;
    public ArrowSpawner ArrowSpawner_WaterBalloon;
}
