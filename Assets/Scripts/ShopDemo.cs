using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopDemo : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public Sprite Image;
        public int Price;
        public bool IsPurchased = false;
    }

    [SerializeField] private List<ShopItem> ShopItemList;
    [SerializeField] private Transform ShopScrollView;
    private GameObject ItemTemplate;
    private GameObject g;

    void Start()
    {
        // Lưu trữ mẫu item (đối tượng đầu tiên trong ScrollView)
        ItemTemplate = ShopScrollView.GetChild(0).gameObject;

        // Lặp qua danh sách các món đồ và khởi tạo chúng
        for (int i = 0; i < ShopItemList.Count; i++)
        {
            g = Instantiate(ItemTemplate, ShopScrollView);
            UpdateItemUI(g, ShopItemList[i], i); // Gọi phương thức để cập nhật UI của món đồ
        }

        // Xóa item mẫu vì không cần thiết nữa
        Destroy(ItemTemplate);
    }

    // Phương thức cập nhật UI cho mỗi món đồ trong cửa hàng
    private void UpdateItemUI(GameObject item, ShopItem shopItem, int index)
    {
        // Cập nhật ảnh của món đồ
        item.transform.GetChild(0).GetComponent<Image>().sprite = shopItem.Image;

        // Cập nhật giá tiền
        Transform priceTextTransform = item.transform.GetChild(1).GetChild(0); // Đảm bảo chỉ mục đúng
        Text priceText = priceTextTransform.GetComponent<Text>();

        if (priceText != null)
        {
            priceText.text = shopItem.Price.ToString();  // Cập nhật giá vào Text
        }
        else
        {
            Debug.LogError("Không thể tìm thấy Text để cập nhật giá!");
        }

        // Lấy nút mua và thiết lập tính tương tác
        Button purchaseButton = item.transform.GetChild(2).GetComponent<Button>();
        purchaseButton.interactable = !shopItem.IsPurchased;

        // Thêm sự kiện khi người dùng nhấn nút mua
        purchaseButton.onClick.AddListener(() => OnItemPurchased(index));
    }


    // Phương thức xử lý khi mua món đồ
    private void OnItemPurchased(int index)
    {
        // Đánh dấu món đồ là đã mua
        ShopItemList[index].IsPurchased = true;

        // Lấy đối tượng UI của món đồ và vô hiệu hóa nút mua
        Transform item = ShopScrollView.GetChild(index);
        Button purchaseButton = item.transform.GetChild(2).GetComponent<Button>();
        purchaseButton.interactable = false;

        // Tùy chọn: Bạn có thể hiển thị thông báo hoặc thay đổi hình ảnh khi món đồ đã được mua
    }
}