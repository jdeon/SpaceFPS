using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CreateBoxSymetrique : MonoBehaviour {

    public bool symetrieX;
    public bool symetrieY;
    public bool symetrieZ;

    public bool symetricRotation;
    public bool local;

    public bool lancer;

    // Update is called once per frame
    void Update () {
		if (this.lancer && transform.childCount > 0)
        {
            float nbChildOriginaux = transform.childCount;

            for (int idxChild = 0; idxChild < nbChildOriginaux; idxChild++)
            {
                Transform child = transform.GetChild(idxChild);

                GameObject symetricChild = GameObject.CreatePrimitive(PrimitiveType.Cube);

                if (this.local)
                {
                    symetricChild.transform.SetParent(transform);
                    calculPositionSymetricIfLocal(child, symetricChild.transform);
                    calculRotationSymetricIfLocal(child, symetricChild.transform);
                } else
                {
                    calculPositionSymetricIfGlobal(child, symetricChild.transform);
                    calculRotationSymetricIfLocal(child, symetricChild.transform);
                    symetricChild.transform.SetParent(transform);
                }

                symetricChild.transform.localScale = child.localScale;
            }
            this.lancer = false;
        }
	}

    /**
     * calcul la position de la symetrie si l'option local est coche
     * */
    private void calculPositionSymetricIfLocal (Transform original, Transform symetrique)
    {
        float positionX, positionY, positionZ;

        if (this.symetrieX)
        {
            positionX = -original.localPosition.x;
        } else
        {
            positionX = original.localPosition.x;
        }

        if (this.symetrieY)
        {
            positionY = -original.localPosition.y;
        } else
        {
            positionY = original.localPosition.y;
        }

        if (this.symetrieZ)
        {
            positionZ = -original.localPosition.z;
        } else
        {
            positionZ = original.localPosition.z;
        }

        symetrique.localPosition = new Vector3(positionX, positionY, positionZ);
    }

    /**
     * calcul la rotation de la symetrie si l'option local est coche
     * */
    private void calculRotationSymetricIfLocal(Transform original, Transform symetrique)
    {
        float angleX, angleY, angleZ;

        if (this.symetrieX)
        {
            angleX = -original.localEulerAngles.x;
        }
        else
        {
            angleX = original.localEulerAngles.x;
        }

        if (this.symetrieY)
        {
            angleY = -original.localEulerAngles.y;
        }
        else
        {
            angleY = original.localEulerAngles.y;
        }

        if (this.symetrieZ)
        {
            angleZ = -original.localEulerAngles.z;
        }
        else
        {
            angleZ = original.localEulerAngles.z;
        }

        symetrique.localEulerAngles = new Vector3(angleX, angleY, angleZ);
    }

    /**
    * calcul la position de la symetrie si l'option local n est pas coche
    * */
    private void calculPositionSymetricIfGlobal(Transform original, Transform symetrique)
    {
        float positionX, positionY, positionZ;

        if (this.symetrieX)
        {
            positionX = 2*transform.position.x - original.position.x;
        }
        else
        {
            positionX = original.position.x;
        }

        if (this.symetrieY)
        {
            positionY = 2 * transform.position.x - original.position.x;
        }
        else
        {
            positionY = original.position.y;
        }

        if (this.symetrieZ)
        {
            positionZ = 2 * transform.position.x - original.position.x;
        }
        else
        {
            positionZ = original.position.z;
        }

        symetrique.position = new Vector3(positionX, positionY, positionZ);
    }

    /**
    * calcul la rotation de la symetrie si l'option local n est pas coche
    * */
    private void calculRotationSymetricIfGlobal(Transform original, Transform symetrique)
    {
        float angleX, angleY, angleZ;

        if (this.symetrieX)
        {
            angleX = -original.eulerAngles.x;
        }
        else
        {
            angleX = original.eulerAngles.x;
        }

        if (this.symetrieY)
        {
            angleY = -original.eulerAngles.y;
        }
        else
        {
            angleY = original.eulerAngles.y;
        }

        if (this.symetrieZ)
        {
            angleZ = -original.eulerAngles.z;
        }
        else
        {
            angleZ = original.eulerAngles.z;
        }

        symetrique.eulerAngles = new Vector3(angleX, angleY, angleZ);
    }
}