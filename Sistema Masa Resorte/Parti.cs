using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parti : MonoBehaviour
{
    private GameObject parti;
    private Vector3 X0; //Posición inicial
    private Vector3 V0; //Velocidad inicial
    private Vector3 X1; //Posición final
    private Vector3 V1; //Velocidad final
    private Vector3 G; //Gravedad
    private float m_parti; //Masa de la partícula
    private float [] fase; //Estado de fase
    private float [] Dfase; //Derivada del estado de fase
    private float [] k1; //Kutta 1
    private float [] k2; //Kutta 2
    private float [] k3; //Kutta 3
    private float [] k4; //Kutta 4
    private float h; //Paso
    private float t; //Tiempo

    //Variables para el resorte
    private Vector3 A; //Posición del resorte
    private Vector3 d;
    private float k; //Constante de proporcionalidad
    private float b; //Constante de amortiguamiento
    private float l; //Longitud del resorte
    private float dl; //Distancia de deformación del resorte

    // Start is called before the first frame update
    void Start()
    {
        //Se asignan los valores iniciales a las variables globales
        parti=GameObject.Find("Particula");
        X0=new Vector3(0.0f, 60.0f, 0.0f);
        V0=new Vector3(0.0f, -5.0f, 0.0f);
        X1=X0;
        V1=V0;
        G=new Vector3(0.0f, -9.8f, 0.0f);
        m_parti=1.0f;
        h=0.01f;
        fase=new float [6];
        Dfase=new float [6];
        k1=new float [6];
        k2=new float [6];
        k3=new float [6];
        k4=new float [6];
        t=0.0f;

        A=new Vector3(0.0f, 0.0f, 0.0f);
        d=new Vector3(0.0f, 0.0f, 0.0f);
        k=125.0f;
        b=0.5f;
        l=40.0f;

        /*Aqui asignamos a las tres primeras posiciones del estado de fase la posición inicial de la particula
        Despues asignamos a los tres ultimos valores del estado de fase la velocidad inicial de la particula
        Se hace lo mismo con la derivada del estado de fase, pero agregando la velocidad a las tres primeras posiciones
        y a las tres ultimas posiciones le agregamos 0*/
        for(int i=0;i<3;i++)
        {
            fase[i]=X0[i];
            fase[i+3]=V0[i];

            Dfase[i]=V0[i];
            Dfase[i+3]=0.0f;
        }
        //Se inicializa la particula a la posición inicial
        parti.transform.position=X0;
    }

    void calculo()
    {
        Vector3 Xf=new Vector3(fase[0], fase[1], fase[2]); //Vector posición
        Vector3 Vf=new Vector3(fase[3], fase[4], fase[5]); //Vector velocidad, usado para agregarle fricción al sistema
        Vector3 F=new Vector3(); //Vector fuerza

        for(int i=0;i<3;i++)
        {
            X1[i]=fase[i];
            V1[i]=fase[i+3];
        }

        d=X1-A;
        // d=d/(Math.Sqrt(Math.Pow((0 - 0), 2) + Math.Pow((60 - 0), 2) + Math.Pow((0 - 0), 2));
        // dl=(Math.Sqrt(Math.Pow((0 - 0), 2) + Math.Pow((60 - 0), 2) + Math.Pow((0 - 0), 2))-l;
        d.Normalize(); // XA / ||XA||
        dl=l-Vector3.Distance(Xf,A); // L-||XA||

        Vector3 Vd=new Vector3(0.0f, 0.0f, 0.0f);
        //Proyección de la velocidad de la particula sobre el resorte
        for(int i=0;i<3;i++)
        {
            Vd[i]=V1[i]*d[i];
        }

        F=(dl*k*d)-(b*Vd);
        Debug.Log(F);

        // SUMATORIA DE FUERZAS
        //Cuando la posición de la particula en el eje Y sea menor a 40 entonces e le aplicara fricción al sistema
        if(Xf.y < 40)
        {
            F+=(-b*Vf); //Se le aplica fricción al sistema
        }
        
        //A continuación se actualiza la derivada del estado de fase para poder resolver la ecuación diferencial
        for(int j=0;j<3;j++)
        {
            Dfase[j]=fase[j+3];
            Dfase[j+3]=F[j]/m_parti;
        }

        for(int i=0;i<6;i++)
        {
            k1[i]=Dfase[i];
            k2[i]=(h*0.5f*Dfase[i])+(k1[i]*0.5f);
            k3[i]=(h*0.5f*Dfase[i])+(k2[i]*0.5f);
            k4[i]=(h*Dfase[i])+k3[i];
        }
    }

    void resolv()
    {
       for(int i=0;i<6;i++)
        {
            fase[i]=fase[i]+((1/6f)*(k1[i]+2*k2[i]+2*k3[i]+k4[i])*h);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positionfinal=new Vector3(fase[0], fase[1], fase[2]);
        calculo();
        resolv();

        parti.transform.position=positionfinal;
        t+=h;
    }
}
