using System.Collections.Generic;
using UnityEngine;
using Gemserk.BehaviourTree;
using Unity.Entities;
using UnityScript.Lang;
using VirtualVillagers.Components;
using VirtualVillagers.Systems;
using System.Linq;

public interface GameWorld
{
    int GetTreeCount();

    List<LumberComponent> GetLumberMills();
}

public class BehaviourTreeTestSceneController : MonoBehaviour, GameWorld {

    public UnityEngine.Object _behaviourTreeManager;
	
	public int minTrees;
	public int maxTrees;

	public DebugTools debugTools;

    public List<LumberComponent> GetLumberMills()
    {
        return FindObjectsOfType<LumberComponent>().ToList();
    }

    public int GetTreeCount()
    {
        return 10;
    }

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
        UnityBehaviourTreeContext context = new UnityBehaviourTreeContext();

        context.SetManager<GameWorld>(this);

        btManager.SetContext(context);

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
        // La otra es casarse con algun sistema, sea ECS o GameObject y que los behaviours usesn eso. Entonces
        // el contexto ya podría tener algo como GetEntity() (o similar) que retorne la entidad sobre la cual está ejecutando.

        // Estaria bueno identificar los elementos estáticos, readonly (tipo assets) y mover a scriptable object, 
        // aunque terminen teniendo logica en si. Por ejemplo, los sub graph/ sub tree podrían ser un scriptable object hoy en día
        // y armar la jerarquía usando eso.

        // Quizás deberían ser módulos básicos, no código en sí. Es decir, tener elementos como condiciones, while, etc, pero no 
        // lógica elaborada, a menos que sea algo reusable que valga la pena tener como sub graph pero seguramente esté compuesto por los
        // otros elementos. 

        // Que pasa si necesito algo como un manager, por ejemplo pedirle algo al quadtree para buscar enemigos cercanos?
        // Podría tener una entidad unica con un componente para esto... podría también tenerlo incorporado en mi sistema de contexto
        // del bt, aunque ahí me caga que el sistema no lo está seteando hoy en día. Podría hacer algo para setear un extra de contexto
        // al sistema y/o al btmanager en vez del sistema y usarlo en mis acciones. En vez de castear a un contexto, castear a otro con mas
        // datos.

        // Me gustaría que los behaviours tuvieran acceso al contexto y pedirle cosas a ese, castearlo a algo, etc, pero no usar
        // el btmanager como hacen hoy en día. Podría ser un parámetro al llamar el nodo o algo que se setea. Tener en cuenta
        // que los tree se reusan entre entidades entonces seguramente es mejor tener algo como un parámetro que define la lógica, el 
        // tree ejecuta sobre ese contexto temporalmente, si se llama con otro parámetro, ejecuta distinto según las variables, etc.

        // Hay elementos del componente de "contexto" no generico que se podrían comenzar a pasar al genérico, aunque es un embole 
        // terminar usando GetBool("pipote"), etc. No se si se puede hacer de otra forma, quizás puedo dejar el grupo de fields todos juntos
        // y setear un struct o un puntero en el contexto pero no usar un componente para eso que tiene pinta que es logica de comportamientos
        // del btree.

        // Otra cosa interesante sería comenzar a hacer pruebas pero con animaciones, efectos y sonidos, para ver como todo esto semi
        // generico comienza a romperse un poco cuando se necesitan transiciones de cosas.
        // TODO: Probar justamente una transición de animación, como afecta al juego (me suena que pasa a ser un comportamiento o algo)

        // Me gustaría laburar en contexto del estilo Self/Target/etc, cosas que se pueden pedir a alguien para saber sobre quien actuar.
        // Por ejemplo, un comportamiento pide Self.position o GetPosition(source), luego hace GetTrees(position, distance), 
        // SetTarget(First(GetTrees(position, distance));

        // Otra cosa a testear, agregar/remover nodos/subgrafos del bt para agregar o remover cierto comportamiento temporal. En varios
        // lados lo he visto y en el Statescript dicen que lo usan para agregar cosas como stun, etc. Para esto parece necesario determinar
        // una especie de orden/prioridad entre los graphs para poder agregarlo antes que otros.
	}
}
