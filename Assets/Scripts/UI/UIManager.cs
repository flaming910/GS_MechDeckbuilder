using System.Collections.Generic;
using MDB.UI;
using TMPro;
using UnityEngine;

namespace MDB.Managers
{
    /// <summary> It manages the UI. Canvases, Texts, Most visuals. </summary>
    public class UIManager : MonoBehaviour
    {

        private static UIManager instance;

        public static UIManager Instance
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
        #pragma warning disable 0649
        #region Prefabs
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private GameObject rewardPreviewPrefab;
        #endregion

        #region Canvases
        [SerializeField] private GameObject mapCanvas;
        [SerializeField] private GameObject fightCanvas;
        [SerializeField] private GameObject rewardsCanvas;
        [SerializeField] private GameObject menuCanvas;

        [SerializeField] private CanvasGroup rewardsPreview;

        /// <summary> Which canvas is currently active so we can disable it? again.. Idk </summary>
        [SerializeField] private GameObject currentCanvas;
        #endregion

        #region Battle fields
        [SerializeField] private Transform playerHand;
        [SerializeField] private Transform playerHealthBars;
        [SerializeField] private TextMeshProUGUI playerEnergyText;

        [SerializeField] private Transform enemyHand;
        [SerializeField] private Transform enemyHealthBars;
        #endregion
        #pragma warning restore 0649

        #region Getters
        public GameObject CardPrefab => cardPrefab;
        public GameObject HealthBarPrefab => healthBarPrefab;

        public Transform PlayerHand => playerHand;
        public Transform PlayerHealthBars => playerHealthBars;
        public TextMeshProUGUI PlayerEnergyText => playerEnergyText;

        public Transform EnemyHand => enemyHand;
        public Transform EnemyHealthBars => enemyHealthBars;
        #endregion

        /// <summary>
        /// Update the UI elements for HP text and bar
        /// </summary>
        /// <param name="currentValue"> The current value of health </param>
        /// <param name="maxValue"> The max value of health </param>
        /// <param name="valueText"> Reference to the visual Text UI Element </param>
        /// <param name="hpBar"> Reference to the visual UI Health Bar </param>
        public void UpdateHealthUI(int currentValue, int maxValue, HealthBar hpBar)
        {
            hpBar.SetHealth(currentValue, maxValue);
        }

        /// <summary>
        /// Updates the UI element for the armor, hiding/unhiding it as needed
        /// </summary>
        /// <param name="armor">Armor value</param>
        /// <param name="hpBar">The health bar that is being affected</param>
        public void UpdateArmorUI(int armor, HealthBar hpBar)
        {
            hpBar.SetArmor(armor);
        }

        /// <summary>
        /// Update the UI elements for energy text and bar
        /// </summary>
        /// <param name="currentValue"> The current value of energy </param>
        /// <param name="maxValue"> The max value of energy </param>
        public void UpdateEnergyUI(int currentValue, int maxValue)
        {
            playerEnergyText.text = currentValue + " / " + maxValue;
        }

        /// <summary> Close current canvas and open Map Canvas. For some reason can't call OpenCanvas through the End Combat Button </summary>
        public void ReturnToMap()
        {
            currentCanvas.SetActive(false);

            mapCanvas.SetActive(true);

            currentCanvas = mapCanvas;
        }

        /// <summary>
        /// Open the fight canvas and set up enemy (enemies?)
        /// </summary>
        /// <param name="enemy"> Enemy to be created </param>
        /// Another parameter for end of battle rewards?
        public void OpenFight(GameObject enemy)
        {
            currentCanvas.SetActive(false);

            fightCanvas.SetActive(true);
            currentCanvas = fightCanvas;

            Instantiate(enemy, Vector3.zero, Quaternion.identity, currentCanvas.transform);
            GameManager.Instance.SetUpFight();
        }

        /// <summary> Close the current canvas and open Rewards Canvas </summary>
        public void OpenRewards()
        {
            Destroy(GameManager.Instance.CurrentEnemyController.gameObject);

            foreach (Transform children in enemyHealthBars)
            {
                Destroy(children.gameObject);
            }

            foreach (Transform children in enemyHand)
            {
                Destroy(children.gameObject);
            }

            currentCanvas.SetActive(false);

            rewardsCanvas.SetActive(true);

            currentCanvas = rewardsCanvas;

            RewardsPanel.Instance.RollCardRewards(3);
        }

        /// <summary>
        /// Show or Hide Rewards Preview Panel.
        /// Also, it should actually accept parameter for what stuff are rewards here to be shown, but soon
        /// </summary>
        public void ShowRewardsPreview(List<MDB.Locations.Reward> possibleRewards)
        {
            foreach (var reward in possibleRewards)
            {
                GameObject go = Instantiate(rewardPreviewPrefab, Vector3.zero, Quaternion.identity, rewardsPreview.transform);
                var texts = go.GetComponentsInChildren<TextMeshProUGUI>();

                texts[0].text = reward.type.ToString();
                if(reward.amountRange[0] == reward.amountRange[1])
                {
                    texts[1].text = reward.amountRange[0].ToString();
                }
                else
                {
                    texts[1].text = reward.amountRange[0] + " - " + reward.amountRange[1];
                }
            }

            rewardsPreview.alpha = 1;
        }

        /// <summary> Hide the rewards preview panel and also destroy the children </summary>
        public void HideRewardsPreview()
        {
            for (int i = rewardsPreview.transform.childCount - 1; i >= 1; i--)
            {
                Destroy(rewardsPreview.transform.GetChild(i).gameObject);
            }

            rewardsPreview.alpha = 0;
        }
    }
}
