# Speedometer Tutorial

This tutorial will demonstrate how to create a speedometer in unity

## 1. Setup a new scene and required game objects

Create a new scene named `Speedometer`.

Add a new canvas object to your scene called `GUI`.

Add an empty game object as a child of the canvas we just added calling this `Speedometer`, this will hold all of the speedometer elements.

## 2. Setup the speedometer UI elements

Now we need to make a template for the speed labels, to begin with create an empty game obect child named `SpeedLabelTemplate` of the `Speedometer` game object we previously added, this will act as a pivot point for our speed labels, add all the following as child objects.

Create an image UI element setting the sprite as the [SquareSprite](https://github.com/GriggsGD/UnityUISpeedometer/blob/main/Assets/Speedometer/Textures/SquareSprite.png), setting its width to **2** and height to **10**, and setting the _Pos Y_ position to **140** to place it above the pivot point.

Add a text UI element named `speedText` setting it's text property to **100** for template, it's font size to **16**, and it's text alignment to middle and center. Under its Rect Transform set its Pos Y position to **120** again to place it above the pivot point next to the dash line.

![SpeedLabel Text Properties](https://user-images.githubusercontent.com/79928221/147616874-40c8d412-4b2e-4b1e-a3a4-e034f59b4d65.png)

