using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MDB.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private GameObject armorObject;
        [SerializeField] private TextMeshProUGUI armorText;
        [SerializeField] private TextMeshProUGUI nameText;

        public void InitialiseSlider(int currentValue, int maxValue)
        {
            healthSlider.maxValue = maxValue;
            healthSlider.value = currentValue;
            healthText.text = currentValue + " / " + maxValue;
        }

        public void SetName(string name)
        {
            nameText.text = name;
        }

        public void SetHealth(int currentValue, int maxValue)
        {
            healthSlider.value = currentValue;
            healthText.text = currentValue + " / " + maxValue;
        }

        public void SetArmor(int armor)
        {
            if (armor == 0)
            {
                armorObject.SetActive(false);
                armorText.text = 0.ToString();
            }
            else
            {
                armorObject.SetActive(true);
                armorText.text = armor.ToString();
            }
        }

    }
}


