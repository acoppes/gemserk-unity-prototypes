using System;
using BehaviorLibrary.Components;
using UnityEngine;

namespace BehaviorLibrary
{
	public class StatefulSequence : BehaviorComponent
	{
		private BehaviorComponent[] _Behaviors;

		private int _LastBehavior = 0;

		/// <summary>
		/// attempts to run the behaviors all in one cycle (stateful on running)
		/// -Returns Success when all are successful
		/// -Returns Failure if one behavior fails or an error occurs
		/// -Does not Return Running
		/// </summary>
		/// <param name="behaviors"></param>
		public StatefulSequence (params BehaviorComponent[] behaviors){
			this._Behaviors = behaviors;
		}

		/// <summary>
		/// performs the given behavior
		/// </summary>
		/// <returns>the behaviors return code</returns>
		public override BehaviorReturnCode Behave(){

			//start from last remembered position
			for(; _LastBehavior < _Behaviors.Length;_LastBehavior++){
				try{
					switch (_Behaviors[_LastBehavior].Behave()){
					case BehaviorReturnCode.Failure:
						_LastBehavior = 0;
						ReturnCode = BehaviorReturnCode.Failure;
						return ReturnCode;
					case BehaviorReturnCode.Success:
						continue;
					case BehaviorReturnCode.Running:
						ReturnCode = BehaviorReturnCode.Running;
						return ReturnCode;
					default:
						_LastBehavior = 0;
						ReturnCode = BehaviorReturnCode.Success;
						return ReturnCode;
					}
				}
				catch (Exception e){

					if (Debug.isDebugBuild) {
						Debug.Log(e.ToString());
					}

					_LastBehavior = 0;
					ReturnCode = BehaviorReturnCode.Failure;
					return ReturnCode;
				}
			}

			_LastBehavior = 0;
			ReturnCode = BehaviorReturnCode.Success;
			return ReturnCode;
		}


	}
}

