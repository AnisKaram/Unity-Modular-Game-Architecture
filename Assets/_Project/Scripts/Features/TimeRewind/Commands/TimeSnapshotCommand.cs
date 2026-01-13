using Project.Core.Interfaces;
using UnityEngine;

namespace Project.Features.TimeRewind.Commands
{
    public class TimeSnapshotCommand : ICommand
    {
        private readonly Transform m_Target;
        private readonly Rigidbody m_Rigidbody;
        private readonly Vector3 m_Position;
        private readonly Quaternion m_Rotation;
        private readonly Vector3 m_Velocity;

        public TimeSnapshotCommand(Transform target, Rigidbody rigidbody)
        {
            m_Target = target;
            m_Rigidbody = rigidbody;
            
            m_Position = m_Target.position;
            m_Rotation = m_Target.rotation;
        }
        
        public void Execute() { }

        public void Undo()
        {
            m_Target.position = m_Position;
            m_Target.rotation = m_Rotation;
        }
    }
}