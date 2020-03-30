# Supine: Camera Documentation

# Configuring camera angle and view



- Main Camera is a child of Camera, so transform can be independently changed to choose best view
- From some experimentation, the set up show in the screenshot works best.
  ![Camera](https://i.imgur.com/Pqx5S11.png)
  - Rotated on the x-axis by 60 degrees gives a good iso view point that looks down on the scene so walls are less likely to need to be hidden, while also showing off the scene well and the models
  - Rotated 155 degrees on the y axis gives us a diagonal view of the scene, making rooms feel larger and easier to move around. Again, this also means walls get in the way of the camera less often. 
  - Position is altered to account for new rotations so player remains in the middle of the screen and player can easily see everything happening around them

# Translating 3D rooms to 2D maps for player 2
- Will need to conduct user testing with some mockups to determine which option is best, but for now there are 2 potential ways we can translate the world to a 2d map
- Both options translate the 3d world to a top down 2D map; this will make icons easier, and be easier to implement successfully in the webapp
  - **OPTION 1**
    - Maps are shown strictly top down, with no angle 
    - Maps aren’t rotated to match the camera, instead the walls remain parallel to the edge of the canvas
    - This will look better in the webapp and will allow for the room to take up more space in the canvas, giving a better, larger view
  - **OPTION 2**
    - Maps are shown strictly top down, with no angle 
    - Maps are rotated on the Y axis to match the rotation of the camera in the 3d game	
    - This will be easier for young children to understand as it better mirrors the 3d game. Changing both the perspective from 3D to 2D and changing the orientation of the scene may be too confusing, and players may fail to see how the two scenes are related and influence one another
