using System.Collections.Generic;
using UnityEngine;
using Gemserk.BehaviourTree;
using Unity.Entities;
using UnityScript.Lang;
using VirtualVillagers;
using VirtualVillagers.Systems;

public class BehaviourTreeTestSceneController : MonoBehaviour {

    public UnityEngine.Object _behaviourTreeManager;
	
	public int minTrees;
	public int maxTrees;

	public DebugTools debugTools;
	
	private void CreateWorld()
	{
		var treesCount = UnityEngine.Random.Range(minTrees, maxTrees);
		
		for (var i = 0; i < treesCount; i++)
		{
			debugTools.SpawnTree();
		}
		
//		debugTools.SpawnLumbermill();
//		debugTools.SpawnEater();
	}

	private void Start() {
        var btManager = _behaviourTreeManager as BehaviourTreeManager;
        btManager.SetContext(new UnityBehaviourTreeContext());

        BehavioursFactory.CreateDefaultBehaviours(btManager);

		CreateWorld();
		
		World.Active.GetExistingManager<BehaviourTreeSystem>().SetBehaviourTreeManager(btManager);

		// World.Active.GetExistingManager<LumberCanvasSystem>().SetLumberBarPrefab(_lumberBarPrefab);
//		World.Active.GetExistingManager<ModelSystem>().SetModelPrefab(_modelPrefab);
		
		// TODO: barras de vida para saber cuanto lumber queda o que tan lleno esta un lumbermill
		
		// TODO: podría ser un sistema que si detecta un lumber holder que no tiene ui, crea ui y asocia al mismo, 
		// y si muere, mata el ui. Lo unico si es que va a tener que definir donde poner el ui, un offset o algo
		// el componente de lumber, lo cual puede ser raro. Capaz que si tiene componente de lumber ui, ahi si
		// define el offset, pero cualquier entidad con componente lumber ui y lumber, se autocrea y destruye el ui.
		
		// Siguiente prueba: agregar una condicion de cooldown y escribir en el contexto
		// Tener una accion separada para incrementar el cooldown? 
		// (o bien chequear por un dato general en el contexto, tipo el "frame" de ejecución)

		// se puede construir action custom? No -> construir propias

		// lugar de datos comun para usar en los behaviour (actions)
		// lugar propio (contexto local, independiente del tree, pertenece a la unidad)

		// tener un buen debug de esto es escencial

		// Siguiente paso, ir a cortar leña y juntarla en unapila.
		// Los arboles crecen con el tiempo de min a max, y cada tamañno tiene un determinado cantidad de leña.
		// Solo crecen si no fueron harvesteados nunca.

		// harvestear lleva tiempo, es decir, el árbol tiene cierta resistencia para bajar de un nivel a otro

		// arboles crecen en tiempo
		// cuando son grandes, ponen otros arboles al rededor (si no hay arboles ya)		

		// que pasa si quiero tener varios templates de arboles, o sea, el behaviour es el mismo pero 
		// quiero distintas configuraciones (variaciones de grow time, spawn child, etc)
		//		* podria tener distintos prefabs (wakale) y meter ahi un random de esos
		// 		* podría mover la config inicial a un scriptable object y usar un random de esos
		// 		* si fuera dato de entidad de ecs, podrían ser como templates/blueprints de esa entidad
		
		// si hay algun arbol harvesteado, le dan prioridad a ese a la hora de elegir

		// Feedback al fluent
		// no se puede hacer un árbol de una leaf sola? (un subarbol)
		// no se pueden agregar subarboles con al builder (yo agregue un Node(), podría llamarse subtree)

        // Se podría tener un concepto que vive dentro del sistema de behaviourstree/actions/statescript que encapsule
        // el concepto de entidad/gameobject. De manera que los behaviours solo saben de estos conceptos. Es como
        // si fuera el sistema del quadtree, para el qt solo le interesa ese sistema.

        // Estaria bueno identificar los elementos estáticos, readonly (tipo assets) y mover a scriptable object, 
        // aunque terminen teniendo logica en si. Por ejemplo, los sub graph/ sub tree podrían ser un scriptable object hoy en día
        // y armar la jerarquía usando eso.

        // Que pasa si necesito algo como un manager, por ejemplo pedirle algo al quadtree para buscar enemigos cercanos?
        // Podría tener una entidad unica con un componente para esto... podría también tenerlo incorporado en mi sistema de contexto
        // del bt, aunque ahí me caga que el sistema no lo está seteando hoy en día. Podría hacer algo para setear un extra de contexto
        // al sistema y/o al btmanager en vez del sistema y usarlo en mis acciones. En vez de castear a un contexto, castear a otro con mas
        // datos.
	}
}
