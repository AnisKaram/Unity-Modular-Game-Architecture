using System;
using Project.Core.Input;
using Project.Features.TimeRewind.Commands;
using Project.Features.TimeRewind.Domain;
using UnityEngine;
using VContainer;
using ICommand = Project.Core.Interfaces.ICommand;

namespace Project.Features.TimeRewind.Components
{
    public class TimeManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private Transform m_Target;
        
        [Header("Settings")]
        [SerializeField] private float m_SecondsToRecord = 5f;
        [SerializeField] private float m_FramesOffset = 2f;
        [SerializeField] private bool m_IsRewinding;

        private Vector3 m_LastRecordedPosition;
        private Quaternion m_LastRecordedRotation;
        
        private PlayerInputReader m_PlayerInputReader;
        private CircularBuffer<ICommand> m_Commands;

        public event Action<bool> OnRewindStateChanged;

        [Inject]
        public void Construct(PlayerInputReader playerInputReader)
        {
            m_PlayerInputReader = playerInputReader;
        }
        
        private void Awake()
        {
            int bufferSize = Mathf.RoundToInt(m_SecondsToRecord / Time.fixedDeltaTime);
            m_Commands = new CircularBuffer<ICommand>(bufferSize);

            RecordPositionAndRotation();
        }
        private void Update()
        {
            m_IsRewinding = m_PlayerInputReader.GetPlayerInputData().isRewinding;
        }
        private void FixedUpdate()
        {
            if (m_IsRewinding)
            {
                Rewind();
                return;
            }
            
            OnRewindStateChanged?.Invoke(false);
            
            if (Time.frameCount % m_FramesOffset != 0) // Only record 2nd frame.
            {
                return;
            }
            Record();
        }

        private void Rewind()
        {
            m_Rigidbody.isKinematic = true;

            if (!m_Commands.IsNotEmpty)
            {
                return;
            }
            
            ICommand command = m_Commands.Pop();
            command.Undo();
            
            OnRewindStateChanged?.Invoke(true);
        }

        private void Record()
        {
            // Only record when the target is moving.
            if (Vector3.Distance(m_Target.position, m_LastRecordedPosition) < 0.01f  &&
                Quaternion.Angle(m_Target.rotation, m_LastRecordedRotation) < 0.1f)
            {
                return;
            }
            
            m_Rigidbody.isKinematic = false;
            
            TimeSnapshotCommand timeSnapshot = new TimeSnapshotCommand(m_Target, m_Rigidbody);
            m_Commands.Push(timeSnapshot);

            RecordPositionAndRotation();
        }

        private void RecordPositionAndRotation()
        {
            m_LastRecordedPosition = m_Target.position;
            m_LastRecordedRotation = m_Target.rotation;
        }
    }
}