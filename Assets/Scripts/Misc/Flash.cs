using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMaterial;
    [SerializeField] private float restoreDefaultMaterialTime = 0.1f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    public float GetRestoreMaterialTime()
    {
        return restoreDefaultMaterialTime;
    }

    public void GetFlashRoutine()
    {
        StartCoroutine(FlashRoutine());
    }

    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(restoreDefaultMaterialTime);
        spriteRenderer.material = defaultMat;
    }

}
