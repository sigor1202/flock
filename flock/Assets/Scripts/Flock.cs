using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //variavel para fazer referencia ao script myManager
    public FlockManager myManager;
    //velocidade
    float speed;
    //boleado usado na verificação da delimitação
    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        //randomiza a velocidade entre o vavlor minimo e maximo
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //limita a area em que os peixes irão se mover
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        RaycastHit hit = new RaycastHit();
        Vector3 direction = myManager.transform.position - transform.position;

        //se sair da area turning sera verdadeiro
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        else if(Physics.Raycast(transform.position,this.transform.forward*50,out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        //se não falso
        else
            turning = false;

        //se chegar no limite faz a rotação
        if(turning)
        {
            
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                myManager.rotationSpeed * Time.deltaTime);
        }
        
        else
        {
            //se for menor que 10 randomiza a velocidade entre a maxima e a minima
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            //se for menor que 20 chama o metodo applyRules
            if(Random.Range(0,100)<20)
                ApplyRules();
        }
       //se move
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        //cria uma lista
        GameObject[] gos;
        //coloca os peixes na lista
        gos = myManager.allFish;
        //centro da distancia dos peixes
        Vector3 vcentre = Vector3.zero;
        //evita a colis~~ao
        Vector3 vavoid = Vector3.zero;
        //velocidade do grupo de peixes
        float gSpeed = 0.01f;
        //distancia geral
        float nDistance;
        //quantidade de peixes
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            //se for diferente
            if (go != this.gameObject)
            {
                //calcula a distancia entre os peixes
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //compara o ndistance com a distancia escolhida
                if(nDistance<= myManager.neighbourDistance)
                {
                    //calcula o vcentre
                    vcentre += go.transform.position;
                    //adiciona mais 1
                    groupSize++;

                    //se a distancia for menor que 1 se afastam
                    if(nDistance<1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    //evita a colição entre eles
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }

        }
        //se o grupo for maior que zero
        if(groupSize>0)
        {
            //calcula o centro
            vcentre = vcentre / groupSize + (myManager.goalPOs - this.transform.position);
            // faz a velocidade de todos serem iguais
            speed = gSpeed / groupSize;
            //calcula a direção
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                //rotaciona de forma suave
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationSpeed * Time.deltaTime);
        }
    }
}
