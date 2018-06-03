﻿using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // ConditionBase

namespace BBSamples.PQSG // Programmers Quick Start Guide
{

	[Condition("Samples/ProgQuickStartGuide/IsNight")]
	[Help("Checks whether it is night. It searches for the first light labeled with " +
		  "the 'MainLight' tag, and looks for its DayNightCycle script, returning the" +
		  "informed state. If no light is found, false is returned.")]
	public class DoneIsNightCondition : ConditionBase
	{

		public override bool Check()
		{
			if (searchLight())
				return light.isNight;
			else
				return false;
		}

		// Method invoked by the execution engine when the condition is used in a priority
		// selector and its last value was false. It must return COMPLETED when the value
		// becomes true. In other case, it can return RUNNING if the method should be
		// invoked again in the next game cycle, or SUSPEND if we will be notified of the
		// change through any other mechanism.
		public override TaskStatus MonitorCompleteWhenTrue()
		{
			if (Check())
				// Light is off. It's night right now.
				return TaskStatus.COMPLETED;
			else
			{
				// Light does not exist, or is "on". We must register ourselves in the
				// light event so we will be notified when the sun sets. In the mean time,
				// we do not need to be called anymore.
				if (light != null)
				{
					light.OnChanged += OnSunset;
				}
				return TaskStatus.SUSPENDED;
				// We will never awake if light does not exist.
			}
		} // MonitorCompleteWhenTrue

		// Similar to MonitorCompleteWhenTrue, but used when the last condition value was
		// true and the execution engine is checking that it has not become false.
		public override TaskStatus MonitorFailWhenFalse()
		{
			if (!Check())
				// Light does not exist, or is "on" (daylight). Condition is false.
				return TaskStatus.FAILED;
			else
			{
				// Light exists, and is "off" (night). We suspend ourselves
				// until sunrise (when the condition will become false).
				light.OnChanged += OnSunrise;
				return TaskStatus.SUSPENDED;
			}
		} // MonitorFailWhenFalse

		// Method attached to the light event that will be called when the light is "off"
		// again. We remove ourselves from the event, and notify the execution engine
		// that the new condition value is true (it is night again).
		public void OnSunset(object sender, System.EventArgs night)
		{
			light.OnChanged -= OnSunset;
			EndMonitorWithSuccess();
		} // OnSunset

		// Similar to OnSunset, but used when we are monitoring the sunrise.
		public void OnSunrise(object sender, System.EventArgs e)
		{
			light.OnChanged -= OnSunrise;
			EndMonitorWithFailure();
		} // OnSunrise

		public override void OnAbort()
		{
			if (searchLight())
			{
				light.OnChanged -= OnSunrise;
				light.OnChanged -= OnSunset;
			}
			base.OnAbort();
		} // OnAbort

		// Search the global light, and stores in the light field. It returns true
		// if the light was found.
		private bool searchLight()
		{
			if (light != null)
				return true;

			GameObject lightGO = GameObject.FindGameObjectWithTag("MainLight");
			if (lightGO == null)
				return false;
			light = lightGO.GetComponent<DoneDayNightCycle>();
			return light != null;
		} // searchLight

		private DoneDayNightCycle light;

	} // class DoneIsNightCondition

} // namespace