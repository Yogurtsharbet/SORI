using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotCloseController : MonoBehaviour {

    public void CloseEnable() {
        gameObject.SetActive(true);
    }

    public void OpenDisEnable() {
        gameObject.SetActive(false);
    }
}
