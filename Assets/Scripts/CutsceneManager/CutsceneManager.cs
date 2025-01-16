using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineVirtualCamera vCam1; 
    public CinemachineVirtualCamera talinVCamCloseUp; 
    public CinemachineVirtualCamera lyraVCamCloseUp;  

    [Header("Dialogue UI")]
    public TMP_Text dialogueText;
    public TMP_Text characterNameText;
    public GameObject dialogueBox;
    public Button dialogueButton;

    [Header("Dialogue Settings")]
    [TextArea(2, 4)]
    public string[] characterNames; 
    [TextArea(5, 8)]
    public string[] dialogueLines;  
    public float textSpeed = 0.05f;
    public int standUpLineIndex = 13;  // The index where Talin will stand up

    [Header("Audio Settings")]
    public AudioSource backgroundForestSound;
    public string Scene1;

    [Header("Animation Settings")]
    public Animator playerAnimator;
    private bool isKneeling = true;

    private int currentCutsceneLine = 0;
    private bool isTyping = false;

    private void Start()
    {
        if (backgroundForestSound != null)
        {
            backgroundForestSound.Play();
        }

        dialogueBox.SetActive(true);
        dialogueButton.onClick.AddListener(OnNextButtonClicked);
        dialogueButton.gameObject.SetActive(false);

        PlayKneelAnimation();  
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // Start with the wide shot
        SwitchToNeutralCamera();
        yield return StartCoroutine(DisplayDialogue(characterNames[currentCutsceneLine], dialogueLines[currentCutsceneLine]));
    }

    private void OnNextButtonClicked()
    {
        if (isTyping) return;

        currentCutsceneLine++;

        if (currentCutsceneLine < characterNames.Length)
        {
            string currentSpeaker = characterNames[currentCutsceneLine];
            string currentDialogue = dialogueLines[currentCutsceneLine];

            if (currentSpeaker == "---")
            {
                SwitchToNeutralCamera();
                characterNameText.text = "";
                dialogueText.text = "";
                StartCoroutine(DisplayDialogue(currentSpeaker, currentDialogue));
                dialogueButton.gameObject.SetActive(true);
            }
            else if (currentSpeaker == "Talin")
            {
                if (currentCutsceneLine == standUpLineIndex && isKneeling)
                {
                    PlayStandingAnimation();
                }

                SwitchToCloseUp(talinVCamCloseUp);
                StartCoroutine(DisplayDialogue(currentSpeaker, currentDialogue));
            }
            else if (currentSpeaker == "Elder Lyra")
            {
                SwitchToCloseUp(lyraVCamCloseUp);
                StartCoroutine(DisplayDialogue(currentSpeaker, currentDialogue));
            }
        }
        else
        {
            EndCutscene();
        }
    }

    private IEnumerator DisplayDialogue(string speaker, string dialogue)
    {
        characterNameText.text = speaker;  
        dialogueText.text = "";
        isTyping = true;
        dialogueButton.gameObject.SetActive(false);

        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        dialogueButton.gameObject.SetActive(true);
    }

    private void SwitchToCloseUp(CinemachineVirtualCamera activeCamera)
    {
        vCam1.gameObject.SetActive(false);
        talinVCamCloseUp.gameObject.SetActive(false);
        lyraVCamCloseUp.gameObject.SetActive(false);

        activeCamera.gameObject.SetActive(true);
    }

    private void SwitchToNeutralCamera()
    {
        talinVCamCloseUp.gameObject.SetActive(false);
        lyraVCamCloseUp.gameObject.SetActive(false);

        vCam1.gameObject.SetActive(true);
    }

    private void PlayKneelAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsKneeling", true);
            //isKneeling = true;
        }
    }

    private void PlayStandingAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsKneeling", false);
            playerAnimator.SetTrigger("Stand");

            StartCoroutine(TransitionToIdleAnimation());

            Debug.Log("Standing animation triggered");
        }
    }

    private IEnumerator TransitionToIdleAnimation()
    {
        // Assuming the "Stand" animation is 1 second long, wait for its duration
        yield return new WaitForSeconds(1.0f);

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Idle");
        }
    }

        private void EndCutscene()
    {
        if (backgroundForestSound != null)
        {
            backgroundForestSound.Stop();
        }

        dialogueBox.SetActive(false);
        characterNameText.text = "";
        dialogueText.text = "";

        if (!string.IsNullOrEmpty(Scene1))
        {
            SceneManager.LoadScene(Scene1);
        }
        else
        {
            Debug.LogError("Next scene name is not set.");
        }

        Debug.Log("Cutscene ended. Proceeding with the game...");
    }
}
