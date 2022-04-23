using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lluvia : MonoBehaviour
{
    private GameObject esfera; //Variable que controla el objeto
    private GameObject clon;
    private Vector3 y0; //Posicion inicial
    private Vector3 yf; //Posicion final
    private Vector3 v0; //velocidad inicial
    private Vector3 vf; //velocidad final
    private Vector3 g; //gravedad
    private float t; //tiempo
    private float h; //Paso
    private float[] D;
    private float[] D2;
    private float band;

    //Variables para los duplicados
    private Vector3 y; //Posicion inicial
    private Vector3 yy; //Posicion final
    private Vector3 v; //velocidad inicial
    private Vector3 vv; //velocidad final
    private Vector3 U;
    // Start is called before the first frame update
    void Start()
    {
        esfera = GameObject.Find("esfera_euler");
        v0 = new Vector3(0.0f, 0.0f, 0.0f);
        vf = v0;
        y0= new Vector3(0.0f, 20.0f, 0.0f);
        yf = y0;
        g = new Vector3(0.0f, -9.8f, 0.0f);
        D = new float[3];
        D2 = new float[3];
        h = 0.1f;
        t = 0.0f;
        band = 0;

        
        

        esfera.transform.position = y0;
    }
    void calculos(){
        h = Time.deltaTime;

        Vector3 v1 = new Vector3();
        Vector3 y1 = new Vector3();
        
        //Primero se calcula el v1 y y1 que corresponden al metodo de euler
        v1 = vf + g * h;
        y1 = yf + v1 * h;
        

        //Ahora se actualiza los valores de vf y yf
        vf = v1;
        yf = y1;
        

        for(int i=0;i<3;i++){
            D[i] = y1[i]; //D[0] = y1.x, D[1] = y1.y, D[2] = y1.z
        }
        
    }
    void duplicate(){
        h = Time.deltaTime;
        U = new Vector3(Random.Range(-30.0f, 30.0f), 20.0f, Random.Range(-10.0f, 10.0f));
        
        if (band == 0)
        {
            v = new Vector3(0.0f, 0.0f, 0.0f);
            vv = v;
            y= new Vector3(U.x, U.y, U.z);
            yy = y;
        }
        yy.x = U.x;
        yy.y = U.y;
        yy.z = U.z;
        
        clon = Instantiate(esfera, U, Quaternion.identity);
        clon.name = "clone_esfer";

        clon.AddComponent<Rigidbody>();

        Vector3 v2 = new Vector3();
        Vector3 y2 = new Vector3();

        v2 = vv + g * h;
        y2 = yy + v2 * h;

        vv = v2;
        yy = y2;

        for(int i=0;i<3;i++){
            D2[i] = y2[i]; //D[0] = y1.x, D[1] = y1.y, D[2] = y1.z
        }

        band++;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.J)){
            calculos();
            duplicate();
            Vector3 posfinal = new Vector3(D[0], D[1], D[2]);
            Vector3 posfinish = new Vector3(D2[0], D2[1], D2[2]);
            esfera.transform.position = posfinal;        
            t += h;
        }
    }
}
