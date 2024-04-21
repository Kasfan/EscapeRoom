using UnityEngine.Events;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// <see cref="StateProxyComponent{T}"/> for Bool value
    /// </summary>
    public class BoolStateProxyComponent: StateProxyComponent<bool>
    {
        public UnityEvent OnTrue;
        public UnityEvent OnFalse;

        protected override void OnStateChanged(bool val)
        {
            if(val)
                OnTrue?.Invoke();
            else
                OnFalse?.Invoke();
        }
    }
}