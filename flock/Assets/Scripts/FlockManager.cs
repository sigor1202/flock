using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    //pega o prefab do peixe
    public GameObject fishPrefab;
    //quantidade de peixes
    public int numFish = 20;
    //lista de peixes
    public GameObject[] allFish;
    //limita o espaço que os peixes irão parecer
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    //posição de destino
    public Vector3 goalPOs;


    [Header("Configurações do Cardume")]
    //velocidade minima
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    //velocidade maxima
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    //distancia do outro peixxe
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    //velocidade de rotação
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //atribui o tamanho da lista
        allFish = new GameObject[numFish];
        //configura cada um dos peixes para spawnar corretamente e atribuir o script flock
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
            Random.Range(-swinLimits.y, swinLimits.y),
            Random.Range(-swinLimits.z, swinLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        //posição de destino
        goalPOs = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //posição de destino
        goalPOs = this.transform.position;
        if(Random.Range(0,100)<10)
        goalPOs = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
            Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
    }
}
