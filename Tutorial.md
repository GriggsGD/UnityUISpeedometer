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

- Also we need another two variables to control the minimum and maximum angles our needle will turn, and where our speed labels will generate...
```
  [SerializeField] float minNeedleAngle = 130f;
  [SerializeField] float maxNeedleAngle = -130f;
```

3. Now let's add functionality to the speedometer needle

- We need to create a method to calculate an angle between the minimum and maximum relative to the speed for the needle...
```
float GetSpeedRotation(float speed, float maxSpeed)
    {
        float totalAngleSize = minNeedleAngle - maxNeedleAngle;

        float speedNormalized = speed / maxSpeed;

        return speed > maxSpeed ? maxNeedleAngle : minNeedleAngle - speedNormalized * totalAngleSize;
    }
```

- We need to add another variable
