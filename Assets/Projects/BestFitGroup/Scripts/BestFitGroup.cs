using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestFitGroup : MonoBehaviour
{
	[SerializeField] private Text[] textToResize;

	private void OnEnable()
	{
		StartCoroutine(Resize());
	} 

	private IEnumerator Resize()
	{
		yield return new WaitForSeconds(1);
		int minSize = int.MaxValue;
		for (int i = 0; i < textToResize.Length; i++)
		{
			var size = textToResize[i].cachedTextGenerator.fontSizeUsedForBestFit;
			if (size < minSize) minSize = size;
		}

		for (int i = 0; i < textToResize.Length; i++)
		{
			textToResize[i].resizeTextMaxSize = minSize;
		}
	}
}
