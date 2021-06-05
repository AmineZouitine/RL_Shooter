# 🔫 Welcome to the RL_Shooter ! 🔫


https://user-images.githubusercontent.com/53370597/116051423-1db2d180-a668-11eb-8df7-dd9e61381641.mp4

In this RL project on Unity, the little green robot (the agent) has to survive as long as possible in front of red robots that run at him. If he is in contact with one of them, he loses. The agent can shoot a green bullet that bounces on the walls (5 times max), he can shoot every 2 seconds, moreover he can move and rotate freely.


 
## Random card generation
![FotoJet](https://user-images.githubusercontent.com/53370597/115955055-e9160d00-a4e3-11eb-8fb6-3fb8534ad366.jpg)
The map can be totally randomly generated, choosing obviously its size, the obstacle ratio (0 to 1), the number of enemies per second, the size of the obstacles etc..

## Enemy chases the agent with a Navmesh

![Nav](https://user-images.githubusercontent.com/53370597/115955179-9ee15b80-a4e4-11eb-9031-187e0668fb2b.PNG)
Often in this kind of situation where the map is randomly generated and we want entities to be able to move in a natural way from a point A to a point B, the NavMesh is often the best solution (even if quite expensive)

## Shooting
https://user-images.githubusercontent.com/53370597/116367721-2980ce00-a7f7-11eb-8623-72bd56693e7b.mp4

The agent can shoot a bullet every 2 seconds, this one must bounce in a realistic way in the environment.
After a lot of tests, my last test seems to work the best, even if I'm thinking about another logic that could avoid sometimes some hazardous behavior.
```csharp
 private void BulletReflection(RaycastHit hit)
    {
       
        transform.position = hit.point;  
        Vector3 reflection = Vector3.Reflect(transform.forward, hit.normal);
        float rotation = 90 - Mathf.Atan2(reflection.z, reflection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, rotation, 0);
        m_currentBouce++; // We can use a Properties with an if in the set
        if (m_currentBouce >= m_maxBouce)
        {
            DestroyBullet();
        }
    }
```
