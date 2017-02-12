//
//  TestCases.cs
//
//  Author:
//       Thomas H. Jonell <@Net_Gnome>
//
//  Copyright (c) 2013 Thomas H. Jonell
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using BehaviorLibrary;
using BehaviorLibrary.Components;
using BehaviorLibrary.Components.Actions;
using UnityEngine;
using NUnit.Framework;

namespace BehaviorLibrary.Tests
{

	internal class SequenceTests
	{
		public SequenceTests (){}

		[Test]
		public void TestStatefulSeq(){
			//_log.enterScope("testStatefulSeq");
			
			bool first = true;
			
			var foo = new StatefulSequence (new BehaviorAction(delegate(){
				if(first){
					return BehaviorReturnCode.Success;
				}else{
					return BehaviorReturnCode.Failure;
				}
			}),new BehaviorAction( delegate(){
				if(first){
					first = false;
					return BehaviorReturnCode.Running;
				}else{
					return BehaviorReturnCode.Success;
				}
			}),new BehaviorAction(delegate(){
				return BehaviorReturnCode.Success;
			}));

			Assert.That ( foo.Behave ()== BehaviorReturnCode.Running);
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Success);
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Failure);

			first = true;

			Assert.That ( foo.Behave ()== BehaviorReturnCode.Running);
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Success);
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Failure);

		}

		[Test]
		public void TestStatefulSel(){
			
			bool first = true;
			bool second = true;
			var foo = new StatefulSelector (new BehaviorAction (delegate(){
				return BehaviorReturnCode.Failure;
			}), new BehaviorAction (delegate() {
				if(first){
					first = false;
					return BehaviorReturnCode.Running;
				}else{
					return BehaviorReturnCode.Failure;
				}
			}), new BehaviorAction (delegate(){
				if(first){
					return BehaviorReturnCode.Success;
				}else{
					if(second){
						second = false;
						return BehaviorReturnCode.Success;
					}else{
						return BehaviorReturnCode.Failure;
					}
				}
			}));
			
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Running);
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Success);
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Failure);
			
			first = true;
			second = true;
			
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Running);
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Success);
			Assert.That ( foo.Behave ()== BehaviorReturnCode.Failure);
			
		}
	}
}

