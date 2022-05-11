using UnityEngine;
using Devboys.SharedObjects.Variables;

public class DashUIHandler : MonoBehaviour
{
    [SerializeField] private TransformReference currentDashTarget = null;
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

    private void LateUpdate()
    {
        //Toggle UI marker active
        if (currentDashTargetValid.CurrentValue != dashUIObject.gameObject.activeSelf)
        {
            dashUIObject.gameObject.SetActive(currentDashTargetValid.CurrentValue);
        }

        //Update UI marker screen position (hover over current dash target)
        if (currentDashTargetValid.CurrentValue)
        {
            Vector3 dashTargetPos = currentDashTarget.GetPosition();
            dashUIObject.position = mainCam.WorldToScreenPoint(dashTargetPos);
        }
    }
}
