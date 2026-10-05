using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chestspawn : MonoBehaviour
{
    public GameObject chestPrefab; // Assign Chest A prefab
    public Transform cameraTransform; // Assign AR Camera
    public Transform parentTransform; // Assign XR Origin

    public float distanceFromCamera = 1.5f; // berapa meter depan camera

    void Start()
    {
        if (chestPrefab != null && cameraTransform != null && parentTransform != null)
        {
            Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
            Quaternion spawnRotation = Quaternion.LookRotation(-cameraTransform.forward); // bagi chest hadap camera
            GameObject spawnedChest = Instantiate(chestPrefab, spawnPosition, spawnRotation, parentTransform);
        }
        else
        {
            Debug.LogWarning("❌ Missing reference in CHEST_SPAWNER");
        }
    }
}