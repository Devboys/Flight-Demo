using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;

public class DashUIHandler : MonoBehaviour
{
    [SerializeField] private VectorReference currentDashTarget = null;
    [SerializeField] private BoolReference currentDashTargetValid = null;
    [SerializeField] private GameObject DashUIPrefab = null;

    private RectTransform dashUIObject;
    private Camera mainCam;

    private void Start()
    {
        dashUIObject = Instantiate(DashUIPrefab, this.transform).GetComponent<RectTransform>();
        dashUIObject.gameObject.SetActive(false);

        mainCam = Camera.main;
    }

    private void Update()
    {
        if(currentDashTargetValid.CurrentValue && !dashUIObject.gameObject.activeSelf)
        {
            dashUIObject.gameObject.SetActive(true);
        }
        else if(!currentDashTargetValid.CurrentValue && dashUIObject.gameObject.activeSelf)
        {
            dashUIObject.gameObject.SetActive(false);
        }

        if (currentDashTargetValid.CurrentValue)
        {
            dashUIObject.position = mainCam.WorldToScreenPoint(currentDashTarget.CurrentValue);
        }
    }
}
