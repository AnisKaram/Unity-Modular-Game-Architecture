using System;
using Project.Features.TimeRewind.Components;
using UnityEngine;
using VContainer;

namespace Project.Features.TimeRewind.View
{
    public class RewindFeedbackView : MonoBehaviour
    {
        [SerializeField] private GameObject m_RewindFeedbackText;
        
        private TimeManager m_TimeManager;

        [Inject]
        public void Construct(TimeManager timeManager)
        {
            m_TimeManager = timeManager;
        }

        private void Awake()
        {
            m_TimeManager.OnRewindStateChanged += TimeManager_OnRewindStateChanged;
            
            DisableFeedback();
        }
        private void OnDestroy()
        {
            m_TimeManager.OnRewindStateChanged -= TimeManager_OnRewindStateChanged;
        }

        private void TimeManager_OnRewindStateChanged(bool isRewindActive)
        {
            if (isRewindActive)
            {
                EnableFeedback();
                return;
            }
            DisableFeedback();
        }

        private void EnableFeedback() => m_RewindFeedbackText.SetActive(true);
        private void DisableFeedback() => m_RewindFeedbackText.SetActive(false);
    }
}