### INFO 5340 / CS 5650: Virtual and Augmented Reality - Fall 2019

# Assignment 1

Read: [Assignment Instructions](https://docs.google.com/document/d/1La3bKARSi58KifaHSFowvJsRRt2wocoFOD25dy8ox_Q/edit?usp=sharing "Detailed Assignment Instructions")

**Remember that you also have to submit information through Canvas before the submission deadline**

<hr>

### Student Name:

Kushal Singh


### Student NetID:

ks2377

<hr>

### Solution

In addition to your code in this repository, please submit screen recordings showing your solutions in action for each part of the assignment. Host the videos on your Cornell Google Drive account and provide a link as instructed below. They have to be accessible to other Cornell accounts, streamable and downloadable (mp4/mov).

**Screen Recording: Part 1**
https://drive.google.com/open?id=1vg4PNpvJh4zRldYAPL6sljrswHottKuH
Filename: a1-2019-part1-\ks2377\>.*
  
**Screen Recording: Part 2**
https://drive.google.com/open?id=1-zrMmt3UzCeVXrKuJlxoSFuVKmgJdo-2
Filename: a1-2019-part2-\ks2377\>.*

**Screen Recording: Part 3**
https://drive.google.com/open?id=1xa3h1qdkyS_fQLNJc8tzNoU7m9NkQQ4E
Filename: a1-2019-part3-\ks2377\>.*

**Screen Recording: Part 4**
https://drive.google.com/open?id=1DHc9_xN-hQWVxNB4PRZgaG16ex8he6q1
Filename: a1-2019-part4-\ks2377\>.*

**Screen Recording: Part 5**

[Insert a link to Google Drive-hosted video]
https://drive.google.com/open?id=11g0-YFN_1t_dDQLPCAA3NzOVYnpxQ5OU
Filename: a1-2019-part5-\ks2377\>.*

<hr>

### Writeup

**Work Summary**

[Write a short summary of your approach to this assignment and list the main challenges you faced]

I approached this assignment in the way that was recommended and outlined by the spec. Specifically, I worked on each part (Part 1, 2, 3, 4, 5) sequentially before moving on to the next part.

One of the primary challenges that I faced was the initial learning curve for developing in Unity. However, Unity has a lot of good online documentation regarding its APIs, which I was able to use in a lot of my scripts.

Specifically, for part 1, the main challenge I faced was getting the slope to rotate on the end as opposed to the center. I was able to accomplish this by following the instructions and selecting the Pivot mode for the Handle.

For part 2, the main challenge I faced was generating the random Spark particles because the Spark particle was being instantiated at a really large position vector, leading to an overflow error for which I received an Invalid AABB expression. I was able to address this through thorough debugging.

For part 3, the main challenge I faced was figuring out how the different functions to have the camera track the object all came together. I was able to solve this by reading through the Unity API documentation on all the functions.

For part 4, the main challenge I faced was figuring out how to transform the cube into the sphere. Originally, I was trying to transform the cube through a MeshRenderer, but this was proving too difficult to implement. So, I switched my implementation and placed an empty Player Ball within the Transformation Sphere and set it inactive at first in the Start() function. When I detected the player cube's entry into the Transformation Sphere via the OnTriggerEnter function, then I set the player cube's status as false, destroyed the player cube, and activated the player ball. 

**Final Five**

[If you implemented the Final Five part, tell us about your solution, what you did and why]

For this stage, I implemented a sound feature in both scenes when the player ball hit the trophy. I did this because it mimicked effects that I previously encountered in my game play experience. Personally, I find satisfaction when I hear a sound after I've hit a particular milestone or objective, which is what I was trying to emulate by adding the sound feature to the trophies. 
