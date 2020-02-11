using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DiceRoller
{
	public class UIMenu : MonoBehaviour
	{
		[SerializeField] private Shaker shacker;

		[Header("Timescale")]
		[SerializeField] private float defaultTimescaleValue;
		[SerializeField] private Slider timescaleSlider;
		[Header("Size")]
		[SerializeField] private float defaultSizeValue;
		[SerializeField] private Slider sizeSlider;


		public void OnTimescaleSliderChange(float v)
		{
			Time.timeScale = v;
		}

		public void OnTimescaleSliderRightClick(BaseEventData e)
		{
			var pointerE = e as PointerEventData;
			if (pointerE.button == PointerEventData.InputButton.Right)
			{
				timescaleSlider.value = defaultTimescaleValue;
			}
		}
		public void OnShakeButton()
		{
			shacker.Shake();
		}

		public void OnSizeSliderChange(float v)
		{
			shacker.SetSize((byte)v);
		}

		public void OnSizeSliderRightClick(BaseEventData e)
		{
			var pointerE = e as PointerEventData;
			if (pointerE.button == PointerEventData.InputButton.Right)
			{
				sizeSlider.value = defaultSizeValue;
			}
		}
	}
}