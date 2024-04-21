using UnityEngine.Events;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// <see cref="StateProxyComponent{T}"/> for Int value
    /// </summary>
    public class IntStateProxyComponent: StateProxyComponent<int>
    {
        public UnityEvent<int> OnValueChanged;

        protected override void OnStateChanged(int val)
        {
            OnValueChanged?.Invoke(val);
        }
    }
}