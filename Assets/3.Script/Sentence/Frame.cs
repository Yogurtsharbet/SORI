using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour {
    private int blankCount = 0;
    private bool isActive = false;          //Ȱ��ȭ ����
    private bool isCompelete = false;       //���� �ϼ� ����
    private bool isPersistence = false;     //������

    public int BlankCount { get { return blankCount; } }    
    public bool IsActive { get { return isActive; } }
    public bool IsCompelete { get { return isCompelete; } }
    public bool IsPersistence { get { return isPersistence; } }

    public void SetBlankCount(int count) {
        blankCount = count;
    }

    public void SetActive(bool yn) {
        isActive = yn;
    }

    public void SetCompelete(bool yn) {
        isCompelete = yn;
    }

    public void SetPersistenct(bool yn) {
        isPersistence = yn;
    }
}
