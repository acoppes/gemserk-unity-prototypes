using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEditorMode {

//Directiva necesaria para evitar hacer una comprobacion "Aplication.IsEditor" en el BBManager
//Que poducía errores en la versión  2017
#if UNITY_EDITOR
	public static bool IsEditor = true;
#else
	public static bool IsEditor = false;
#endif



}
