using Pada1.BBCore.Framework;
using Pada1.BBCore;
using UnityEngine;

namespace BBCore.Conditions
{
    [Condition("Basic/CheckButton")]
    [Help("Checks whether a button is pressed")]
    public class CheckButton : ConditionBase
    {
        [InParam("button", DefaultValue = "Jump")]
        [Help("Button expected to be pressed")]
        public string button;
        /*
        public enum MouseAction {down, up, during}
        [InParam("mouseAction", DefaultValue = MouseAction.during)]
        public MouseAction mouseAction = MouseAction.during;*/

		public override bool Check()
        {
            /*switch (mouseAction)
            {
                case MouseAction.down:
                    return Input.GetMouseButtonDown(button);

                case MouseAction.up:
                    return Input.GetMouseButtonUp(button);

                case MouseAction.during:*/
                    return Input.GetButton(button);
            /*}
            return false;*/
		}
    }
}