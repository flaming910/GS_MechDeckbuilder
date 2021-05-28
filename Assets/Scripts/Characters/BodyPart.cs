using MDB.Managers;
using System;
using MDB.UI;
using TMPro;
using UnityEngine.UI;

namespace MDB.Characters
{
    public enum BodyPartType { Head, Torso, RightArm, LeftArm, RightLeg, LeftLeg, Core };

    /// <summary> Data of a body part. Its type and max health </summary>
    [Serializable]
    public struct BodyPartsData
    {
        public BodyPartType type;
        public int maxHealth;
    }

    /// <summary> Class creating a body part with a certain type and health  </summary>
    public class BodyPart : ITarget
    {
        /// <summary> Type of body part. Arm, head, etc... </summary>
        private BodyPartType type;

        /// <summary> Type of body part. Arm, head, etc... </summary>
        public BodyPartType MyType { get => type; set => type = value; }

        /// <summary> Character this body part belongs to </summary>
        private Character owner;

        /// <summary> Character this body part belongs to </summary>
        public Character MyOwner { get => owner; set => owner = value; }

        /// <summary> Maximum health this body part has </summary>
        private int maxHealth;

        /// <summary> Maximum health this body part has </summary>
        public int MyMaxHealth { get => maxHealth; set => maxHealth = value; }

        /// <summary> Health this body part currently has </summary>
        private int currentHealth;

        /// <summary> Current temporary health(Armor) the body part has </summary>
        private int currentArmor;

        /// <summary> Health this body part currently has </summary>
        public int MyCurrentHealth { get => currentHealth; set => currentHealth = value; }

        /// <summary> The visual health bar of this body part </summary>
        private HealthBar healthBar;

        /// <summary> Whether this body part is currently alive </summary>
        private bool isFunctional;

        /// <summary> Whether this body part is currently alive  </summary>
        public bool IsFunctional { get => isFunctional; set => isFunctional = value; }

        public BodyPart(BodyPartType type, Character owner, int maxHealth, HealthBar healthBar)
        {
            this.type = type;
            this.owner = owner;
            this.maxHealth = maxHealth;
            currentHealth = maxHealth;
            isFunctional = true;
            this.healthBar = healthBar;
            healthBar.InitialiseSlider(currentHealth, maxHealth);

            UpdateHealthUI();
        }

        public BodyPart(BodyPart bodyPart)
        {
            type = bodyPart.type;
            owner = bodyPart.owner;
            maxHealth = bodyPart.maxHealth;
            currentHealth = bodyPart.maxHealth;
            isFunctional = bodyPart.isFunctional;
        }

        private void UpdateHealthUI()
        {
            UIManager.Instance.UpdateHealthUI(currentHealth, maxHealth, healthBar);
        }

        private void UpdateArmorUI()
        {
            UIManager.Instance.UpdateArmorUI(currentArmor, healthBar);
        }

        /// <summary>
        /// Reduce body part's health by some amount.
        /// If body part is dead, deal damage to the character's core
        /// </summary>
        /// <param name="damageTaken"> The amount of health to lose </param>
        public void TakeDamage(int damageTaken)
        {
            if (IsFunctional)
            {
                if (currentArmor > 0)
                {
                    currentArmor -= damageTaken;
                    if (currentArmor < 0)
                    {
                        damageTaken = currentArmor * -1;
                        currentArmor = 0;
                    }
                    else
                    {
                        damageTaken = 0;
                    }

                    UpdateArmorUI();
                }
                MyCurrentHealth -= damageTaken;

                if (MyCurrentHealth <= 0)
                {
                    MyOwner.TakeDamage(MyCurrentHealth * -1); // Carry over excess damage to character's core health

                    //Disable body part
                    MyCurrentHealth = 0;

                    IsFunctional = false;

                }

                UpdateHealthUI();
            }
            else
            {
                MyOwner.TakeDamage(damageTaken);
            }
        }

        /// <summary>
        /// Heal the body part
        /// </summary>
        /// <param name="heal"></param>
        public void Heal(int heal)
        {
            if (MyCurrentHealth + heal > MyMaxHealth)
            {
                MyCurrentHealth = MyMaxHealth;
            }
            else
            {
                MyCurrentHealth += heal;
            }
            if (!isFunctional) isFunctional = true;

            UpdateHealthUI();
        }

        public void GainArmor(int armor)
        {
            if (!isFunctional) return;
            currentArmor += armor;
            UpdateArmorUI();
        }
    }


}
