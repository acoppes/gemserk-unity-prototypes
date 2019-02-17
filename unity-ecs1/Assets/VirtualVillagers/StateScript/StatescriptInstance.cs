using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Scripting
{
    public class StatescriptGraph
    {

    }

    public class StatescriptState
    {
        // get data
        // enqueue event
        // set frame ticking enabled

        // on activate
        // on deactivate
        // on timer event
        // on frame tick

        // used by the editor
    }

    public struct StatescriptComponent : Unity.Entities.IComponentData
    {
        // command frame
        // statescript instances
        // owner vars
        // sync manager
    }
    
    public class StatescriptVarBags
    {
        // identifier 16-bit, var primiteve, var array
    }

    public struct StatescriptInstance
    {
        // instance id
        // graph pointer
        // states
        // future events
        // instance vars
    }
    
}