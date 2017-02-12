using System;
using BehaviorLibrary;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Actions;
using UnityEngine;
using NUnit.Framework;

namespace BehaviorLibrary.Tests
{

	internal class Issue2
	{
		public Issue2 ()
		{
		}
		
		[Test]
		public void test1(){
			
			var foo = new Sequence (new BehaviorAction (delegate() {
				return BehaviorReturnCode.Running;
			}), new BehaviorAction (delegate() {
				return BehaviorReturnCode.Running;
			}), new BehaviorAction (delegate() {
				return BehaviorReturnCode.Running;
			}), new BehaviorAction (delegate() {
				return BehaviorReturnCode.Running;
			}));
			
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Running, "all running is running");
			
			foo = new Sequence (new BehaviorAction (delegate() {
				return BehaviorReturnCode.Running;
			}), new BehaviorAction (delegate() {
				return BehaviorReturnCode.Running;
			}), new BehaviorAction (delegate() {
				return BehaviorReturnCode.Running;
			}), new BehaviorAction (delegate() {
				return BehaviorReturnCode.Success;
			}));
			
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Running, "all but one running is running");
			
			foo = new Sequence (new BehaviorAction (delegate() {
				return BehaviorReturnCode.Success;
			}), new BehaviorAction (delegate() {
				return BehaviorReturnCode.Success;
			}), new BehaviorAction (delegate() {
				return BehaviorReturnCode.Success;
			}), new BehaviorAction (delegate() {
				return BehaviorReturnCode.Success;
			}));
			
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Success, "all success is success");
			
		}
	}
}

