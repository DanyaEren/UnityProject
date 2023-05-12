using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushHourWinZone : MonoBehaviour
{
    [SerializeField] private RushHourChallenge _rushHourChallenge;
    [SerializeField] private OpenableObject _openableObject;
    private bool _wasActivated = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (_wasActivated)
            return;

        if(collision.gameObject.name.Equals("MainCube"))
        {
            StartCoroutine(_openableObject.Open());
            _rushHourChallenge.OnWin();
            _wasActivated = true;
        }
    }
}
