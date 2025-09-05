using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TogglePassword : MonoBehaviour
{
    [Header("Input Field")]
    public TMP_InputField passwordField;

    [Header("Button & Icon")]
    public Button toggleButton;
    public Image iconImage;
    public Sprite showIcon;  // Prefab/icon mắt mở
    public Sprite hideIcon;  // Prefab/icon mắt đóng

    private bool isPasswordHidden = true;

    void Start()
    {
        // Gán sự kiện click
        toggleButton.onClick.AddListener(TogglePasswordView);

        // Đặt mặc định là password
        SetPasswordMode(true);
    }

    void TogglePasswordView()
    {
        isPasswordHidden = !isPasswordHidden;
        SetPasswordMode(isPasswordHidden);
    }

    void SetPasswordMode(bool hide)
    {
        if (hide)
        {
            passwordField.contentType = TMP_InputField.ContentType.Password;
            passwordField.asteriskChar = '●';
            iconImage.sprite = showIcon; // mắt mở
        }
        else
        {
            passwordField.contentType = TMP_InputField.ContentType.Standard;
            iconImage.sprite = hideIcon; // mắt đóng
        }

        // Bắt buộc cập nhật lại hiển thị text
        passwordField.ForceLabelUpdate();
    }
}
