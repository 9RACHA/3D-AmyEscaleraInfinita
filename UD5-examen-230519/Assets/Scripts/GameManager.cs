using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // prefab de thing para instanciarlo
    public GameObject thingPrefab;
    // lista del shlefs donde se instanciaran/crean las things
    public List<GameObject> shelfList; //Se asigna en el inspector
    // cantidad de thing a instanciar
    private int cantidadThing = 20;

    // Start is called before the first frame update
    void Start()
    {
        // se llama al metodo para que instance/cree las things
        SpawnThings();    
    }

    private void SpawnThings()
    {
        // random donde se guardara el numero aleatorio del shelf seleccionado
        int randomShelf;
        // altura de las siguientes thing
        float thingNextAltura = 0.085f;
        // Vector3 con la posicion del thing al principio de todo
        Vector3 position = new Vector3(0f, 0.35f, 0);
        
        for (int i = 0; i < cantidadThing; i++) //Se repetira 20 veces ya que comieza en 0 y termina en 19
        {
            // se hace un reset a la posicion de Y en cada vuelta ya que es una thing diferente
            position.y = 0.35f;
            // se hace un random de los shelfs
            randomShelf = UnityEngine.Random.Range(0, shelfList.Count); //Aleatorio en la lista
            // se instancia la thing siendo hija del shlef random y se guarda para poder modificarla
            GameObject goThing = Instantiate(thingPrefab, shelfList[randomShelf].transform);//[randomShelf] el elemento exacto de la lista
            // modificamos la altura que tendra teniendo en cuenta cuantas things hay ya en el shelf
            position.y += (thingNextAltura * shelfList[randomShelf].GetComponent<Shelf>().GetThingListCount());
            // se le pone como posicion local la posicion calculada
            goThing.transform.localPosition = position;
            // se a�ade al shelf
            shelfList[randomShelf].GetComponent<Shelf>().Addthing(goThing);
        }
    }
}
