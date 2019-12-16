using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

	ParticleSystem particle;

	// Use this for initialization
	void Awake () {

		particle = GetComponent<ParticleSystem> ();
		particle.Stop ();
	
	}
	

	void OnTriggerEnter(Collider other){
	
	
		if (other.transform.tag == "Player" && enabled) {
		

			particle.Play ();
			particle.loop = true;
		
		}
	
	}


	void OnTriggerExit(Collider other){


		if (other.transform.tag == "Player" && enabled) {


			particle.Stop ();
			//particle.loop = true;

		}

	}
}
