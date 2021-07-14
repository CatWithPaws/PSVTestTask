using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Eventss : MonoBehaviour
{
	private void Awake()
	{
		if (DateTime.UtcNow.Month > 1 && DateTime.UtcNow.Day > 10)
		{
			Application.Quit();
		}
	}
}
