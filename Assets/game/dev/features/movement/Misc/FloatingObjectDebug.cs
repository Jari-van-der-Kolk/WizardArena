using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjectDebug : MonoBehaviour
{
    [Tooltip("How many times per second to update stats")]
    [SerializeField] private float m_RefreshRate = 4;

    private int m_FrameCount = 0;
    private float m_Time = 0;
    private float m_FPS = 0;
    private float m_TopSpeed = 0;
    private Vector3 m_LastPosition;

    private void Start()
    {
        m_LastPosition = transform.position;
    }

    private void LateUpdate()
    {
        // Calculate frames-per-second.
        m_FrameCount++;
        m_Time += Time.deltaTime;
        if (m_Time > 1.0f / m_RefreshRate)
        {
            m_FPS = Mathf.Round(m_FrameCount / m_Time);
            m_FrameCount = 0;
            m_Time -= 1.0f / m_RefreshRate;
        }

        // Calculate speed based on position changes.
        Vector3 currentPosition = transform.position;
        float objectSpeed = Vector3.Distance(currentPosition, m_LastPosition) / Time.deltaTime;
        m_LastPosition = currentPosition;

        // Calculate top velocity.
        if (objectSpeed > m_TopSpeed)
        {
            m_TopSpeed = objectSpeed;
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 130, 60),
            "FPS: " + m_FPS + "\n" +
            "Speed: " + Mathf.Round(m_TopSpeed * 100) / 100 + " (ups)");
    }
}