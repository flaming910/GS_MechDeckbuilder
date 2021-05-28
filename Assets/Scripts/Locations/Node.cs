using MDB.Interfaces;
using MDB.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MDB.Locations
{
    /// <summary> The type of this location. Battle, Elite Battle, Boss Battle, Shop, Gambling room? Enhancement room? </summary>
    public enum LocationType { Battle, NotBattle };

    /// <summary> The type of reward. Gold, Card, Body Part, Upgrade? Enhancement? </summary>
    public enum RewardType { Gold, Card, Part};

    /// <summary> What the reward is and how much of it you can get </summary>
    [Serializable]
    public struct Reward
    {
        //Probably add a sprite to display pictures
        public RewardType type;
        public int[] amountRange;
    }

    /// <summary> Basic locations. Should include type of location and child nodes </summary>
    public class Node : MonoBehaviour, IClickable
    {
        #region Serialised Variables
        #pragma warning disable 0649
        /// <summary> Type of this location. Battle/Shop/Strip Club? </summary>
        [SerializeField]
        private LocationType type;

        /// <summary> Nodes that this node lead to </summary>
        [SerializeField]
        private List<Node> childNodes;

        // Make a List of enemies/events in this location

        /// <summary> Prefab of enemy. I don't know how this will be done later. For now, just a quick prefab </summary>
        [SerializeField]
        private GameObject enemy;

        /// <summary> Whether the player can go to this location </summary>
        [SerializeField]
        private bool isAccessible;

        /// <summary> The rewards you can possibly get from this location </summary>
        [SerializeField]
        private List<Reward> possibleRewards;
        #pragma warning restore 0649


        #endregion

        /// <summary> How many nodes from the root node is this one </summary>
        private int distFromRoot;

        /// <summary> Whether this location has already been visited. To not fight the same battle again? </summary>
        private bool isVisited;

        /// <summary> The node this one extends from </summary>
        private Node parentNode;

        #region Getters
        /// <summary> Whether the player can go to this location </summary>
        public bool IsAccessible { get => isAccessible; set => isAccessible = value; }
        public List<Node> ChildNodes => childNodes;
        public int DistFromRoot => distFromRoot;
        #endregion

        // ReSharper disable twice ParameterHidesMember
        /// <summary>
        /// This allows us to set the distance from root and also the child nodes. Might get changed once auto-gen is made but it'll do the job for now
        /// </summary>
        /// <param name="distFromRoot">How many nodes from the root node is this one</param>
        /// <param name="childNodes">The nodes this connects to</param>
        public void SetValues(int distFromRoot, List<Node> childNodes, GameObject enemy, Node parentNode)
        {
            this.distFromRoot = distFromRoot;
            this.childNodes = childNodes;
            this.enemy = enemy;
            this.parentNode = parentNode;
            type = LocationType.Battle;
        }

        public void DisableOtherChildren(Node nodeToNotDisable)
        {
            foreach (var node in childNodes)
            {
                if (node != nodeToNotDisable) node.IsAccessible = false;
            }
        }

        // <summary> When mouse gets over this node, show possible rewards? </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIManager.Instance.ShowRewardsPreview(possibleRewards);
        }

        /// <summary> When mouse moves away from this node, hide rewards </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.HideRewardsPreview();
        }

        /// <summary> When this node is clicked, mark it as visited and make all its children accessible </summary>g
        public void OnPointerClick(PointerEventData eventData)
        {
            UIManager.Instance.HideRewardsPreview(); // Just to make sure rewards preview is off before we move to other canvas otherwise it gets stuck

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (isAccessible && !isVisited)
                {
                    isVisited = true;

                    if (parentNode)
                    {
                        parentNode.DisableOtherChildren(this);
                    }

                    foreach (var node in childNodes)
                    {
                        node.IsAccessible = true;
                    }

                    switch (type)
                    {
                        case LocationType.Battle:
                            UIManager.Instance.OpenFight(enemy);
                            break;
                        case LocationType.NotBattle:
                            break;
                    }
                }
            }
        }

    }
}
