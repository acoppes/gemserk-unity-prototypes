﻿using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers
{
	public class TreeView : MonoBehaviour
	{
		[UnityEngine.SerializeField]
		protected Transform _modelTransform;

		private BehaviourTreeContextComponent _treeData;
		private LumberComponent _lumberComponent;
		private Components.TreeComponent _treeComponent;

		private int _size = -1;
		
		public void SetTreeData(BehaviourTreeContextComponent treeData)
		{
			_treeData = treeData;
			_lumberComponent = treeData.GetComponent<LumberComponent>();
			_treeComponent = treeData.GetComponent<Components.TreeComponent>();
		}

		private void LateUpdate()
		{
			if (_treeData == null)
				return;
			
			if (_size != _treeComponent.currentSize)
			{
				SetSize(_treeComponent.currentSize);
			}
			
			var maxLumber = (_treeComponent.currentSize + 1) * _treeComponent.lumberPerSize;
			var harvestPercentage = _lumberComponent.current / maxLumber;

			_modelTransform.localEulerAngles = new Vector3(0, 0, 
				Mathf.Lerp(0, 90, 1 - harvestPercentage));
		}

		private void SetSize(int size)
		{
			_size = size;
			
			Vector3[] sizes =
			{
				new Vector3(0.25f, 0.25f, 0.25f),
				new Vector3(0.65f, 0.65f, 0.65f),
				new Vector3(1.0f, 1.0f, 1.0f),
			};

			if (size < 0)
				size = 0;
			if (size >= sizes.Length)
				size = sizes.Length - 1;
			
			_modelTransform.localScale = sizes[size];
		}
		
	}

}
