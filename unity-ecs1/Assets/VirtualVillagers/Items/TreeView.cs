﻿using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers
{
	public class TreeView : MonoBehaviour
	{
		[UnityEngine.SerializeField]
		protected Transform _modelTransform;

		private LumberComponent _lumberComponent;
		private Components.TreeComponent _treeComponent;

		private int _size = -1;

		[SerializeField]
		protected bool _rotateOnLumber = false;
		
		[SerializeField]
		protected Vector3[] sizes = new Vector3[]
		{
			new Vector3(0.25f, 0.25f, 0.25f),
			new Vector3(0.65f, 0.65f, 0.65f),
			new Vector3(1.0f, 1.0f, 1.0f),
		};
		
		public void SetTreeData(GameObject treeData)
		{
			_lumberComponent = treeData.GetComponent<LumberComponent>();
			_treeComponent = treeData.GetComponent<Components.TreeComponent>();
		}

		private void LateUpdate()
		{
			if (_treeComponent == null || _lumberComponent == null)
				return;
			
			if (_size != _treeComponent.currentSize)
			{
				SetSize(_treeComponent.currentSize);
			}

			if (!_rotateOnLumber) 
				return;
			
			var maxLumber = (_treeComponent.currentSize + 1) * _treeComponent.lumberPerSize;
			var harvestPercentage = _lumberComponent.current / maxLumber;
	
			_modelTransform.localEulerAngles = new Vector3(0, 0, 
				Mathf.Lerp(0, 90, 1 - harvestPercentage));
		}

		private void SetSize(int size)
		{
			_size = size;

			if (size < 0)
				size = 0;
			
			if (size >= sizes.Length)
				size = sizes.Length - 1;
			
			_modelTransform.localScale = sizes[size];
		}
		
	}

}
