using System.Collections.Generic;
using BodyPartSwap;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    private static Blackboard m_instance;

    public static Blackboard instance
    {
        get
        {
            if (m_instance == null)
            {
                Debug.LogError("What?? No instance found!!! :(((");
                return null;
            }
            return m_instance;
        }
    }

    public CharacterSetupMemory loadedSave              = null;
    public Dictionary<Options, int> activeConfiguration = null;

    public static void ResetBlackboard(Blackboard _instance)
    {
        m_instance = _instance;
    }
}