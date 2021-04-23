using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), DisallowMultipleComponent]
public class AgentMouvement : MonoBehaviour
{
    [SerializeField, Range(0,50f)] private float m_speed;
    [SerializeField, Range(100f,1000f)] private float m_rotateSpeed;

    private Rigidbody m_rigidbody;
    private Vector3 m_lastDirection;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>(); 
    }

    public void Mouvement(float _mouvementX, float _mouvementZ, float _rotation)
    {
        m_rigidbody.velocity = Vector3.zero; //Need to be change 
        Vector3 direction = Vector3.zero;
        Vector3 rotateDirection = Vector3.zero;


        switch (_mouvementX)
        {
            case 1:
                direction = transform.forward * m_speed;

                break;
            case 2:
                direction = transform.forward * -m_speed;
                break;
        }

        switch (_mouvementZ)
        {
            case 1:
                direction = transform.right * m_speed;
                break;
            case 2:
                direction = transform.right * -m_speed;
                break;
        }

        switch (_rotation)
        {
            case 1:
                rotateDirection = -transform.up;
                break;
            case 2:
                rotateDirection = -transform.up;
                break;
        }
        transform.Rotate(rotateDirection, Time.deltaTime * m_rotateSpeed);
        m_rigidbody.AddForce(direction,
            ForceMode.Impulse);
    
    }
    public void FixedUpdate()
    {
        m_rigidbody.velocity = Vector3.zero;
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * m_speed;
        Vector3 rotateDirection = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.A))
        {
            rotateDirection = -Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            rotateDirection = Vector3.up;
        }

        transform.Rotate(rotateDirection, Time.deltaTime * m_rotateSpeed);
        m_rigidbody.AddForce(direction,
            ForceMode.Impulse);
    }

}
