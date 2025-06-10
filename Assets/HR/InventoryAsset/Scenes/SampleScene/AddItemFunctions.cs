using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InventorySampleScene
{
    public class AddItemFunctions : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private GameObject addAxe;
        [SerializeField]
        private GameObject addHammer;
        [SerializeField]
        private GameObject addSword;
        [SerializeField]
        private GameObject addIron;
        [SerializeField]
        private GameObject addPotion;
        [SerializeField]
        private GameObject addRuby;
        [SerializeField]
        private GameObject addHat;
        [SerializeField]
        private GameObject addShirt;
        [SerializeField]
        private GameObject addGlove;
        [SerializeField]
        private GameObject addPants;
        [SerializeField]
        private GameObject addShoes;

        public void AddHat()
        {
            addHat.SetActive(true);
        }
        public void AddShirt()
        {
            addShirt.SetActive(true);
        }
        public void AddGlove()
        {
            addGlove.SetActive(true);
        }
        public void AddPants()
        {
            addPants.SetActive(true);
        }
        public void AddShoes()
        {
            addShoes.SetActive(true);
        }

        public void AddAxe()
        {
            addAxe.SetActive(true);
        }
        public void AddHammer()
        {
            addHammer.SetActive(true);
        }
        public void AddSword()
        {
            addSword.SetActive(true);
        }
        public void AddIron()
        {
            addIron.SetActive(true);
        }
        public void AddPotion()
        {
            addPotion.SetActive(true);
        }
        public void AddRuby()
        {
            addRuby.SetActive(true);
        }

        public void AddItemTest_()
        {
            PlayerInvenManager.instance.AddItemToHotBarOrPlayerInventory("Hat");
        }

        public void ClearMain()
        {
            InventoryController.instance.InventoryClear("PlayerInventory");
        }
        public void ClearHotBar()
        {
            InventoryController.instance.InventoryClear("HotBar");
        }
        public void swap(InventoryItem item1, InventoryItem inSlot)
        {
            string item1inv = item1.GetInventory();
            string inSLotInv = inSlot.GetInventory();

            int positem1 = item1.GetPosition();
            int posinslotinv = inSlot.GetPosition();
            InventoryController.instance.RemoveItemPos(inSLotInv, inSlot.GetPosition(), inSlot.GetAmount());

            InventoryController.instance.AddItemPos(item1inv, inSlot.GetItemType(), positem1, inSlot.GetAmount());

            InventoryController.instance.AddItemPos(inSLotInv, item1.GetItemType(), posinslotinv, item1.GetAmount());


        }
    }
}
