using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private MeshRenderer m_Renderer;
    [SerializeField] Color Mycolors;
    
    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>(); //Random Color when meshrenderer is !Null
        ModifyItem();
    } 

    

    public void ModifyItem() { //Function Call Modify Item
        if (m_Renderer != null) {
            Mycolors = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));
            m_Renderer.material.color = Mycolors;
        }

        float xRotation = Random.Range(0f, 360f); //random rotation vectors
        float yRotation = Random.Range(0f, 360f);
        float zRotation = Random.Range(0f, 360f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation); // transform rotation randomly
    }
}
