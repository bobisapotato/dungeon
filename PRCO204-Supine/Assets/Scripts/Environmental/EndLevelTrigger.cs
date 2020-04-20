using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
	public GameManager gameMan;

	private void Start()
	{
		gameMan = FindObjectOfType<GameManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("colliding");
		gameMan.openDemoMenu();
	}
	

}
