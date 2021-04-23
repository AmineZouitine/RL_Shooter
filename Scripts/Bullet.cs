using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Bullet : MonoBehaviour
{
    [Header("Bullet info")]
    [SerializeField, Range(0f, 200f)] private float m_bulletSpeed;
    [SerializeField] private GameObject m_destroyEffect;
    [SerializeField] private int m_maxBouce;


    private Vector3 direction;
    private float m_distanceEachFrame;
    private int m_currentBouce;

    private void Start()
    {
        m_currentBouce = 0;
        direction = Vector3.forward;
    }
    private void Update()
    {
        m_distanceEachFrame = m_bulletSpeed * Time.deltaTime;
        CheckReflection();
        Move();
    }
    private void Move()
    {
        Vector3 velocity = (direction * m_distanceEachFrame);
        transform.Translate(velocity);
    }
    private void CheckReflection()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_distanceEachFrame + 0.01f))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<EnemyDetection>().Explose();
            }
            else
            {
                BulletReflection(hit);
            }
        }
    }


    private void BulletReflection(RaycastHit hit)
    {
       
        transform.position = hit.point;
        Vector3 reflection = Vector3.Reflect(transform.forward, hit.normal);
        float rotation = 90 - Mathf.Atan2(reflection.z, reflection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, rotation, 0);
        m_currentBouce++;
        if (m_currentBouce >= m_maxBouce)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(Instantiate(m_destroyEffect, transform.position, Quaternion.identity), 2);
        Destroy(gameObject);
    }
}
