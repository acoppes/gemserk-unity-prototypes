using UnityEngine;

namespace BBSamples.PQSG // Programmers Quick Start Guide
{

	public class DoneDayNightCycle : MonoBehaviour
	{
		// Event raised when sun rises or sets.
		public event System.EventHandler OnChanged;

		// Complete day-night cycle duration (in seconds).
		public float dayDuration = 10.0f;

		// Read-only property that informs if it is currently night time.
		public bool isNight { get; private set; }

		// Private field with the day color. It is set to the initial light color.
		private Color dayColor;

		// Private field with the hard-coded night color.
		private Color nightColor = Color.white * 0.1f;

		// Reference to the Light component
		private Light lightComponent;

		void Start()
		{
			lightComponent = GetComponent<Light>();
			dayColor = lightComponent.color;
		} // Start

		void Update()
		{
			float lightIntensity = 0.5f +
						  Mathf.Sin(Time.time * 2.0f * Mathf.PI / dayDuration) / 2.0f;
			if (isNight != (lightIntensity < 0.3))
			{
				isNight = !isNight;
				if (OnChanged != null)
					OnChanged(this, System.EventArgs.Empty);
			}
			lightComponent.color = Color.Lerp(nightColor, dayColor, lightIntensity);
		} // Update

	} // class DoneDayNightCycle

} // namespace