using UnityEngine;
using System.Collections;

public class ToucheClavierCliquable : MonoBehaviour {

	private bool isClick = false;
	private bool isClickTraite = true;

	void OnMouseDown(){
		isClick = true;
		isClickTraite = false;
	}

	public bool getIsClick(){
		return this.isClick;
	}

	public void setIsClick(bool isClickSet){
		this.isClick = isClickSet;
	}

	public bool getIsClickTraite(){
		return this.isClickTraite;
	}

	public void setIsClickTraite(bool isClickTraiteSet){
		this.isClickTraite = isClickTraiteSet;
	}
}
