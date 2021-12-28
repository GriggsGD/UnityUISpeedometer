using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class SpeedometerCtrl : MonoBehaviour
{
    enum SpeedType
    {
        MPH,
        KPH
    }
    [SerializeField] Rigidbody target;

    [Header("Needle Properties")]
    [SerializeField] float minNeedleAngle = 130f;
    [SerializeField] float maxNeedleAngle = -130f;

    [Header("Label Propetries")]
    [SerializeField] int speedLabelAmount = 15;
    [SerializeField] GameObject speedLabelTemplate;
    [SerializeField] string speedLabelTextElementName = "speedText";

    [Header("Speed Properties")]
    [SerializeField] SpeedType speedType = SpeedType.MPH;
    [SerializeField] float maxSpeed = 150;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI speedTXT;
    [SerializeField] TextMeshProUGUI speedTypeTXT;
    [SerializeField] Transform needlePivot;
    [SerializeField] Transform needleRoot;

    private void Start()
    {
        CreateSpeedLabels(maxSpeed);
    }
    void Update()
    {
        needlePivot.eulerAngles = new Vector3(0, 0, GetSpeedRotation(CalculateSpeed(target.velocity.magnitude), maxSpeed));
        speedLabelTemplate.SetActive(false);
        speedTXT.text = CalculateSpeed(target.velocity.magnitude).ToString("0");
        speedTypeTXT.text = speedType == SpeedType.MPH ? "MPH" : "KPH";
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
            speedLabel.transform.Find(speedLabelTextElementName).eulerAngles = Vector3.zero;
            speedLabel.SetActive(true);
        }

        needleRoot.SetAsLastSibling();
    }
    float GetSpeedRotation(float speed, float maxSpeed)
    {
        float totalAngleSize = minNeedleAngle - maxNeedleAngle;

        float speedNormalized = speed / maxSpeed;

        return speed > maxSpeed ? maxNeedleAngle : minNeedleAngle - speedNormalized * totalAngleSize;
    }
    float CalculateSpeed(float velocity)
    {
        switch (speedType)
        {
            case SpeedType.MPH:
                return velocity * 2.23693629f;
            case SpeedType.KPH:
                return velocity * 3.6f;
        }
        return 0;
    }
}
