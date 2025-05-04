using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

[RequireComponent(typeof(Collider))]

public class CamZone : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera virtualCamera;
    void Start()
    {
        virtualCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = false;
        }
    }

    private void OnValidate()
    {
        GetComponent<Collider>().enabled = true;
    }
}
