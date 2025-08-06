using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using NUnit.Framework;
using Microsoft.Unity.VisualStudio.Editor;
using Image = UnityEngine.UI.Image;

public class Shop : MonoBehaviour
{
    [System.Serializable] class ShopItem
    {
        public Sprite Image;
        public int Price;
        public bool IsPurchased = false;
    }

    [SerializeField] List<ShopItem> ShopItemsList;

    GameObject ItemTemplate;
    GameObject g;
    [SerializeField] Transform ShopScollView;
    Button buyBtn;


    void Start()
    {
        ItemTemplate = ShopScollView.GetChild (0).gameObject;

        int len = ShopItemsList.Count;
        for (int i = 0; i < len; i++)
        {
            g = Instantiate(ItemTemplate, ShopScollView);
            g.transform.GetChild(0).GetComponent<Image>().sprite = ShopItemsList[i].Image;
            g.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = ShopItemsList[i].Price.ToString();
            buyBtn = g.transform.GetChild(2).GetComponent<Button>();
            buyBtn.interactable = !ShopItemsList[i].IsPurchased;
            buyBtn.AddEvenListener(i,OnShopItemBtnClicked );
        }
        Destroy(ItemTemplate);
    }
    void OnShopItemBtnClicked(int itemIndex)
    {
        Debug.Log(itemIndex);
    }
}
