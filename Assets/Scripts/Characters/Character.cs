using System;
using MDB.Managers;
using System.Collections.Generic;
using MDB.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MDB.Characters
{
    /// <summary> Big dick class that contains all shared character features. I.e. Health, Energy, Body parts, etc... </summary>
    public class Character : MonoBehaviour, ITarget
    {
        /// <summary> List of body parts the character has </summary>
        protected Dictionary<BodyPartType, BodyPart> bodyParts;

        /// <summary> Data of character's every body part </summary>
        [SerializeField, ArrayElementTitle("type")]
        protected BodyPartsData[] bodyPartsData;

        /// <summary> Max health the character has </summary>
        [SerializeField]
        protected int maxCoreHealth;

        /// <summary> Health the character currently has </summary>
        protected int currentCoreHealth;

        /// <summary> Max energy the character has </summary>
        [SerializeField]
        protected int maxEnergy;

        /// <summary> Energy the character currently has </summary>
        protected int currentEnergy;

        #region UI Element References
        /// <summary> The visual health bar of this character </summary>
        protected HealthBar healthBar;

        /// <summary> The health bar prefab for core health and body part healths </summary>
        protected GameObject healthBarPrefab;

        /// <summary> The area where all this character's health bars will go </summary>
        protected Transform healthField;

        #endregion

        #region Getters
        /// <summary> Health the character currently has </summary>
        public int CurrentCoreHealth => currentCoreHealth;

        public int MaxCoreHealth => maxCoreHealth;

        public int MaxEnergy => maxEnergy;
        public Dictionary<BodyPartType, BodyPart> BodyPartsDict => bodyParts;

        /// <summary> Energy the character currently has </summary>
        public int CurrentEnergy => currentEnergy;

        #endregion

        /// <summary> Set up the character's energy stat. To be used at the beginning of each round </summary>
        public virtual void InitializeEnergy()
        {
            currentEnergy = maxEnergy;
        }

        /// <summary> Set up the character's health stat </summary>
        protected void InitializeHealth()
        {
            currentCoreHealth = maxCoreHealth;

            GameObject gc = Instantiate(healthBarPrefab, healthField);
            gc.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            healthBar = gc.GetComponent<HealthBar>();
            healthBar.SetName("Core");

            healthBar.InitialiseSlider(currentCoreHealth, maxCoreHealth);

            UpdateHealthUI();
        }

        /// <summary> Setting this character as the owner of each of their body parts and initializing their health stat </summary>
        protected void SetUpBodyParts()
        {
            bodyParts = new Dictionary<BodyPartType, BodyPart>();

            foreach (var bodyPart in bodyPartsData)
            {
                GameObject gc = Instantiate(healthBarPrefab, healthField);
                gc.GetComponent<HealthBar>().SetName(bodyPart.type.ToString());

                // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
                bodyParts[bodyPart.type] = new BodyPart(
                    bodyPart.type,
                    this,
                    bodyPart.maxHealth, gc.GetComponent<HealthBar>());
            }
        }

        /// <summary> Called once before the first frame </summary>
        protected virtual void Start()
        {
            InitializeHealth();
            InitializeEnergy();
        }

        public bool CheckPartStatus(BodyPartType bodyPartType)
        {
            return bodyParts[bodyPartType] != null;
        }

        public BodyPart GetBodyPart(BodyPartType type)
        {
            return bodyParts[type];
        }

        protected void UpdateHealthUI()
        {
            UIManager.Instance.UpdateHealthUI(currentCoreHealth, maxCoreHealth, healthBar);
        }

        #region Character Card Interaction

        /// <summary>
        /// Reduce character's health by some amount.
        /// </summary>
        /// <param name="damageTaken"> The amount of health to lose </param>
        public virtual void TakeDamage(int damageTaken)
        {
            currentCoreHealth = Math.Max(0, currentCoreHealth - damageTaken);

            UpdateHealthUI();
        }

        /// <summary>
        /// Heal the body part
        /// </summary>
        /// <param name="heal"></param>
        public void Heal(int heal)
        {
            if (currentCoreHealth + heal > maxCoreHealth)
            {
                currentCoreHealth = maxCoreHealth;
            }
            else
            {
                currentCoreHealth += heal;
            }

            UpdateHealthUI();
        }

        /// <summary>
        /// Reduce character's energy by some amount.
        /// </summary>
        /// <param name="energyUsed"> The amount of energy to lose </param>
        public virtual void ReduceEnergy(int energyUsed)
        {
            currentEnergy -= energyUsed;
        }

        /// <summary>
        /// Increase energy by some amount
        /// </summary>
        /// <param name="energyGained"> The amount of energy to gain</param>
        public virtual void GainEnergy(int energyGained)
        {
            currentEnergy += energyGained;
        }

        #endregion

        /// <summary>
        /// Set up the visual fields for health bars and Energy text
        /// </summary>
        /// <param name="barPrefab"> The prefab for the health bar </param>
        /// <param name="hpField"> The area where health bars will be placed </param>
        public void SetUpVisuals(GameObject barPrefab, Transform hpField)
        {
            this.healthBarPrefab = barPrefab;

            this.healthField = hpField;
        }
    }


    public class CharacterObject
    {
        public Dictionary<BodyPartType, BodyPart> bodyParts;
        public int maxCoreHealth;
        public int currentCoreHealth;
        public int maxEnergy;
        public int currentEnergy;

        public CharacterObject(Character character)
        {
            bodyParts = new Dictionary<BodyPartType, BodyPart>();
            maxCoreHealth = character.MaxCoreHealth;
            currentCoreHealth = character.CurrentCoreHealth;
            maxEnergy = character.MaxEnergy;
            currentEnergy = character.CurrentEnergy;
            foreach (var bodyPartKVP in character.BodyPartsDict)
            {
                bodyParts[bodyPartKVP.Key] = new BodyPart(bodyPartKVP.Value);
            }
        }
    }

}
