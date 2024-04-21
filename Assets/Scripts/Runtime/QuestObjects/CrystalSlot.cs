using EscapeRoom.Interactions;
using EscapeRoom.QuestLogic;
using UnityEngine;

namespace EscapeRoom.QuestObjects
{
    public class CrystalSlot: NetworkedDropZone
    {
        /// <summary>
        /// Condition that check if CrystalSlot has object of target color
        /// </summary>
        [System.Serializable]
        public class CrystalSlotHasTargetColor: Condition
        {
            [SerializeField] 
            private CrystalSlot dropZone;

            [SerializeField] 
            private CrystalColor.Color targetColor;

            public override bool IsTrue
            {
                get
                {
                    if (dropZone == null)
                        return false;
                    
                    var interactable = dropZone.DroppedInteractable;
                    if (interactable == null)
                        return false;

                    var crystalColor = interactable.Transform.GetComponent<CrystalColor>();
                    if (crystalColor == null)
                        return false;

                    return crystalColor.CurrentColor == targetColor;
                }
            }
        }
    }
}