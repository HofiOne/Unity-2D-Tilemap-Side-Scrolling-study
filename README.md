# Unity-2D-Tilemap-Side-Scrolling-study

This repository contains a simple demo Unity project that was born to compare performance of different solutions implementing a very basic classic side scroller where the scrolled terrain is built up from a Grid + Tilemap + Rigidbody 2D + Tilemap Collider 2D + Composie Collider 2D

It containes 3 scenes
- PlatformMovesScene
  - a very simplified version of the implementation I used originally, this one moves the (kinematic) Rigidbody of the tilemp vis Rigidbody2D.MovePosition
- CameraMovesScene
  - this one is using a Unity built in Camera stack where the camras are moving only with different speed creating the parallax effect (also uses a Cinemachine camera to follow the player according to my special requirements, see bellow in details)
- CVCCameraGhostTargetFollowScene
  - this one uses a special Cinemachine camera setup to be able to follow the player and support the parallax effect at the same time (according to my special requirements, see bellow in details)

A bit more details.

My first implementation contained multiple layers with parallax effect added too and the top most of those layers contained the above mentined tilemap.
In the original version, based on my false assumptions, I simply moved that foremost layer just like the others (that do not have any rigidbodies or colliders attached to) and i did not pay attention on how really the rigidbodies are allowed to be moved.
That led me a very poor performance that is nicely confirmed in Unity profiler.




Related Unity forum discussions:
- https://forum.unity.com/threads/optimizing-even-more-tilemap-colliders.1197712/#post-7658578
- https://forum.unity.com/threads/how-to-move-a-virtual-camera-with-a-target-framing-transposer.1189591/

Special thanks to MelvMay (https://forum.unity.com/members/melvmay.287484/) and Gregoryl (https://forum.unity.com/members/gregoryl.1242385/) to help clarifying my concerns, pointing out my faults in my original implementations and being patient and helpful with an absolute Unity beginer.
