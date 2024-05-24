using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;


public class NewManager : MonoBehaviour
{
    public TextMeshProUGUI ClicksTotalText;
    public TextMeshProUGUI CountdownText;
    public float clickCooldownDuration = 5f;
    private float clickCooldownTimer = 0f;
    private int TotalClicks = 0;
    public Button ClickButton; // Reference to your UI button
    public GameObject ExitButton; // Reference to the second button GameObject
    public GameObject LeaderboardButton;
    public GameObject GamePanel; // Reference to your panel GameObject
    private bool setScoreBool = false;

    void Start()
    {
        clickCooldownTimer = 0f;
    }
    public void Reset()
    {
        clickCooldownTimer = 0f;
        TotalClicks = 0;
        ClickButton.interactable = true;
        // Deactivate the second button GameObject:
        ExitButton.SetActive(false); // Enable the second button
        LeaderboardButton.SetActive(false); // Enable the second button
        setScoreBool = false;
        ClicksTotalText.text = "Score: 0";
    }
    void Update()
    {
        // Only update the countdown if the panel is active
        if (GamePanel.activeSelf)
        {
            clickCooldownTimer += Time.deltaTime;

            if (clickCooldownTimer >= clickCooldownDuration)
            {
                ClickButton.interactable = false; // Disable the button

                HandlerSaveScoreAfterTimer();

                // Activate the second button GameObject:
                ExitButton.SetActive(true); // Enable the second button
                LeaderboardButton.SetActive(true); // Enable the second button

                if (setScoreBool == false)
                {
                    setScoreBool = true;
                }
            }

            float remainingTime = Mathf.Max(0f, clickCooldownDuration - clickCooldownTimer);
            CountdownText.text = $"Countdown: {remainingTime:F1} s";
        }
    }

    private void HandlerSaveScoreAfterTimer()
    {
        int score = TotalClicks;
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("score").SetValueAsync(score);
    }
    public void AddClicks()
    {
        TotalClicks++;
        ClicksTotalText.text = TotalClicks.ToString("Score: 0");
        clickCooldownTimer = 0f;
    }
}
