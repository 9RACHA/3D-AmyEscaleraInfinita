using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour {

    /*Cada Thing al ser espaneada elegira aleatoriamente entre los materiales disponibles cual adjudicarse.
    Este comportamiento es la parte que falta por completar del prefab Thing.*/
    
    // Lista de materiales
    public List<Material> listaMateriales;

    private void Start()
    {
        // Se selecciona un material aleatorio cuando se crea el objeto, si esta en un objeto hijo
        GetComponentInChildren<Renderer>().material = listaMateriales[Random.Range(0, listaMateriales.Count)];
        Debug.Log("Materiales aleatorios");
    }

    /*Si todo estuviese en el mismo GameObject:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour
{
    // Lista de materiales
    public List<Material> listaMateriales;

    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();

        // Se selecciona un material aleatorio cuando se crea el objeto
        renderer.material = listaMateriales[Random.Range(0, listaMateriales.Count)];

        Debug.Log("Materiales aleatorios");
    }
}
*/
}
