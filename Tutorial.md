# Speedometer Tutorial

This tutorial will demonstrate how to create a speedometer in unity

## 1. Setup a new scene and required game objects

Create a new scene named `Speedometer`.

Add a new canvas object to your scene called `GUI`.

Add an empty game object as a child of the canvas we just added calling this `Speedometer`, this will hold all of the speedometer elements.

## 2. Setup the speedometer UI elements

1. Now we need to make a template for the speed labels, to begin with create an empty game obect child named `SpeedLabelTemplate` of the `Speedometer` game object we previously added, this will act as a pivot point for our speed labels, add all the following as child objects.

- Create an UI image game object setting its width to **2** and height to **10**, and setting the _Pos Y_ position to **140** to place it above the pivot point. This will act as a dash line for each of our speed labels.

- Add a text UI element named `speedText` setting it's text property to **100** for template, it's font size to **16**, and it's text alignment to middle and center. Under its Rect Transform set its Pos Y to **120** to position it next to the dash line.\
![SpeedLabel Text Properties](https://user-images.githubusercontent.com/79928221/147616874-40c8d412-4b2e-4b1e-a3a4-e034f59b4d65.png)

2. Next we need to add the needle to our speedometer, to start off with add a UI image game object named `NeedleIMG` as a child of and centered to the `Speedometer` game object, setting its width and height to **125**.

- Set the image's sprite to this needle image included in this repo [[Link]](https://github.com/GriggsGD/UnityUISpeedometer/blob/main/Assets/Speedometer/Textures/Needle.png), if you import this to a 3D project make sure the file's texture type is set to `Sprite(2D and UI)`.

