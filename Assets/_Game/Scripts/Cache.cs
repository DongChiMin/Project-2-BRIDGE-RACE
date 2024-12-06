using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache
{
    static public Dictionary<GameObject, MeshRenderer> m_dictionary_StairPole = new Dictionary<GameObject, MeshRenderer>();

    static public MeshRenderer GetStairMeshRenderer(GameObject stairPole)
    {

        if (m_dictionary_StairPole.ContainsKey(stairPole))
        {
            return m_dictionary_StairPole[stairPole];
        }

        return m_dictionary_StairPole[stairPole] = stairPole.GetComponent<MeshRenderer>();
    }
}
