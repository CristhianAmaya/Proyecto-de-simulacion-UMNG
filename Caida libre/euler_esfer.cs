using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class euler_esfer : MonoBehaviour
{
    private GameObject esfera; //Variable que controla el objeto
    private Vector3 y0; //Posicion inicial
    private Vector3 yf; //Posicion final
    private Vector3 v0; //velocidad inicial
    private Vector3 vf; //velocidad final
    private Vector3 g; //gravedad
    private float t; //tiempo
    private float h; //Paso
    private float[] D;
    // Start is called before the first frame update
    void Start()
    {
        esfera = GameObject.Find("esfera_euler");
        v0 = new Vector3(0.0f, 0.0f, 0.0f);
        vf = v0;
        y0= new Vector3(30.0f, 50.0f, 0.0f);
        yf = y0;
        g = new Vector3(0.0f, -9.8f, 0.0f);
        D = new float[6];
        h = 0.1f;
        t = 0.0f;

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
            //D[i+3] = v1[i];
        }

        // vf = g * t + v0; //V = gt + v0
        // //vf = vf + h * (g * t + v0);
        // //yf = (1 / 2f) * (g * (t * t)) + (v0 * t) + y0; //X = (1/2)gt^2 + v0t + x0
        // yf = y0 + h * ((1 / 2f) * (g * (t * t)) + (v0 * t) + y0);
    }

    // Update is called once per frame
    void Update()
    {
        calculos();
        Vector3 posfinal = new Vector3(D[0], D[1], D[2]);
        esfera.transform.position = posfinal;
        t += h;
    }
}
