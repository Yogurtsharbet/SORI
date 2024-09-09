using DG.Tweening;
using DTT.AreaOfEffectRegions;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class IndicatorControl : MonoBehaviour {
    private ArcRegion arcArrow;

    private Quaternion targetRotation;
    private float currentY = 0f;

    public float rotateSpeed = 10f;

    private void Awake() {
        arcArrow = GetComponent<ArcRegion>();
    }

    private void Update() {
        currentY += Time.deltaTime * rotateSpeed;
        targetRotation = Quaternion.LookRotation(Camera.main.transform.forward) * Quaternion.Euler(300f, 0, 0)
            * Quaternion.AngleAxis(currentY, Vector3.up);

        transform.rotation = targetRotation;

        arcArrow.Angle = -currentY;
    }
}
