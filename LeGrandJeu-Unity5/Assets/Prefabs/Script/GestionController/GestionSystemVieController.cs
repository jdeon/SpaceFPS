using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GestionSystemVieController : MonoBehaviour , ISystemVie {

	public enum enumVieAZero {reloadLevel, lastCheckpoint};


	public enumVieAZero optionVieAZero;
    public int vieMax; 
	public Material[] listCouleurPV; // 3 couleurs, vert, orange et rouge
	public Transform transformVisuel;

    private Armure armure;
	private GestionCheckpoint gestionCheckpointController;
	private int vie;

    // Use this for initialization
    void Start()
    {
		this.vie = vieMax;
		this.gestionCheckpointController = gameObject.GetComponent<GestionCheckpoint>();
    }

    public void OnTriggerEnter(Collider other)
    {
		if(other.gameObject.tag == Constantes.TAG_PROJECTILE_LIFE)
        {
            blesse(1);
        } 
    }

	public void soin (int montantSoint){
		this.vie += montantSoint;
		if (this.vie > this.vieMax) {
			this.vie = this.vieMax;
		}
	}

    public void blesse(int nbDegat){

		int degatRestant = nbDegat;

		if (this.armure != null)
		{
			degatRestant = this.armure.absorberDegat(nbDegat);
		}

		this.vie -= degatRestant;
		if (this.vie <= 0) {
			tue ();
		}
    }

	public void tue (){
		if (this.optionVieAZero == enumVieAZero.lastCheckpoint && null != gestionCheckpointController) {
			//A revoir pour rénitailisé à ce niveau
			this.gestionCheckpointController.respawnCheckPoint();
		} else if (this.optionVieAZero == enumVieAZero.reloadLevel){
            //WaitForSeconds(5.0);  // or however long you want it to wait
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	public void addArmure(int pointDArmure){
		if (null == this.armure || this.armure.isBrisee () || this.armure.protectionMax <= pointDArmure) {
			this.armure = new Armure (pointDArmure, transformVisuel, listCouleurPV);
		} else {
			this.armure.regeneration (pointDArmure);
		}
	}

	public int getVie(){
		return this.vie;
	}

	public Armure getArmure (){
		return this.armure;
	}
}
