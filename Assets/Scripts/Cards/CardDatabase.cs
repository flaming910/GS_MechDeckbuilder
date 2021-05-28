using UnityEngine;

namespace MDB.Cards
{
    public class CardDatabase : MonoBehaviour
    {
        private static CardDatabase instance;

        public static CardDatabase Instance
        {
            get => instance;
        }

        private void Awake()
        {
            if (instance == null)
            {
                playerCardList.InitialiseCardList();
                commonEnemyCardList.InitialiseCardList();
                currentPlayerCardList = ScriptableObject.CreateInstance<CardListBase>();
                currentPlayerCardList.InitialiseCardList(playerCardList);
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(gameObject);
            }
        }

        #pragma warning disable 0649
        [SerializeField] private CardListBase playerCardList;
        [SerializeField] private CardListBase commonEnemyCardList;
        #pragma warning restore 0649


        private CardListBase currentPlayerCardList;

        /// <summary> A list of all cards the player can obtain </summary>
        public CardListBase PlayerCards => playerCardList;
        public CardListBase CommonEnemyCards => commonEnemyCardList;
        /// <summary> The player card list for this run </summary>
        public CardListBase CurrentPlayerCardList => currentPlayerCardList;

    }
}


