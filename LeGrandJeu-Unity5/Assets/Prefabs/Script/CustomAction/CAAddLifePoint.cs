using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAAddLifePoint : CustomActionScript {

	public enum TypePointAAjouter {vie, regenerationArmure, reparationArmure , nouvelleArmure};

	public int montantPointAjouter;

	public TypePointAAjouter typePointAAjouter;

	public GameObject targetWithISystemVieOptionel;

	private ISystemVie sytemeVieCible;

	public override IEnumerator DoActionOnEvent (MonoBehaviour sender, GameObject args)	{

		if (null != targetWithISystemVieOptionel) {
			sytemeVieCible = targetWithISystemVieOptionel.GetComponent<ISystemVie> ();
		}

		if (null == this.sytemeVieCible) {
			this.sytemeVieCible = args.GetComponent<ISystemVie> ();
		}

		if (null != this.sytemeVieCible) {
			switch (typePointAAjouter) {
			case TypePointAAjouter.vie:
				this.sytemeVieCible.soin (montantPointAjouter);
				break;

			case TypePointAAjouter.regenerationArmure:
				if (null != this.sytemeVieCible.getArmure ()) {
					this.sytemeVieCible.getArmure ().regeneration (montantPointAjouter);
				}
				break;

			case TypePointAAjouter.reparationArmure:
				if (null != this.sytemeVieCible.getArmure ()) {
					this.sytemeVieCible.getArmure ().reparation (montantPointAjouter);
				}
				break;

			case TypePointAAjouter.nouvelleArmure:
				this.sytemeVieCible.addArmure (montantPointAjouter);
				break;
			}
		}
		yield return null;
	}
}
