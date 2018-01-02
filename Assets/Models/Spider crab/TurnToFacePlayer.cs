using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToFacePlayer : MonoBehaviour
{
    private Camera m_camera;


    void Awake()
    {
        m_camera = Camera.main;
    }


    void Update()
    {
        Vector3 playerAtCrabEyeLevel = m_camera.transform.position;
        playerAtCrabEyeLevel.y = transform.position.y;
        float distToPlayer = (playerAtCrabEyeLevel - transform.position).magnitude;

        Vector3 onCameraPos = m_camera.WorldToViewportPoint(transform.position);
        bool isInPlayerView = (onCameraPos.z > 0.0f &&
            onCameraPos.x >= 0.0f && onCameraPos.x <= 1.0f &&
            onCameraPos.y >= 0.0f && onCameraPos.y <= 1.0f);

        if (isInPlayerView == false || distToPlayer > 35.0f)
        {
            transform.LookAt(playerAtCrabEyeLevel);
        }
    }
}
