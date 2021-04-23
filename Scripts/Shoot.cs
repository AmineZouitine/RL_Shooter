using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Shoot Information")]
    [SerializeField] private Transform m_bulletSpawnPos;
    [SerializeField] private GameObject m_bulletPrefabs;
    [SerializeField] private float m_timeBetweenShoot;
    [SerializeField] private MapGenerator m_mapGenerator;

    public float currentTime { get; set; }


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }

    public void ShootRestart()
    {   
        currentTime = 0f;
    }
    public void FixedUpdate()
    {
        if(m_timeBetweenShoot > 0)
        {
            currentTime -= Time.fixedDeltaTime;
        }
    }
    public void Fire()
    {
        if(currentTime <= 0)
        {
            currentTime = m_timeBetweenShoot;
            Destroy(Instantiate(m_bulletPrefabs, new Vector3(m_bulletSpawnPos.position.x, 1.116333f, m_bulletSpawnPos.position.z), transform.rotation),5);
        }
    }

  
}
