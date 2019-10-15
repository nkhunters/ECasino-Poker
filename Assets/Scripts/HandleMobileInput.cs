using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandleMobileInput : MonoBehaviour
{
    //input field object
    [SerializeField] TMP_InputField tmpInputField, otpInputField, emailInputField, passwordInputField, registerNameInputField, registerEmailInputField, registerPasswordInputField;
    [SerializeField] Button continueButton, submitOtpButton, incorrectDialogOkButton, loginButton, registerButton;
    [SerializeField] GameObject loginCanvas, enterMobileCanvas, enterOtpCanvas, registerCanvas, incorrectDialog;
    [SerializeField] TMP_Text otp_sent_to_text, login_info_text, register_info_text;
    [SerializeField] string login_api_url, register_api_url;
    [SerializeField] Toggle rememberMe;
    /*FirebaseAuth firebaseAuth;
    PhoneAuthProvider phoneAuthProvider;*/
    private string verificationId;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    // Start is called before the first frame update
    void Start()
    {
        tmpInputField.onValueChanged.AddListener(OnValueChange);
        otpInputField.onValueChanged.AddListener(OnOtpValueChange);

        if (PlayerPrefs.GetInt("remember_me") == 1)
        {
            rememberMe.isOn = true;
            string email = PlayerPrefs.GetString("email");
            string password = PlayerPrefs.GetString("password");
            emailInputField.text = email;
            passwordInputField.text = password;
        }
    }

    private void OnOtpValueChange(string text)
    {
        if (text.Length < 6)
        {
            submitOtpButton.interactable = false;
        }

        if (text.Length >= 6)
        {
            submitOtpButton.interactable = true;
        }

        if (text.Length > 6)
        {
            otpInputField.text = text.Substring(0, 6);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChange(string text)
    {
        Debug.Log("OnValueChange event received. New text is [" + text.Length + "].");

        if (text.Length < 10)
        {
            continueButton.interactable = false;
        }

        if (text.Length >= 10)
        {
            continueButton.interactable = true;
        }

        if (text.Length > 10)
        {
            tmpInputField.text = text.Substring(0, 10);
        }
    }

    public void sendOtp()
    {
        Debug.Log("In Send Otp");
        continueButton.interactable = false;

        string mobileNumber = "+91"+tmpInputField.text;
        Debug.Log(mobileNumber);

        otp_sent_to_text.text = "OTP sent to " + mobileNumber;
        enterMobileCanvas.SetActive(false);
        enterOtpCanvas.SetActive(true);

        /*firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        phoneAuthProvider = PhoneAuthProvider.GetInstance(firebaseAuth);
        phoneAuthProvider.VerifyPhoneNumber(mobileNumber, 300000, null,
          verificationCompleted: (credential) => {
              // Auto-sms-retrieval or instant validation has succeeded (Android only).
              // There is no need to input the verification code.
              // `credential` can be used instead of calling GetCredential().

              signInUserWithCredential(credential);
              return;

  },
          verificationFailed: (error) => {
              // The verification code was not sent.
              // `error` contains a human readable explanation of the problem.
              Debug.Log("Error - "+error);
          },
          codeSent: (id, token) => {
              // Verification code was successfully sent via SMS.
              // `id` contains the verification id that will need to passed in with
              // the code from the user when calling GetCredential().
              // `token` can be used if the user requests the code be sent again, to
              // tie the two requests together.
              Debug.Log("Code sent");
              verificationId = id;
             
          },
          codeAutoRetrievalTimeOut: (id) => {
      // Called when the auto-sms-retrieval has timed out, based on the given
      // timeout parameter.
      // `id` contains the verification id of the request that timed out.
  });*/
    }

    public void submitOtp()
    {
        Debug.Log("In Submit Otp");
        submitOtpButton.interactable = false;

        string otp = otpInputField.text;
        Debug.Log("otp - "+otp);
        Debug.Log("id - " + verificationId);
        /*Credential credential = phoneAuthProvider.GetCredential(verificationId, otp);
        signInUserWithCredential(credential);*/
        if (otp == "123456")
        {
            enterOtpCanvas.SetActive(false);
            registerCanvas.SetActive(true);
        }
        else
        {
            incorrectDialog.SetActive(true);
            submitOtpButton.interactable = true;
        }
    }

    /*private void signInUserWithCredential(Credential credential)
    {
        Debug.Log("in credential");
        firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " +
                               task.Exception);
                incorrectDialog.SetActive(true);
                submitOtpButton.interactable = true;
                return;
            }

           

            *//*FirebaseUser newUser = task.Result;
            Debug.Log("User signed in successfully");
            // This should display the phone number.
            Debug.Log("Phone number: " + newUser.PhoneNumber);
            // The phone number providerID is 'phone'.
            Debug.Log("Phone provider ID: " + newUser.ProviderId);

            PlayerPrefs.SetString("mobile", newUser.PhoneNumber);*//*

        });

        
    }*/

    public void dismissIncorrectDialog()
    {
        incorrectDialog.SetActive(false);
    }

    public void goBackFromOtpScreen()
    {
        enterOtpCanvas.SetActive(false);
        enterMobileCanvas.SetActive(true);
    }

    public void goToSignUp()
    {
        loginCanvas.SetActive(false);
        enterMobileCanvas.SetActive(true);
    }

    public void goToLogin()
    {
        enterMobileCanvas.SetActive(false);
        loginCanvas.SetActive(true);
    }

    public void registerUser()
    {
        registerButton.interactable = false;
        string name = registerNameInputField.text, email = registerEmailInputField.text, mobile = PlayerPrefs.GetString("mobile"), password = registerPasswordInputField.text;
        if (name.Length == 0)
        {
            register_info_text.text = "Please enter name...";
            registerButton.interactable = true;
            return;
        }
        if (email.Length == 0)
        {
            register_info_text.text = "Please enter email...";
            registerButton.interactable = true;
            return;
        }

        if (! validateEmail(email))
        {
            register_info_text.text = "Please enter a valid email...";
            registerButton.interactable = true;
            return;
        }

        if (password.Length == 0)
        {
            register_info_text.text = "Please enter password...";
            registerButton.interactable = true;
            return;
        }

        register_info_text.text = "Signing in...";
        StartCoroutine(sendRegisterRequest(name, email, mobile, password));
    }

    public void loginUser()
    {
        loginButton.interactable = false;
        string email = emailInputField.text;
        string password = passwordInputField.text;
        if(email.Length == 0)
        {
            login_info_text.text = "Please enter email...";
            loginButton.interactable = true;
            return;
        }
        if (! validateEmail(email))
        {
            login_info_text.text = "Please enter valid email...";
            loginButton.interactable = true;
            return;
        }
        if (password.Length == 0)
        {
            login_info_text.text = "Please enter password...";
            loginButton.interactable = true;
            return;
        }
        login_info_text.text = "Logging in...";
        StartCoroutine(sendLoginRequest(email, password));
    }

    IEnumerator sendLoginRequest(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(login_api_url, form);
        yield return www.SendWebRequest();
        LoginRegisterResponse response = JsonUtility.FromJson<LoginRegisterResponse>(www.downloadHandler.text);
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            login_info_text.text = response.message;
            loginButton.interactable = true;
        }
        else
        {
            Debug.Log("Login complete!");
            if(response.status)
            {
                login_info_text.text = "";
                PlayerPrefs.SetInt("user_id", response.id);
                PlayerPrefs.SetString("email", email);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.SetString("user_name", response.name);
                PlayerPrefs.SetInt("remember_me", 0);
                if(rememberMe.isOn)
                    PlayerPrefs.SetInt("remember_me", 1);
                SceneManager.LoadScene("Main Scene");
            }
            else
            {
                login_info_text.text = response.message;
                loginButton.interactable = true;
            }
        }
    }

    IEnumerator sendRegisterRequest(string name, string email, string mobile, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("name" ,name);
        form.AddField("email", email);
        form.AddField("mobile", mobile);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(register_api_url, form);
        yield return www.SendWebRequest();

        LoginRegisterResponse response = JsonUtility.FromJson<LoginRegisterResponse>(www.downloadHandler.text);
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            register_info_text.text = response.message;
            registerButton.interactable = true;
        }
        else
        {
            Debug.Log("Register complete!");
            if (response.status)
            {
                register_info_text.text = "";
                PlayerPrefs.SetInt("user_id", response.id);
                PlayerPrefs.SetString("email", email);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.SetString("user_name", response.name);
                PlayerPrefs.SetInt("remember_me", 0);
                SceneManager.LoadScene("Main Scene");
            }
            else
            {
                register_info_text.text = response.message;
                registerButton.interactable = true;
            }
        }

    }

    public static bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }

    [Serializable]
    public class LoginRegisterResponse
    {
        public bool status;
        public string message, name, email;
        public int id;
    }
}
