using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEnter : MonoBehaviour
{
    private SphereChallenge sphereChallenge;
    public bool _wasSphereEnter;

    private void Awake()
    {
        sphereChallenge = GetComponentInParent<SphereChallenge>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_wasSphereEnter)
            return;

        if(other.CompareTag("Sphere"))
        {
            _wasSphereEnter = true;
            other.transform.SetParent(transform);
            other.transform.position = transform.position;
            other.GetComponent<Rigidbody>().isKinematic = true;
            sphereChallenge.CheckIfChallengeCompleted();
        }
    }
}
