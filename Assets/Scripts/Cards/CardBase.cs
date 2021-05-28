using System;
using System.Collections.Generic;
using MDB.Characters;
using MDB.Managers;
using MDB.Utils;
using UnityEngine;

namespace MDB.Cards
{
    public enum TargetType
    {
        PlayerHead,
        PlayerTorso,
        PlayerRightArm,
        PlayerLeftArm,
        PlayerRightLeg,
        PlayerLeftLeg,

        EnemyHead,
        EnemyTorso,
        EnemyRightArm,
        EnemyLeftArm,
        EnemyRightLeg,
        EnemyLeftLeg,

        PlayerCore,
        EnemyCore,

        None
    }

    public enum CardRarity
    {
        Default,   //0
        Common,    //1
        Uncommon,  //2
        Epic,      //3
        Legendary  //4
    }

    public class CardBase
    {
        #region Variables
        ///<summary> Name of card </summary>
        private string cardName;

        /// <summary> The visual art of this card </summary>
        private Sprite cardArt;

        ///<summary>Card text</summary>
        private string cardText;

        /// <summary> Card cost </summary>
        private int cardCost;

        ///<summary>what to target</summary>
        private TargetType[] targetTypes;

        ///<summary>The functions this card calls and in order</summary>
        private List<Action> actions;

        ///<summary>What part is needed to play this card</summary>
        private BodyPartType source;

        /// <summary>What rarity is the card(Default can't be added to deck but its there from the start </summary>
        private CardRarity rarity;
        #endregion

        #region Getters
        ///<summary> Name of card </summary>
        public string CardName => cardName;

        /// <summary> The visual art of this card </summary>
        public Sprite CardArt => cardArt;

        ///<summary>Card text</summary>
        public string CardText => cardText;

        /// <summary> Card cost </summary>
        public int CardCost => cardCost;

        ///<summary>What part is needed to play this card</summary>
        public BodyPartType Source => source;

        /// <summary> Rarity of the card </summary>
        public CardRarity Rarity => rarity;
        #endregion

        /// <summary>
        /// Construct a card
        /// </summary>
        /// <param name="cardName">Name of card</param>
        /// /// <param name="cardArt">Art of card</param>
        /// <param name="cardText">Card description text</param>
        /// <param name="cardCost">Card energy cost</param>
        /// <param name="actions">The actions the card performs in order</param>
        /// <param name="source">What body part is needed to play this card</param>
        /// <param name="targetTypes">What parts this card targets</param>
        public CardBase(string cardName, Sprite cardArt, CardRarity rarity, string cardText, int cardCost, List<Action> actions, BodyPartType source, TargetType[] targetTypes)
        {
            this.cardName = cardName;
            this.cardArt = cardArt;
            this.rarity = rarity;
            this.cardText = cardText;
            this.cardCost = cardCost;
            this.actions = actions;
            this.source = source;
            this.targetTypes = targetTypes;
        }

        /// <summary>
        /// Checks if the card can be played(can it do anything)
        /// </summary>
        /// <returns>True if card is playable</returns>
        public bool CardCanBePlayed(Character sourceCharacter)
        {
            if (sourceCharacter.CurrentEnergy < cardCost) return false;
            if (source != BodyPartType.Core) if (!sourceCharacter.GetBodyPart(source).IsFunctional) return false;
            foreach (var targetType in targetTypes)
            {
                if (targetType.ToString().Contains("Enemy"))
                {
                    if (targetType == TargetType.EnemyCore)
                    {
                        if (GameManager.Instance.CurrentEnemyCharacter.CurrentCoreHealth <= 0) return false;
                    }
                    else
                    {
                        return GameManager.Instance.CheckPartStatus(ConversionUtils.ConvertTargetToBodyPart(targetType), false);
                    }
                }
                else if (targetType.ToString().Contains("Player"))
                {
                    if (targetType == TargetType.PlayerCore)
                    {
                        if (GameManager.Instance.PlayerCharacter.CurrentCoreHealth <= 0) return false;
                    }
                    else
                    {
                        return GameManager.Instance.CheckPartStatus(ConversionUtils.ConvertTargetToBodyPart(targetType), true);
                    }
                }

            }

            return true;
        }

        /// <summary>
        /// Triggers card effects
        /// </summary>
        /// <param name="sourceCharacter">Character playing the card</param>
        public void PlayCard(Character sourceCharacter)
        {
            Cost(sourceCharacter, cardCost);
            foreach (var action in actions)
            {
                action();
            }
        }


        #region Card Functions
        /// <summary>
        /// Deduct energy from the character that plays the card
        /// </summary>
        /// <param name="sourceCharacter">Character that plays the card</param>
        /// <param name="energy">Card cost</param>
        public void Cost(Character sourceCharacter, int energy)
        {
            sourceCharacter.ReduceEnergy(energy);
        }

        /// <summary>
        /// Deal damage to a target(type either body part or character)
        /// </summary>
        /// <param name="target">Body part or character</param>
        /// <param name="damage">Damage to deal</param>
        public static void DealDamage(ITarget target, int damage)
        {
            target.TakeDamage(damage);
        }

        /// <summary>
        /// Heal a target(either body part or character)
        /// </summary>
        /// <param name="target">Body part or character</param>
        /// <param name="heal">Damage to deal</param>
        public static void Heal(ITarget target, int heal)
        {
            target.Heal(heal);
        }

        public static void GainArmor(BodyPart target, int armor)
        {
            target.GainArmor(armor);
        }

        /// <summary>
        /// Restore energy
        /// </summary>
        /// <param name="target">The character that gains energy</param>
        /// <param name="energy">Energy to regain</param>
        public static void GainEnergy(Character target, int energy)
        {
            target.GainEnergy(energy);
        }
        #endregion
    }
}

