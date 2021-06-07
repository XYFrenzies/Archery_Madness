using System.Collections.Generic;
using UnityEngine.XR;

public class GameStateManager : Singleton< GameStateManager >
{
    public HandController LeftController;
    public HandController RightController;
    public HighScore HighScore;
    public CurrentScore CurrentScore;
}
