using System.Collections.Generic;
using MDB.Characters;
using MDB.Controllers;
using UnityEngine;

namespace MDB.Managers
{
    /// <summary> It manages overall game mechanics </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static GameManager Instance
        {
            get => instance;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary> List of turns. Just gonna be 2 turns at any given time. Player and Enemy </summary>
        [SerializeField]
        private Queue<Turn> turns;

        private Turn lastTurn;

        /// <summary> The turn we are currently on </summary>
        private int turnIndex;

        private ControllerBase playerController;
        private ControllerBase currentEnemyController;

        private Character playerCharacter;
        private Character currentEnemyCharacter;

        public Character PlayerCharacter => playerCharacter;
        public Character CurrentEnemyCharacter => currentEnemyCharacter;

        public ControllerBase PlayerController => playerController;
        public ControllerBase CurrentEnemyController => currentEnemyController;

        private void Start()
        {
            playerController = GameObject.FindWithTag("Player")?.GetComponent<ControllerBase>();

            playerCharacter = playerController != null ? playerController.MySourceCharacter : null;
        }

        /// <summary>
        /// Is the part alive
        /// </summary>
        /// <param name="targetType">the body part type</param>
        /// <param name="isPlayerPart">is the part belonging to the player</param>
        /// <returns></returns>
        public bool CheckPartStatus(BodyPartType targetType, bool isPlayerPart)
        {
            return isPlayerPart ? playerCharacter.CheckPartStatus(targetType) : currentEnemyCharacter.CheckPartStatus(targetType);
        }

        /// <summary> Find the enemy in the scene and set up their controller and battle turns </summary>
        public void SetUpFight()
        {
            currentEnemyController = GameObject.FindWithTag("Enemy")?.GetComponent<ControllerBase>();
            currentEnemyCharacter = currentEnemyController != null ? currentEnemyController.MySourceCharacter : null;

            SetUpTurns(new[] { playerController, currentEnemyController });
        }

        /// <summary> Instantiate the list of turns and set the respective characters for each turn </summary>
        /// <param name="controllers">the controllers of all things in the combat that need turns</param>
        private void SetUpTurns(ControllerBase[] controllers)
        {
            turns = new Queue<Turn>();

            foreach (var controller in controllers)
            {
                if (controller.IsPlayer)
                {
                    controller.UpdateDeck();
                }
                controller.DiscardAndDraw();
                turns.Enqueue(new Turn(controller));
            }

            turns.Enqueue(lastTurn = turns.Dequeue().StartTurnEffects());
        }

        /// <summary> End the current turn and start the next. Call start of turn functions </summary>
        public void EndTurn()
        {
            lastTurn.EndTurnEffects();
            turns.Enqueue(lastTurn = turns.Dequeue().StartTurnEffects());
        }
    }
}
