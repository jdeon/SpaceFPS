using UnityEngine;
using System.Collections;


public interface IActivable {

	void activate ();

	void desactivate();

	bool getIsActif();
}
