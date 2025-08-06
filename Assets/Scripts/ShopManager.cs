using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text moneyText;  // Hiển thị số tiền người chơi có
    public Text carNameText;  // Hiển thị tên xe
    public Text carPriceText;  // Hiển thị giá xe
    public Button buyButton;  // Nút mua xe
    public Button backButton;  // Nút quay lại
    public Image carImageDisplay;  // Hiển thị ảnh xe

    public int playerMoney = 1000;  // Tiền người chơi
    public int carPrice = 500;  // Giá của xe
    public string carName = "Xe Đua";  // Tên xe
    public Sprite carImage;  // Hình ảnh của xe

    void Start()
    {
        // Cập nhật UI khi vào cửa hàng
        UpdateUI();

        // Gán sự kiện cho các nút
        buyButton.onClick.AddListener(TryBuyCar);
        backButton.onClick.AddListener(BackToMainMenu);
    }

    void UpdateUI()
    {
        // Cập nhật các thông tin về tiền và xe
        moneyText.text = "Tiền: " + playerMoney.ToString();
        carNameText.text = "Tên Xe: " + carName;
        carPriceText.text = "Giá: " + carPrice.ToString();
        carImageDisplay.sprite = carImage;  // Cập nhật hình ảnh của xe
    }

    void TryBuyCar()
    {
        // Kiểm tra xem người chơi có đủ tiền mua xe không
        if (playerMoney >= carPrice)
        {
            playerMoney -= carPrice;  // Trừ tiền khi mua xe
            UpdateUI();  // Cập nhật lại UI
            Debug.Log("Mua xe thành công!");
        }
        else
        {
            Debug.Log("Không đủ tiền để mua xe!");
        }
    }

    void BackToMainMenu()
    {
        // Ẩn cửa hàng khi người chơi quay lại
        gameObject.SetActive(false);
        Debug.Log("Quay lại màn hình chính!");
    }
}
