using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CADesarmeCible : CustomActionScript
{
    GameObject target;

	public override IEnumerator DoActionOnEvent(MonoBehaviour sender, GameObject args)
	{
		if(null != target)
        {
			Transform transformArme = target.transform.Find("TransformArme");
			if(null != transformArme)
            {
				int childCount = transformArme.childCount;
				for(int i = 0; i < childCount; i++)
                {
					GameObject.Destroy(transformArme.GetChild(i).gameObject);
				}
			}
        }
		
		yield return null;
	}
}
