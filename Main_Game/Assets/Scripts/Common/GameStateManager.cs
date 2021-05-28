using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton< GameStateManager >
{
    public HandController LeftHand;
    public HandController RightHand;
    public BowController Bow;
    public TargetSpawner TargetSpawn;
    public ArrowSpawner ArrowSpawner_Broadhead;
    public ArrowSpawner ArrowSpawner_Hammerhead;
    public ArrowSpawner ArrowSpawner_WaterBalloon;
}
