using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simulation : MonoBehaviour
{
    private GameObject EsferaA; //Esfera con solución analítica
    private GameObject EsferaB; //Esfera con solución metódo de Euler

    private Vector3 V0; //Velocidad inicial
    private Vector3 Y0; //posición inicial

    private Vector3 Vf_A; //Velocidad final particula A (Roja)
    private Vector3 Yf_A; //Posición final particula A (Roja)

    private Vector3 Vf_E; //Velocidad final particula E (Azul)
    private Vector3 Yf_E; //Posición final particula E (Azul)

    private Vector3 nH; //Vector Normal
    private float dH;

    private Vector3 G; //gravedad
    private float t; //Tiempo
    private float h; //Paso
    private float m; //Masa
    private float f1; //Fricción 1
    private float f2; //Fricción 2
    // Se llama al inicio antes de la primera actualizaci�n del cuadro
    void Start()
    {
        EsferaA = GameObject.Find("Sphere01A");
        EsferaB = GameObject.Find("Sphere02E");

        V0 = new Vector3(20, 20, 0); //Se hace para que la esfera se mueva de manera parabolica
        Y0 = new Vector3(0, 10, 0);

        Vf_A = new Vector3(0, 0, 0);
        Yf_A = new Vector3(0, 0, 0);

        Vf_E = new Vector3(0, 0, 0);
        Yf_E = new Vector3(0, 0, 0);

        Vf_E = V0;
        Yf_E = Y0;

        G = new Vector3(0, -9.8f, 0);

        nH = new Vector3(0,1,0);
        dH = -5;

        t = 0.0f;
        m = 1.0f;
        h = 0.01f;
        f1 = 0.3f;
        f2 = 0.8f;

        EsferaB.transform.position = Y0;
    }

    //Mientras se va ejecutando el sistema, nos va dando el tiempo de ejecución que tiene el sistema 
    void Update()
    {
        t += Time.deltaTime;

        //Desarrollo con EULER
        Vector3 F = new Vector3(); //F=0

        F += m * G;
        float l_H = Vector3.Dot(Yf_E,nH) + dH; //Producto punto de: H(a) = a . n + dh = 0

        if (l_H > 0)
        {
            F += (-f1) * Vf_E; //Friccion
        }
        if (l_H < 0)
        {
            F += (-f2) * Vf_E; //Friccion
        }


        Yf_E = Yf_E + Vf_E * h; //Se coloca primero la posicion porque primero hay que trabajar con la velocidad actual y no con la nueva
        Vf_E = Vf_E + (F/m)*h;

        Debug.Log("Pos E" + Yf_E);
        Debug.Log("Pos A" + Yf_A);
        EsferaB.transform.position = Yf_E;

        t += h;
    }
}
