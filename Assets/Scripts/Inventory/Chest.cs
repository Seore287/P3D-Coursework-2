using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Item Properties")]
    public Transform itemSpawnPoint;
    public GameObject[] itemPrefab;
    public Item[] item;
    public int[] itemQuantity;

    [Header("Chest Audio Settings")]
    public AudioClip chestAudio;
    private AudioSource chestAudioSource;

    public GameObject uiPrompt;

    private bool isOpened = false;
    private bool playerInRange = false; 

    private PlayerInput playerInput;
    private Animator animator;


    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Gameplay.OpenCloseChest.performed += _ => OnOpenChest();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        chestAudioSource = GetComponent<AudioSource>();

        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!isOpened && uiPrompt != null)
            {
                uiPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(false);
            }
        }
    }

    public void OnOpenChest()
    {
        if (playerInRange && !isOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;

        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false);
        }

        // Play the open animation
        animator.SetTrigger("Open");

        if (chestAudioSource != null)
        {
            chestAudioSource.PlayOneShot(chestAudio);
        }

        // Spawn item
        if (item != null && itemSpawnPoint != null)
        {
            for (int i = 0; i < item.Length; i++)
            {
                GameObject spawnedItem = Instantiate(itemPrefab[i], itemSpawnPoint.position + new Vector3(0, i * 0.5f, 0), Quaternion.identity);
                ItemPickup itemPickup = spawnedItem.GetComponent<ItemPickup>();
                if (itemPickup != null)
                {
                    itemPickup.item = item[i]; // Assign the ScriptableObject data
                    itemPickup.quantity = itemQuantity[i]; // Assign quantity
                }
            }

            
            //int randomIndex = Random.Range(0, item.Length);
            //Instantiate(item[randomIndex], itemSpawnPoint.position, itemSpawnPoint.rotation);
        }
    }
}
