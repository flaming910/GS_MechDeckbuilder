using MDB.Managers;
using UnityEngine;

namespace MDB.Characters
{
    /// <summary> The actual player class used to make mechanics specific to the player </summary>
    public class Player : Character
    {
        /// <summary> Called once before the first frame </summary>
        protected override void Start()
        {
            SetUpBodyParts();

            base.Start();
        }

        /// <summary> Set up the Player's Energy stat. Basically refill energy, and also update energy text </summary>
        public override void InitializeEnergy()
        {
            base.InitializeEnergy();

            UIManager.Instance.UpdateEnergyUI(currentEnergy, maxEnergy);
        }

        /// <summary> If the player is dead, game over. </summary>
        public override void TakeDamage(int damageTaken)
        {
            base.TakeDamage(damageTaken);

            if (currentCoreHealth <= 0)
            {
                // Whatever to game over
            }
        }

        /// <summary> Increase Player's Energy by some amount and also update the energy text </summary>
        public override void GainEnergy(int energyGained)
        {
            base.GainEnergy(energyGained);

            UIManager.Instance.UpdateEnergyUI(currentEnergy, maxEnergy);
        }

        /// <summary> Decreade Player's Energy by some amount and also update the energy text </summary>
        public override void ReduceEnergy(int energyUsed)
        {
            base.ReduceEnergy(energyUsed);

            UIManager.Instance.UpdateEnergyUI(currentEnergy, maxEnergy);
        }

    }
}
