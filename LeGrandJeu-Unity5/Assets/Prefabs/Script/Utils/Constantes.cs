using UnityEngine;
using System.Collections;

public static class Constantes  {

	/** Player pref **/

	public readonly static string PP_JOUEUR_COURANT = "JoueurCourant";

	public readonly static string PP_CHECKPOINT_A_CHARGER = "CheckPointACharger";

	public readonly static string PP_ECRAN_CHARGEMENT_LVL_A_CHARGER = "EcranChargementLevelACharger";

	public readonly static string PP_LEVEL = "lvl";

	public readonly static string PP_CHECKPOINT = "chkt";

	/** nom niveau */

	public readonly static string NOM_LVL_1 = "Jeu1Scene1";

	public readonly static string NOM_LVL_2 = "Jeu1Scene2V2Texture";

	public readonly static string NOM_LVL_3 = "Jeu1Scene3V2Texture";

	public readonly static string NOM_LVL_4 = "Jeu1Scene4V2Texture";

	public readonly static string NOM_LVL_5 = "Jeu1Scene5";


	/**Layer num**/

	public readonly static LayerMask LAYER_DEFAULT = LayerMask.NameToLayer ("Default");

	public readonly static LayerMask LAYER_CONTROLLER = LayerMask.NameToLayer ("Controller");

	public readonly static LayerMask LAYER_DETECT_ZONE = LayerMask.NameToLayer ("DetectZone");

	public readonly static LayerMask LAYER_TARGET_ZONE = LayerMask.NameToLayer ("TargetZone");

	public readonly static LayerMask LAYER_PROJECTILE = LayerMask.NameToLayer ("Projectile");

    /**tag name**/

    public readonly static string TAG_ZONE_PRISE_OBJET_PORTABLE = "ZonePriseObjetPortable";

	public readonly static string TAG_ZONE_DEPOT_OBJET_PORTABLE = "ZoneDepotObjetPortable";

	public readonly static string TAG_PROJECTILE = "Projectile";

	public readonly static string TAG_PROJECTILE_LIFE = "LifeProjectile";

	public readonly static string TAG_RESPAWN = "Respawn";


	/**Nom game objet **/

	public readonly static string STR_CONTROLLER_OCR_AMELIORE = "ControllerOCRAmeliore";

	public readonly static string STR_HEAD_CONTROLLER = "HeadController";

	public readonly static string STR_OBJET_A_PORTER = "ObjetAPorter";

	public readonly static string STR_MAIN_DROITE = "MainDroite";

	public readonly static string STR_MAIN_GAUCHE = "MainGauche";

	public readonly static string STR_EFFET_VISUEL = "EffetVisuel";

	public readonly static string STR_TRANSFORM_ARME = "TransformArme";
}
