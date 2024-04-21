using UnityEngine;

namespace EscapeRoom.QuestObjects
{
    public class CrystalColor: MonoBehaviour
    {
        /// <summary>
        /// Color type of the crystal
        /// </summary>
        public enum Color
        {
            Red = 0,
            Green = 1,
            Purple = 2
        }

        [SerializeField] 
        [Tooltip("Type of the crystal")]
        private Color color;

        [SerializeField] 
        [Tooltip("Color materials of the crystal: 1:Red, 2:Green, 3:Purple")]
        private Material[] colorMaterials;

        [SerializeField] 
        [Tooltip("Renderer of the crystal")]
        private Renderer graphics;
        
        public Color CurrentColor => color;

        private void Awake()
        {
            graphics.material = colorMaterials[(int)color];
        }
    }
}