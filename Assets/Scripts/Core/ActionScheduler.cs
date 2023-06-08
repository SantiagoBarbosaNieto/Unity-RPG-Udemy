using System;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                Debug.Log("Cancelling " + currentAction);
                currentAction.Cancel();
            }
            currentAction = action;
        }

        internal void CancelCurrentAction()
        {
            if (currentAction == null) return;
            currentAction.Cancel();
        }
    }
}