- To be able to have our needle rotate around the speedometer's center properly we will need to change its pivot point, to do this simply go to the Rect Transform isnpector properties of the `NeedleIMG` and set it's Pivot Y property to **0**, this will then change the Pos properties which we need to set back to 0,0,0.\
![Needle rect transform](https://user-images.githubusercontent.com/79928221/147830967-54c2ad85-7559-4749-b710-2ef03414db36.png)

### The speedometer should look like below in the scene
![Needle setup](https://user-images.githubusercontent.com/79928221/147833645-67de234a-9240-4c26-89a9-a4db24b9e2d9.png)

## 3. Create a script to control the speedometer's behaviour

1. Start off by creating a new script called `SpeedometerCtrl`.

- To begin with we need a reference to the object we want to measure the speed of, we can do this by declaring a _rigidbody_ variable named `target`, this will be used to read the velocity of our target object (_only if your target game object is using a rigidbody_). **Make sure to add `[SerializeField]` before the variable so it can be set in the inspector.**
```
  [SerializeField] Rigidbody target;
```
- We need a property/variable to control the maximum speed we want our speedometer to display up to, this is needed to calculte the maximum angle the needle can turn and the last speed label to generate and display...
```
  [SerializeField] float maxSpeed = 150;
```

- We need two variables to control the minimum and maximum angles our needle will turn, and where our speed labels will generate...
```
  [SerializeField] float minNeedleAngle = 130f;
  [SerializeField] float maxNeedleAngle = -130f;
```

- Also we need another to variables to reference and control the needle and speed label template game objects
```
  [SerializeField] Transform needlePivot;
  [SerializeField] GameObject speedLabelTemplate;
```

2. Now let's add functionality to the speedometer needle

- We need to create a method to calculate an angle between the minimum and maximum relative to the speed for the needle...
```
float GetSpeedRotation(float speed, float maxSpeed)
    {
        float totalAngleSize = minNeedleAngle - maxNeedleAngle; // Calculates the total angle the needle can turn

        float speedNormalized = speed / maxSpeed; //Calculates a percentage to apply to the angle by dividing the max speed from the current speed, i.e Current speed = 15mph / max speed = 150mph = 10%

        return speed > maxSpeed ? maxNeedleAngle : minNeedleAngle - speedNormalized * totalAngleSize; //Outputs the method's angle result, if the current speed is over the max speed it clamps the output to the max angle to prevent the angle over shooting
    }
```

- Now in the update method we can use the method we just created to rotate the needle relative to the speed...
```
void Update()
{
  needlePivot.eulerAngles = new Vector3(0, 0, GetSpeedRotation(target.velocity.magnitude * 2.23693629f, maxSpeed)); //Rotates the needle using the method we created above, reading from our target rigidbody and multiplying it to convert it to a miles per hour measure
}
```
  
3. Next lets develop some code to generate our speed labels

- Starting off we need to add an extra variable, an int called `speedLabelAmount` to control how many speed labels to generate
```
[SerializeField] int speedLabelAmount = 15;
```

- Create a new method taking an argument for the max speed to generate the speed labels...
```
void CreateSpeedLabels(float maxSpeed){
}
```

- Inside our new method add a for loop making sure it's condition (2nd statement) is `i <= speedLabelAmount` and not just the default default operator `<` as this will prevent the last label being generated...
```
void CreateSpeedLabels(float maxSpeed){
  for (int i = 0; i <= speedLabelAmount; i++){
  }
}
```
- Inside the for loop add the following code...
```
    GameObject speedLabel = Instantiate(speedLabelTemplate, transform); //Spawns a new speed label
    float labelSpeedNormalized = (float)i / speedLabelAmount; //Calculates an percentage index relative to the label amount for the generated speed label to use to calculate its position and display speed
    float speedLabelAngle = minNeedleAngle - labelSpeedNormalized * totalAngleSize; //Calculates a placement angle for the generated speed label
    speedLabel.transform.eulerAngles = new Vector3(0, 0, speedLabelAngle); //Sets the angle to the speed label calculated above
    speedLabel.GetComponentInChildren<Text>().text = (labelSpeedNormalized * maxSpeed).ToString("0"); //Sets the speed label speed text
    speedLabel.GetComponentInChildren<Text>().transform.eulerAngles = Vector3.zero; //Sets the speed label test to stay upright
    speedLabel.SetActive(true); //Sets the speed label active
```

- After the for loop inside the method we need to add code to place the speedometer needle above our speed labels, if we do not add this the needle will display underneath the speed labels, this is because our script generates new game objects at the start of playmode/when the game start which gets placed underneath our needle in the hierarchy, UI is rendered from top to bottom meaning the last item in the hierarchy of the UI canvas is rendered on top.

The whole method for generating our speed labels should look like the snippet below...
```
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
            speedLabel.GetComponentInChildren<Text>().transform.eulerAngles = Vector3.zero;
            speedLabel.SetActive(true);
        }

        needlePivot.SetAsLastSibling();
    }
```

- Now create an `Awake` method with the following code to call the method we just created to generate our speed labels and siable our template at the start of the game...
```
private void Awake()
  {
    CreateSpeedLabels(maxSpeed); //Calls the method above we created providing the maxSpeed variable as the argument
    speedLabelTemplate.SetActive(false); //Sets the speed label template inactive to stop it displaying on our speedometer
  }
```

## 4. Testing the analog speedometer

1. We need a game object using a rigidbody to traverse through our scene to calculate its speed and to reference our game objects in the `SpeedometerCtrl` script/component

- Place down a plane mesh into the scene called `Ground` scaled by **30** on its X and Z axis, this will act as a ground for our car/object to move around on

- Place a `Car prefab` provided by the Unity Starter Assets [included in this repo](https://github.com/GriggsGD/UnityUISpeedometer/blob/main/Assets/Standard%20Assets/Vehicles/Car/Prefabs) into our scene, this uses a rigidbody component which we can use and reference on our speedometer to calculate and display a speed from

- Move the `Main camera` in the hierarchy as a child of the `Car` (Create a new camera if there is not one in your scene already), setting its XYZ position to **0**,**4**,**-10** to place it behind and above our `Car`, also making sure its rotation is **0** on all axis and scaled to **1** on all axis, this will lock the camera view to our vehicle

- Now select our `Speedometer` game object in our `GUI` canvas and attach the `SpeedometerCtrl` script to it in the inspector, then make sure to connect the `SpeedLabelTemplate` game object to the `Speed Label Template` property, the `NeedleIMG` game object to `Needle Pivot`, and the `Car` game object to `Target`

2. Run/play the scene to test the speed labels are generating and the needle is turning correctly

![Analog Result](https://user-images.githubusercontent.com/79928221/148010986-e1b463fd-d99a-44ae-a986-614a8941dc53.gif)

## 5. Extras: Digital Speedometer Center Display

1. Adding the extra elements to our speedometer UI

- Starting off with our digital center display we need a [Black circle sprite](https://github.com/GriggsGD/UnityUISpeedometer/blob/main/Assets/Speedometer/Textures/BlackCenterDisplay.png) to represent our center display, to do this add another image element called `CenterDisplayIMG` as a child of our `Speedometer` game object making sure its positioned 0,0,0 to center it, setting the sprite linked above, its width and height to **300**, and its image colour alpha property to **128** to give it some transparency

- In the middle of our center display we want a text element to show our speed, add a text UI element called `speedTXT` as a child of the `CenterDisplayIMG`, setting its text to **100** for a preview how the speed will look on our center display, also setting the font size to **50**, alternatively you could use a TextMesh Pro element like in the project included in this repo to add further font effects i.e an underlay

- We need add another text element as a child of the previous game object to display the unit measurement for our speed, call this object `measureTXT`, setting its text to MPH for preview and is font size to 18, again you could use a TextMesh Pro element like in the project included in this repo to add further font effects

- Now we've got the required elements for our center digital display you will notice the needle gets in the way of the display, to fix this we can use a image mask on our needle.
  - Start by adding another image element as a child of our `Speedometer` object calling this `NeedleMask`, set the sprite of this image to the [needle mask png included in this repo](https://github.com/GriggsGD/UnityUISpeedometer/blob/main/Assets/Speedometer/Textures/Speedometer%20Mask.png) with width and height set to **300**, 
  - Connect a `Mask` component to the image object
  - Next drag the `NeedleIMG` game object onto the `NeedleMask` to set it as a child, this will apply the mask to the needle
  - Finally go back to our `NeedleMask` game object and untick `Show Mask Graphic` if it is not already

Your speedometer should look like this\
![Digital Display mask result](https://user-images.githubusercontent.com/79928221/148277576-34c887f7-23f0-470c-9fb6-a4bd3571234c.png)

2. Coding the digital display functionality

- To start off open back up our `SpeedometerCtrl` script

- We need to add some new variables to reference our new game objects we want to manipulate
```
[SerializeField] Text speedTXT; //References the text element on our digital display showing the current speed
[SerializeField] Text speedTypeTXT; //References the text element on our digital display showing the speed unit measure
```
Or if you are using TextMesh Pro use this code...
```
[SerializeField] TextMeshProUGUI speedTXT;
[SerializeField] TextMeshProUGUI speedTypeTXT;
```
Also remember to add `using TMPro;` at the very top of your script to be able to manipulate TextMesh Pro objects!

- Next in our `Update` method add the following code, this will update the text of our `speedTXT` text element with the current speed
```
speedTXT.text = (target.velocity.magnitude * 2.23693629f).ToString("0"); //Updates the text element with our current speed, using "0" in the ToString method will eliminate any decimal places
```

- We will also need to add another variable to reference our `NeedleMask`, this is required as previously we set the needle as the last sibling of our `Speedometer` object so that it displays above our speed labels...
```
[SerializeField] Transform needleRoot;
```

- Now go back to our `CreateSpeedLabels()` method, at the bottom replace `needlePivot.SetAsLastSibling();` with `needleRoot.SetAsLastSibling();`, the method should look like below...
```
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
            speedLabel.GetComponentInChildren<Text>().transform.eulerAngles = Vector3.zero;
            speedLabel.SetActive(true);
        }

        needleRoot.SetAsLastSibling();
    }
```

Remember to save your changes!

3. Testing the digital display

- Back in unity select the `Speedometer` object, go to the inspector properties and connect the `speedTXT` object to the `Speed TXT` property, connect the `measureTXT` object to the `Speed Type TXT` property, and connect the `NeedleMask` object to `Needle Parent`

- Now play your scene and move around, the digital display should show your speed

## 6. Extras: Speed unit customisation

1. Coding the speed unit customisation

- It would be nice to be able to control what unit measure the speedometer uses, to do this we will need some more variables and a new method to the `SpeedometerCtrl` script

- Start off by adding an enum to hold our speed types above our variables...
```
enum SpeedType
  {
      MPH,
      KPH
  }
```

- Add another variable to select our speed type listed in the enum, we will set this to MPH (Miles per hour) as default, you will need a default for it to show in our inspector properties...
```
[SerializeField] SpeedType speedType = SpeedType.MPH; //To prevent confusion SpeedType starting with a captial is the enum we created and the lower case is the new variable < holding its selection
```

- Now lets create a new float method to calculate our desired speed unit measure taking in the target's velocity as an argument...
```
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
```

- With our method created to calculate our speed measure we can apply this to our speedometer elements to display the speed relative to the unit selected
  - Go back to the update method and replace `needlePivot.eulerAngles = new Vector3(0, 0, GetSpeedRotation(target.velocity.magnitude * 2.23693629f, maxSpeed));` with `needlePivot.eulerAngles = new Vector3(0, 0, GetSpeedRotation(CalculateSpeed(target.velocity.magnitude), maxSpeed));`, this will turn the needle relative to our selected speed unit
  - Again in our Update method replace `speedTXT.text = (target.velocity.magnitude * 2.23693629f).ToString("0");` with `speedTXT.text = CalculateSpeed(target.velocity.magnitude).ToString("0");`, this will display the speed relative to our selected speed unit
  - Lastly in the update method add `speedTypeTXT.text = speedType == SpeedType.MPH ? "MPH" : "KPH";`, this will show our current speed unit on our digital display...
Your update method should look like below
```
void Update()
    {
        needlePivot.eulerAngles = new Vector3(0, 0, GetSpeedRotation(CalculateSpeed(target.velocity.magnitude), maxSpeed));
        speedTXT.text = CalculateSpeed(target.velocity.magnitude).ToString("0");
        speedTypeTXT.text = speedType == SpeedType.MPH ? "MPH" : "KPH";
    }
```
Save your changes!

2. Testing

- Go back to unity and select the `Speedometer` object
  - In its inspector properties select your desired speed unit in the `Speed Type` drop down
  - Run your scene and you should see the speedometer display the selected speed type

### MPH Result
![Speedometer MPH result](https://user-images.githubusercontent.com/79928221/148296109-be5f60c1-2a81-4348-86d0-9f15a3297941.gif)

### KPH Result
![Speedometer KPH result](https://user-images.githubusercontent.com/79928221/148294518-a3283e83-2ea5-4666-b91e-8f4a6e5ba0e3.gif)
