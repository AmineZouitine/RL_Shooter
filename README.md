# Welcome to the Shooter-AI !

https://user-images.githubusercontent.com/53370597/115968388-47181400-a527-11eb-9258-1b2cceaec859.mp4

In this RL project on Unity, the little green robot (the agent) has to survive as long as possible in front of red robots that run at him. If he is in contact with one of them, he loses. The agent can shoot a green bullet that bounces on the walls (5 times max), he can shoot every 2 seconds, moreover he can move and rotate freely.


 
## Random card generation
![FotoJet](https://user-images.githubusercontent.com/53370597/115955055-e9160d00-a4e3-11eb-8fb6-3fb8534ad366.jpg)
The map can be totally randomly generated, choosing obviously its size, the obstacle ratio (0 to 1), the number of enemies per second, the size of the obstacles etc..

## Enemy chases the agent with a Navmesh

![Nav](https://user-images.githubusercontent.com/53370597/115955179-9ee15b80-a4e4-11eb-9031-187e0668fb2b.PNG)
Often in this kind of situation where the map is randomly generated and we want entities to be able to move in a natural way from a point A to a point B, the NavMesh is often the best solution (even if quite expensive)

Video : https://youtu.be/SzOyQpriOl8 (A hd video is coming soon)


