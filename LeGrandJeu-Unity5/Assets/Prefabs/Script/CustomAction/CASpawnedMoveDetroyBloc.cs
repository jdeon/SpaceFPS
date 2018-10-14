using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Fait aparaitre une liste de bloc en créant chaque bloc par acroissement et en les faisant disparaitre au point d arriver de la même manière.
 * */
public class CASpawnedMoveDetroyBloc : CustomActionScript {

	public enum ApparitionMode {UneDimension, DeuxDimension, TroisDimension}

	public Transform depart;
	public Transform arrive;
	public List<Vector4> listCaracBloc;
	public Material material;
	public float vitesse;
	public float densite; //mass par metre cube
	public ApparitionMode nbDimension; //1:z, 2: x,z  3 : x,y,z

	private Vector3 direction;
	private float distance;

	private bool rotating; 
	private Vector3 rotationAnglesParSec;


	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)	{
		this.direction = (arrive.position - depart.position).normalized;
		this.distance = Vector3.Distance(depart.position,arrive.position) ;
		if (depart.rotation == arrive.rotation) {
			this.rotating = false;
			this.rotationAnglesParSec = Vector3.zero;
		} else {
			this.rotating = true;
			this.rotationAnglesParSec = (arrive.rotation.eulerAngles - depart.rotation.eulerAngles) * this.vitesse / this.distance;
		}
		foreach(Vector4 vect in listCaracBloc){
			StartCoroutine(genererEtGestionBloc(new Vector3(vect.x, vect.y, vect.z), vect.w));
		}
		yield return null;
	}

	private IEnumerator genererEtGestionBloc (Vector3 tailleBloc, float delay){
		yield return new WaitForSeconds(delay);
		float dimentionMin = this.vitesse * Time.deltaTime ;
		float tempsAvantDestruction = (this.distance + Mathf.Abs(Vector3.Dot(tailleBloc,this.direction) /2)) / this.vitesse;

		Vector3 distancesFinDiminution = tailleBloc /2 ;
		GameObject newBloc = GameObject.CreatePrimitive(PrimitiveType.Cube);
		newBloc.transform.position = depart.position + dimentionMin * this.direction / 2;
		newBloc.transform.rotation = depart.rotation;
		newBloc.transform.SetParent(depart);

		//Rajout du rigidbody
		Rigidbody rigidBBloc = newBloc.AddComponent<Rigidbody>();
		rigidBBloc.useGravity = false ;
		rigidBBloc.mass = this.densite * tailleBloc.x * tailleBloc.y * tailleBloc.z ;
		rigidBBloc.constraints = RigidbodyConstraints.FreezeRotation;

		//Calcul taille
		Vector3 newScale = new Vector3(tailleBloc.x, tailleBloc.y, tailleBloc.z);
		switch (nbDimension){
		case ApparitionMode.TroisDimension:
			newScale.z = dimentionMin;
			newScale.x = dimentionMin;
			newScale.y = dimentionMin;
			break;
		case ApparitionMode.DeuxDimension:
			newScale.z = dimentionMin;
			newScale.x = dimentionMin;
			distancesFinDiminution.y = 0;
			break;
		case ApparitionMode.UneDimension: 
			newScale.z = dimentionMin;
			distancesFinDiminution.x = 0;
			distancesFinDiminution.y = 0;
			break;
		}
		newBloc.transform.localScale = newScale ;

		//Rajout du materiaux
		Renderer render = newBloc.GetComponent<Renderer>();
		if(null == render){
			render = newBloc.AddComponent<Renderer>();
		}
		render.material = this.material;


		yield return null ;

		while(tempsAvantDestruction > 0){
			Vector3 vitesseDesirer = vitesse * this.direction;
			newScale = newBloc.transform.localScale;
			Vector3 positionViaDepart = depart.InverseTransformPoint (newBloc.transform.position);
			Vector3 positionViaArrive = arrive.InverseTransformPoint (newBloc.transform.position);
			//Boucle sur toutes les directions
			for(int i = 0 ; i <3 ; i++){
				//Phase d aparition
				if(Mathf.Abs(positionViaDepart[i]) < distancesFinDiminution[i]){
					vitesseDesirer[i] /=2 ;
					newScale[i] = Mathf.Abs(positionViaDepart[i]) *2;
				} //Condition pour réduction
				else if(Mathf.Abs(positionViaArrive[i]) < distancesFinDiminution[i]){
					vitesseDesirer[i] /=2 ;
					newScale[i] = Mathf.Abs(positionViaArrive[i]) *2;
				} else {
					newScale[i] = tailleBloc[i];
				}
			}
				
			rigidBBloc.velocity = vitesseDesirer;
			newBloc.transform.localScale = newScale;
			if (rotating) {
				rigidBBloc.angularVelocity = rotationAnglesParSec;
			}
			tempsAvantDestruction -= Time.deltaTime;
			yield return null;
		}
		GameObject.Destroy(newBloc);
		yield return null;
	}
}