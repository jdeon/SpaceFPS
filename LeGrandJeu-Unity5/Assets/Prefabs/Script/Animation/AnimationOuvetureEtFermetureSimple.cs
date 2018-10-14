using UnityEngine;
using System.Collections;

public class AnimationOuvetureEtFermetureSimple : MonoBehaviour {


	public ConditionEventAbstract conditionOuverture;
	public ConditionEventAbstract conditionFermeture;

	Animator anim;
		
	void Start ()
	{
		anim = GetComponent<Animator>();
	}
		
	void Update ()
	{
		if (conditionOuverture.getIsActive()) {
			ouvrir ();
			conditionOuverture.desactiveEvent();
		} else if (conditionFermeture.getIsActive()) {
			fermer ();
			conditionFermeture.desactiveEvent();
		}
	}

	public void ouvrir (){
		anim.SetTrigger ("Ouvrir");
	}

	public void fermer (){
		anim.SetTrigger ("Fermer");
	}
}
