using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class RevLimiterUIController : MonoBehaviour
{
    public CarController carCtrler;

    Image revIMG;

    public float minImgFillVal = .1f;
    public float maxImgFillVal = .9f;
    void Start()
    {
        revIMG = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        revIMG.fillAmount = ConvertRevToFillVal(carCtrler.Revs);
    }
    float ConvertRevToFillVal(float revs)
    {
        float totalFillSize = maxImgFillVal - minImgFillVal;

        return minImgFillVal + (revs * totalFillSize);
    }
}
