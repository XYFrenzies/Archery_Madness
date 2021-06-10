using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundPlayer : MonoBehaviour
{
    public AudioClipPool[] Pools;

    public void Play(string a_ClipName)
    {
        //find the pool in Pools that has the name, and play a random one.
    }

    public static SoundPlayer Get(string a_Name)
    {
        if (m_AllPlayers.TryGetValue(a_Name, out SoundPlayer player))
        {
            return player;
        }

        return null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void OnLoad()
    {
        SoundPlayer[] soundPlayers = Resources.FindObjectsOfTypeAll<SoundPlayer>();
        m_AllPlayers = new Dictionary<string, SoundPlayer>();
        foreach (SoundPlayer player in soundPlayers)
        {
            m_AllPlayers.Add(player.gameObject.name, player);
        }
    }

    private static Dictionary<string, SoundPlayer> m_AllPlayers;
}

