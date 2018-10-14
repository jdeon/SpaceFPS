using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CreateBoxColliderForCylinder : MonoBehaviour {

    public enum Direction { x, y, z };

    public float rayonCylindre;
    public float longueurCylindre;
    public int nombreDeDivision;

    /**useLocalSize est true le programme n est pas encore teste*/
    public bool useLocalSize;
    public Direction axeCylindre;

    public bool lancer;
    
    private float nbDegree;
    private Vector3[] listPointTest;
    private GameObject colliderOfObject;

// Update is called once per frame
void Update () {
        if (lancer && nombreDeDivision > 2 && 360 % nombreDeDivision == 0)
        {
            colliderOfObject = new GameObject("Colliders_of_" + gameObject.name);
            colliderOfObject.transform.position = transform.position;
            colliderOfObject.transform.rotation = transform.rotation;

            Vector3[] listPointDepart = new Vector3[nombreDeDivision];
            Vector3[] listPointParentScale = new Vector3[nombreDeDivision];
            this.listPointTest = new Vector3[nombreDeDivision];
            this.nbDegree = 360 / nombreDeDivision;

            float axeAbscisse = -rayonCylindre * Mathf.Sin(Mathf.PI / nombreDeDivision);
            float axeOrdonee = -rayonCylindre * Mathf.Cos(Mathf.PI / nombreDeDivision);

            listPointDepart[0] = calculatePremierPoint(axeAbscisse, axeOrdonee);

            for (int numPoint = 1; numPoint < this.nombreDeDivision; numPoint++) { 
                listPointDepart[numPoint] = calculatePointSuivant(listPointDepart[0], numPoint);
            }

            listPointParentScale = listeDepartToParentScale(listPointDepart);

            for (int numPoint = 0; numPoint < this.nombreDeDivision - 1; numPoint++)
            {
                creationCollider(listPointParentScale[numPoint], listPointParentScale[numPoint + 1], numPoint);
            }

            creationCollider(listPointParentScale[nombreDeDivision - 1], listPointParentScale[0], nombreDeDivision - 1);


           /*for(int iTest = 0; iTest < this.listPointTest.Length; iTest++)
            {
                GameObject test = new GameObject("test_" + iTest);
                test.transform.position = transform.position + this.listPointTest[iTest];
                test.AddComponent<BoxCollider>().size = new Vector3(.05f,.05f,.05f);
            }*/

            this.lancer = false;
        }

        this.nbDegree = 0;
    }

    private Vector3 calculatePremierPoint(float axeAbscisse, float axeOrdonee)
    {
        Vector3 premierPoint;

        switch (this.axeCylindre)
        {
            case Direction.x:
                premierPoint = new Vector3(0, axeOrdonee, axeAbscisse);
                break;

            case Direction.y:
                premierPoint = new Vector3(axeAbscisse, 0, axeOrdonee);
                break;

            case Direction.z:
                premierPoint = new Vector3(axeAbscisse, axeOrdonee, 0);
                break;

            default:
                premierPoint = new Vector3(0, 0, 0);
                break;
        }

        return premierPoint;
    }

    private Vector3 calculatePointSuivant(Vector3 premierPoint, int numPoint)
    {
        Vector3 pointSuivant;

        //En x et en y on tourne dans le sens inverse car le premier point a un repère opposee il faut donc un moins
        switch (this.axeCylindre)
        {
            case Direction.x:
                pointSuivant = Quaternion.Euler(-numPoint * this.nbDegree, 0, 0) * premierPoint;
                break;

            case Direction.y:
                pointSuivant = Quaternion.Euler(0, -numPoint * this.nbDegree, 0) * premierPoint;
                break;

            case Direction.z:
                pointSuivant = Quaternion.Euler(0, 0, numPoint * this.nbDegree) * premierPoint;
                break;

            default:
                pointSuivant = premierPoint;
                break;
        }

        return pointSuivant;
    }

    private Vector3[] listeDepartToParentScale(Vector3[] listPointDepart) {
        Vector3[] listParentScale = new Vector3[listPointDepart.Length];
        Transform[] listTransform = new Transform[listPointDepart.Length];

        GameObject goParent = new GameObject();

        for(int numPoint = 0; numPoint < listPointDepart.Length; numPoint++)
        {
            GameObject goEnfant = new GameObject();
            goEnfant.transform.SetParent(goParent.transform);
            goEnfant.transform.position = listPointDepart[numPoint];
            listTransform[numPoint] = goEnfant.transform;
        }

        goParent.transform.rotation = transform.rotation;

        if (this.useLocalSize)
        {
            goParent.transform.SetParent(transform);
            goParent.transform.localScale = new Vector3(1, 1, 1);
        } else
        {
            goParent.transform.localScale = new Vector3(1, 1, 1);
            goParent.transform.SetParent(transform);
        }

       for (int numPoint = 0; numPoint < listTransform.Length; numPoint++)
        {
            listParentScale[numPoint] = listTransform[numPoint].position;
            this.listPointTest[numPoint] = listTransform[numPoint].position;
        }

        DestroyImmediate(goParent);

        return listParentScale;
    }


    private void creationCollider (Vector3 premierOrigine, Vector3 pointSuivant, int numPoint)
    {
        Vector3 cotee = pointSuivant - premierOrigine;
        GameObject goCollider = new GameObject("collider_" + numPoint);
        goCollider.transform.rotation = transform.rotation;
        goCollider.transform.SetParent(colliderOfObject.transform);

        float sizeCote;
        float sizeEpaisseur;
        float sizeLongeur;

        if (useLocalSize)
        {
            goCollider.transform.localPosition = cotee / 2 + premierOrigine;
            sizeCote = cotee.magnitude;
            sizeEpaisseur = .01f;
            sizeLongeur = this.longueurCylindre;
        } else
        {
            goCollider.transform.position = cotee /2 + premierOrigine + transform.position;
            sizeCote = cotee.magnitude;
            sizeEpaisseur = .01f;
            sizeLongeur = this.longueurCylindre;
        }

        BoxCollider col = goCollider.AddComponent<BoxCollider>();

        // En x et en y on tourne dans le sens inverse car le premier point a un repère opposee il faut donc un moins
        switch (this.axeCylindre)
        {
            case Direction.x:
                goCollider.transform.localRotation = Quaternion.Euler(-numPoint * this.nbDegree, 0, 0);             
                col.size = new Vector3(sizeLongeur, sizeEpaisseur, sizeCote);
                break;

            case Direction.y:
                goCollider.transform.Rotate(new Vector3(0, -numPoint * this.nbDegree, 0));
                col.size = new Vector3(sizeCote, sizeLongeur, sizeEpaisseur);
                break;

            case Direction.z:
                goCollider.transform.localRotation = Quaternion.Euler(0, 0, numPoint * this.nbDegree);
                col.size = new Vector3(sizeCote, sizeEpaisseur, sizeLongeur);
                break;
        }
    }

    //Cree un vecteur oriente cotee en x, epaisseur en y et longueur en z
    private Vector3 orienteParentLocalScale()
    {
        Vector3 localScaleOriente;

        //En x et en y on tourne dans le sens inverse car le premier point a un repère opposee il faut donc un moins
        switch (this.axeCylindre)
        {
            case Direction.x:
                localScaleOriente = new Vector3(1/transform.localScale.z, 1 / transform.localScale.y, 1 / transform.localScale.x);
                break;

            case Direction.y:
                localScaleOriente = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.z, 1 / transform.localScale.y);
                break;

            case Direction.z:
                localScaleOriente = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);
                break;

            default:
                localScaleOriente = new Vector3();
                break;
        }

        return localScaleOriente;
    }
}
