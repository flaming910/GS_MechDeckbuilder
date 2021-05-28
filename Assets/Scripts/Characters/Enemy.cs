using UnityEngine;

namespace MDB.Characters
{
    /// <summary> Enemy class used to make mehcanics specific to the enemy AI, such as choosing a card based on current scenario </summary>
    public class Enemy : Character
    {
        /// <summary> Called once before the first frame </summary>
        protected override void Start()
        {
            SetUpBodyParts();

            base.Start();
        }

        /// <summary> If the enemy is dead, end combat. </summary>
        public override void TakeDamage(int damageTaken)
        {
            base.TakeDamage(damageTaken);

            if (currentCoreHealth == 0)
            {
                Managers.UIManager.Instance.OpenRewards();
            }
        }
    }
}
