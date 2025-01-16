using UnityEngine;

[CreateAssetMenu(fileName = "Sword Slash", menuName = "Abilities/Sword Slash")]
public class SwordAbility : Ability
{
    public float damage = 20f;
    [SerializeField] private GameObject newSwordPrefab; 
    private GameObject currentSwordInstance;

    public override void Activate(GameObject player)
    {
        Transform swordTransform = player.transform.Find("TalinDraven/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/rightHand");

        if (swordTransform != null)
        {
            // Destroy the current sword if it exists
            if (currentSwordInstance != null)
            {
                Destroy(currentSwordInstance);
            }

            // Instantiate the new sword with the VFX
            currentSwordInstance = Instantiate(newSwordPrefab, swordTransform.position, swordTransform.rotation);
            currentSwordInstance.transform.SetParent(swordTransform); // Make the new sword a child of the player's hand

            Debug.Log("Sword Slash activated with new sword including VFX!");
        }
        else
        {
            Debug.LogError("Right Hand transform not found in player hierarchy.");
        }
    }
}
