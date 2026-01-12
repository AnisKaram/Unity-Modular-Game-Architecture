using System;
using System.Collections.Generic;

namespace Project.Core
{
    public class EventBus
    {
        private readonly Dictionary<Type, Delegate> m_Handlers = new Dictionary<Type, Delegate>();

        public void Subscribe<T>(Action<T> callback)
        {
            if (!m_Handlers.ContainsKey(typeof(T))) // Add the callback if the type T isn't in the dictionary. 
            {
                m_Handlers.Add(typeof(T), callback); 
            }
            else // Combine the callback to the existing chain.
            {
                m_Handlers[typeof(T)] = Delegate.Combine(m_Handlers[typeof(T)], callback);
            }
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            if (m_Handlers.ContainsKey(typeof(T))) // Remove the callback from the source. 
            {
                m_Handlers[typeof(T)] = Delegate.Remove(m_Handlers[typeof(T)], callback);

                if (m_Handlers[typeof(T)] == null) // Remove the key when the list becomes null/empty.
                {
                    m_Handlers.Remove(typeof(T));
                }
            }
        }

        public void Raise<T>(T signal)
        {
            if (m_Handlers.ContainsKey(typeof(T)))
            {
                Action<T> casted = (Action<T>)m_Handlers[typeof(T)];
                casted?.Invoke(signal);
            }
        }
    }
}