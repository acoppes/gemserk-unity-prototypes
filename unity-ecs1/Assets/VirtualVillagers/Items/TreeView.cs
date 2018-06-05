﻿using UnityEngine;
using VirtualVillagers.Components;

namespace VirtualVillagers
{
	public class TreeView : MonoBehaviour
	{
		[UnityEngine.SerializeField]
		protected Transform _modelTransform;

		private BehaviourTreeContextComponent _treeData;
		private LumberHolder _lumberHolder;
		private Components.Tree _tree;

		private int _size = -1;
		
		public void SetTreeData(BehaviourTreeContextComponent treeData)
		{
			_treeData = treeData;
			_lumberHolder = treeData.GetComponent<LumberHolder>();
			_tree = treeData.GetComponent<Components.Tree>();
		}

		private void LateUpdate()
		{
			if (_treeData == null)
				return;
			
			if (_size != _tree.currentSize)
			{
				SetSize(_tree.currentSize);
			}
			
			var maxLumber = (_tree.currentSize + 1) * _tree.lumberPerSize;
			var harvestPercentage = _lumberHolder.current / maxLumber;

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
