using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNumber = 0;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue -1);
        
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNumber = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        if (!transform.GetChild(activeSlotIndexNumber).GetComponentInChildren<InventorySlot>())
        {
            ActiveWeapon.Instance.NullifyCurrentWeapon();
            return;
        }
        GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNumber).GetComponent<InventorySlot>().GetWeaponInfo().weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn,ActiveWeapon.Instance.transform.position, Quaternion.identity);

        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
