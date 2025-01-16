using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect effect;
    private Material[] skinnedMaterials;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private bool isDissolving = false;

    void Start()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.sharedMaterials;
        }
    }

    public void TriggerAnimation()
    {
        if (!isDissolving) // Ensure it runs only once
        {
            isDissolving = true;
            StartCoroutine(DissolveAnimation());
        }
    }

    public IEnumerator DissolveAnimation()
    {
        if (effect != null)
        {
            effect.Play();
        }
        if (skinnedMaterials.Length > 0)
        {
            float dissolveCounter = 0;
            while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                dissolveCounter += dissolveRate;
                for (int i = 1; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", dissolveCounter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
