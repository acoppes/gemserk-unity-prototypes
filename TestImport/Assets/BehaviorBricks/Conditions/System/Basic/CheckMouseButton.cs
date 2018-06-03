using Pada1.BBCore.Framework;
using Pada1.BBCore;
using UnityEngine;

namespace BBCore.Conditions
{
    [Condition("Basic/CheckMouseButton")]
    [Help("Checks whether a mouse button is pressed")]
    public class CheckMouseButton : ConditionBase
    {

        public enum MouseButton {left = 0, right = 1, center = 2};
        
        [InParam("button", DefaultValue = MouseButton.left)]
        [Help("Mouse button expected to be pressed")]
        public MouseButton button = MouseButton.left;

        /*
        public enum MouseAction {down, up, during}
        [InParam("mouseAction", DefaultValue = MouseAction.during)]
        public MouseAction mouseAction = MouseAction.during;*/

        public override bool Check()
        {
            /*switch (mouseAction)
            {
                case MouseAction.down:
                    return Input.GetMouseButtonDown((int)button);

                case MouseAction.up:
                    return Input.GetMouseButtonUp((int)button);

                case MouseAction.during:*/
                    return Input.GetMouseButton((int)button);
            /*}
            return false;*/
		}
    }
}