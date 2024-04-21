using TMPro;
using UnityEngine;

namespace EscapeRoom.UI
{
    public class RayInteractorUI: MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI tooltip;
        
        public string HoverText
        {
            get => tooltip.text;
            set => tooltip.text = value;
        }

        public bool Active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
    }
}