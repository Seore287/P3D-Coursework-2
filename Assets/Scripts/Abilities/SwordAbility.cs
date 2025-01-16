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

            if (currentSwordInstance != null)
            {
                Destroy(currentSwordInstance);
            }


            currentSwordInstance = Instantiate(newSwordPrefab, swordTransform.position, swordTransform.rotation);
            currentSwordInstance.transform.SetParent(swordTransform); 
        }
        else
        {
            Debug.LogError("Right Hand transform not found in player hierarchy.");
        }
    }
}
