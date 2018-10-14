using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISystemVie {

	void blesse(int degatSubit);
	void soin (int soinRecu);
	void addArmure(int pointProtectionAemure);
	int getVie();
	Armure getArmure ();

}
