using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // thêm thư viện này để load scene

public class LoginChecker : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    [Header("Message")]
    public TMP_Text messageText; // để hiện thông báo

    // Tài khoản test (hardcode)
    private string correctEmail = "khanhtnse184544@fpt.edu.vn";
    private string correctPassword = "123";

    // Regex để check email
    private Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLoginClick();
        }
    }

    public void OnLoginClick()
    {
        string email = emailField.text.Trim();
        string password = passwordField.text;

        // Kiểm tra rỗng
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Email và Password không được để trống!");
            return;
        }

        // Kiểm tra định dạng email
        if (!emailRegex.IsMatch(email))
        {
            ShowMessage("Email không hợp lệ!");
            return;
        }

        // Kiểm tra thông tin đăng nhập
        if (email == correctEmail && password == correctPassword)
        {
            ShowMessage("Đăng nhập thành công ✅");

            // Chuyển sang HomeScene
            SceneManager.LoadScene("HomeScence");
        }
        else
        {
            ShowMessage("Sai email hoặc mật khẩu ❌");
        }
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;

        Debug.Log(msg);
    }
}
