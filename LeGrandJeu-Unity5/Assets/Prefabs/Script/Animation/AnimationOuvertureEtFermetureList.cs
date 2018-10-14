using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOuvertureEtFermetureList : MonoBehaviour {

	public List<ConditionEventAbstract> listConditionOuverture;
	public List<ConditionEventAbstract> listConditionFermeture;

	Animator anim;

	void Start ()
	{
		anim = GetComponent<Animator>();
	}

	void Update ()
	{
		foreach(ConditionEventAbstract conditionOuverture in listConditionOuverture){
			if (conditionOuverture.getIsActive ()) {
				ouvrir (listConditionOuverture.IndexOf(conditionOuverture) + 1);
			}
		}
			
		foreach(ConditionEventAbstract conditionFermeture in listConditionFermeture){
			if (conditionFermeture.getIsActive ()) {
				fermer (listConditionFermeture.IndexOf(conditionFermeture) + 1);
			}
		}
	}

	public void ouvrir (int action){
		anim.SetInteger ("ActionAFaire", action);
	}

	public void fermer (int action){
		anim.SetInteger ("ActionAFaire", -1 * action);
	}
}
