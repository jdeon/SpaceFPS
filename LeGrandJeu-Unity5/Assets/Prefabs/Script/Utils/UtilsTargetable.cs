using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsTargetable
{

	public static bool isTargetAtteignable(Vector3 coordOrigin, Vector3 coordTarget, int nbObstacleMax)
	{
		RaycastHit hit = new RaycastHit();
		bool isTrigger = true;
		while (isTrigger)
		{
			isTrigger = false;
			hit = new RaycastHit();
			if (Physics.Linecast(coordOrigin, coordTarget, out hit))
			{
				if (null == hit.collider || hit.collider.gameObject.layer == Constantes.LAYER_DEFAULT.value)
				{
					return false;
				}
				//On touche directement le controller
				else if (hit.collider.gameObject.layer == Constantes.LAYER_CONTROLLER.value)
				{
					return true;
				}
				//On touche un collider Trigger donc on reocommence a partir du point de colision
				else if (hit.collider.isTrigger && nbObstacleMax >= 0 &&
					(hit.collider.gameObject.layer == Constantes.LAYER_DETECT_ZONE.value || hit.collider.gameObject.layer == Constantes.LAYER_TARGET_ZONE.value))
				{
					isTrigger = true;
					Vector3 vectorToTarget = coordTarget - coordOrigin;
					vectorToTarget.Normalize();
					coordOrigin = hit.point + (.001f * vectorToTarget); //Plus permet d'éviter de toujours retombé sur le meme collider
					nbObstacleMax--;
				}
			}
		}
		return false;
	}
}
