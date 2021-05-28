using MDB.Controllers;
using UnityEngine;

namespace MDB.Managers
{
    /// <summary> A character's turn. Mostly it just resets their energy (Future plans: Applies "Next round:" effects) </summary>
    [System.Serializable]
    public class Turn
    {
        /// <summary> Character controller whose turn this is </summary>
        [SerializeField]
        private ControllerBase characterController;

        public Turn(ControllerBase characterController)
        {
            this.characterController = characterController;
        }

        // Make a list of effects maybe

        /// <summary> Go through the list of start of turn effects I guess, such as refilling energy and drawing cards </summary>
        public Turn StartTurnEffects()
        {
            characterController.MySourceCharacter.InitializeEnergy();
            characterController.IsMyTurn = true;
            return this;
        }

        public void EndTurnEffects()
        {
            characterController.IsMyTurn = false;
            characterController.DiscardAndDraw();
        }
    }
}
