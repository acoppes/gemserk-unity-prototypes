using Pada1.BBCore.Framework;
using Pada1.BBCore;
using UnityEngine;

namespace BBCore.Conditions
{
    [Condition("Basic/CheckKey")]
    [Help("Checks whether a key is pressed")]
    public class CheckKey : ConditionBase
    {
        [InParam("key", DefaultValue = KeyCode.None)]
        [Help("Key expected to be pressed")]
        public KeyCode key = KeyCode.None;
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
                    return Input.GetKey(key);
            /*}
            return false;*/
		}
    }
}