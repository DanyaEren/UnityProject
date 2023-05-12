using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereChallenge : MonoBehaviour
{
    [SerializeField] private Statue _statue;
    [SerializeField] private List<SphereEnter> _sphereEnter;
    
    public void CheckIfChallengeCompleted()
    {
        foreach (var sphere in _sphereEnter)
        {
            if (!sphere._wasSphereEnter)
                return;
        }

        StartCoroutine(_statue.GoDown());
    }
}
