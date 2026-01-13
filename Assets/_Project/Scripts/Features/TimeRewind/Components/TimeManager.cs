using Project.Features.TimeRewind.Commands;
using Project.Features.TimeRewind.Domain;
using UnityEngine;
using ICommand = Project.Core.Interfaces.ICommand;

namespace Project.Features.TimeRewind.Components
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_Rigidbody;
        [SerializeField] private Transform m_Target;
        
        [SerializeField] private float m_SecondsToRecord = 5f;
        [SerializeField] private bool m_IsRewinding;
        
        private CircularBuffer<ICommand> m_Commands;

        private void Awake()
        {
            int bufferSize = Mathf.RoundToInt(m_SecondsToRecord / Time.fixedDeltaTime);
            m_Commands = new CircularBuffer<ICommand>(bufferSize);
        }
        private void Update()
        {
            m_IsRewinding = Input.GetKey(KeyCode.RightShift); // Placeholder, will be replaced later.
        }
        private void FixedUpdate()
        {
            if (m_IsRewinding)
            {
                Rewind();
                return;
            }
            
            if (Time.frameCount % 2 != 0) // Only record 2nd frame.
            {
                return;
            }
            Record();
        }

        private void Rewind()
        {
            m_Rigidbody.isKinematic = true;
                
            if (m_Commands.IsNotEmpty)
            {
                ICommand command = m_Commands.Pop();
                command.Undo();
            }
        }
        private void Record()
        {
            m_Rigidbody.isKinematic = false;
            
            TimeSnapshotCommand timeSnapshot = new TimeSnapshotCommand(m_Target, m_Rigidbody);
            m_Commands.Push(timeSnapshot);
        }
    }
}