﻿using UnityEngine;

namespace Movement
{
    /// <summary>
    /// This script is used to display real-time statistics to help with tweaking and debugging.
    /// </summary>
    [RequireComponent(typeof(Q3PlayerController))]
    public class Q3PlayerDebug : MonoBehaviour
    {
        [SerializeField] private Vector2 boxSize = new Vector2(160, 100);

        [Tooltip("How many times per second to update stats")]
        [SerializeField] private float m_RefreshRate = 4;

        private int m_FrameCount = 0;
        private float m_Time = 0;
        private float m_FPS = 0;
        private float m_TopSpeed = 0;
        private Q3PlayerController m_Player;

        private void Start()
        {
            m_Player = GetComponent<Q3PlayerController>();
        }

        private void LateUpdate()
        {
            // Calculate frames-per-second.
            m_FrameCount++;
            m_Time += Time.deltaTime;
            if (m_Time > 1.0 / m_RefreshRate)
            {
                m_FPS = Mathf.Round(m_FrameCount / m_Time);
                m_FrameCount = 0;
                m_Time -= 1.0f / m_RefreshRate;
            }

            // Calculate top velocity.
            if (m_Player.Speed > m_TopSpeed)
            {
                m_TopSpeed = m_Player.Speed;
            }
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, boxSize.x, boxSize.y),
                "FPS: " + m_FPS + "\n" +
                "Speed: " + Mathf.Round(m_Player.Speed * 100) / 100 + " (ups)\n" +
                "Top: " + Mathf.Round(m_TopSpeed * 100) / 100 + " (ups)\n" +
                "Velocity: " + m_Player.axisVelocity);
        }
    }
}