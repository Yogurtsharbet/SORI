using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenMainController : MonoBehaviour
{
    public void OpenInvenMainContainer() {
        gameObject.SetActive(true);
    }

    public void CloseInvenMainContainer() {
        gameObject.SetActive(false);
    }
}
