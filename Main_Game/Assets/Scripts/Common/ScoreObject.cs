using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ CreateAssetMenu( fileName = "NewScoreObject", menuName = "Score/ScoreObject", order = 0 ) ]
public class ScoreObject : ScriptableObject
{
    public HighScore[] HighScores;

    public void Save()
    {
        EditorUtility.SetDirty( this );
        AssetDatabase.SaveAssets();
    }
}
