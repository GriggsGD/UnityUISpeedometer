using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class SpeedometerCtrl : MonoBehaviour
{
    public float minNeedleAngle = 130f;
    public float maxNeedleAngle = -130f;
    public int speedLabelAmount = 10;

    public Transform needlePivot;
    public Transform needleParent;
    public GameObject speedLabelTemplate;
    public TextMeshProUGUI speedTXT;
    public Image brakeIMG;

    public CarController carCtrl;

    private void Start()
    {
        CreateSpeedLabels(carCtrl.MaxSpeed);
    }
    void Update()
    {
        needlePivot.eulerAngles = new Vector3(0, 0, GetSpeedRotation(carCtrl.CurrentSpeed, carCtrl.MaxSpeed));
        speedLabelTemplate.SetActive(false);
        speedTXT.text = carCtrl.CurrentSpeed.ToString("0");
        brakeIMG.enabled = carCtrl.BrakeInput == 1 ? true : false;
    }
    void CreateSpeedLabels(float maxSpeed)
    {
        float totalAngleSize = minNeedleAngle - maxNeedleAngle;

        for (int i = 0; i <= speedLabelAmount; i++)
        {
            GameObject speedLabel = Instantiate(speedLabelTemplate, transform);
            float labelSpeedNormalized = (float)i / speedLabelAmount;
            float speedLabelAngle = minNeedleAngle - labelSpeedNormalized * totalAngleSize;
            speedLabel.transform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabel.GetComponentInChildren<Text>().text = (labelSpeedNormalized * maxSpeed).ToString("0");
            speedLabel.transform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabel.SetActive(true);
        }

        needleParent.SetAsLastSibling();
    }
    float GetSpeedRotation(float speed, float maxSpeed)
    {
        float totalAngleSize = minNeedleAngle - maxNeedleAngle;

        float speedNormalized = speed / maxSpeed;

        return speed > maxSpeed ? maxNeedleAngle : minNeedleAngle - speedNormalized * totalAngleSize;
    }
}
