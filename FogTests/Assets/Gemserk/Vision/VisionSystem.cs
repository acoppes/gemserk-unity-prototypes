using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Gemserk.Vision
{
    public class VisionSystem : MonoBehaviour {

        public int width = 128;
        public int height = 128;

        public int totalPlayers = 2;
	
        // flag...
        public int _activePlayers = 1;

        public bool raycastEnabled = true;

        [SerializeField]
        protected VisionTexture _visionTexture;
	
        [SerializeField]
        protected VisionCamera _visionCamera;
	
        [SerializeField]
        protected VisionTerrainTexture _visionTerrain;

        private VisionMatrix _visionMatrix;


        private Vector2 _localScale;

        [SerializeField]
        protected bool _alwaysUpdate;

        [SerializeField]
        protected bool _cacheVisible = true;
	
        private int _layerVisible;
        private int _layerHidden;

        private static CachedIntAbsoluteValues cachedAbsoluteValues;

        [SerializeField]
        public bool updateMethod;

        [SerializeField]
        public bool _recalculatePreviousVisible = true;
	
        public void Init()
        {
            _localScale = _visionCamera.GetScale(width, height);
	   
            _visionTexture.Create(width, height, _localScale);

            if (_visionTerrain != null)
            {
                _visionTerrain.Create(width, height, _localScale);
            }
	    
            _visionMatrix = new VisionMatrix();
            _visionMatrix.Init(width, height, 0, 0);

            _layerVisible = LayerMask.NameToLayer("Default");
            _layerHidden = LayerMask.NameToLayer("Hidden");

            // register static ground configuration...


            cachedAbsoluteValues.Init(Math.Max(width, height));
        }

        private VisionPosition GetMatrixPosition(Vector2 p)
        {
            var w = (float) width;
            var h = (float) height;

            var i = Mathf.RoundToInt(p.x / _localScale.x + w * 0.5f);
            var j = Mathf.RoundToInt(p.y / _localScale.y + h * 0.5f);

            return new VisionPosition
            {
                x = Math.Max(0, Math.Min(i, width - 1)),
                y = Math.Max(0, Math.Min(j, height - 1))
            };
        }

        private Vector2 GetWorldPosition(int i, int j)
        {
            var w = (float) width;
            var h = (float) height;

            var x = (i - w * 0.5f) * _localScale.x;
            var y = (j - h * 0.5f) * _localScale.y;

            return new Vector2(x, y);
        }

        private bool IsBlocked(VisionMatrix visionMatrix, short groundLevel, int x0, int y0, int x1, int y1)
        {
            Profiler.BeginSample("IsBlocked");

            int dx = cachedAbsoluteValues.Abs(x1 - x0);
            int dy = cachedAbsoluteValues.Abs(y1 - y0);
		
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;

            int err = (dx > dy ? dx : -dy) / 2;
            int e2;

            var blocked = false;

            var width = visionMatrix.width;
		
            for (;;)
            {
                // Tests if current pixel is already visible by current vision,
                // that means the line to the center is clear.
                if (_cacheVisible && visionMatrix.temporaryVisible[x0 + y0 * width] == 2)
                {
                    break;
                }

                var ground = visionMatrix.ground[x0 + y0 * width];

                if (ground > groundLevel)
                {
                    blocked = true;
                    break;
                }
			
                if (x0 == x1 && y0 == y1)
                    break;

                e2 = err;
			
                if (e2 > -dx)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 >= dy) 
                    continue;
			
                err += dx;
                y0 += sy;
            }

            Profiler.EndSample();
            return blocked;
        }
	
        private void DrawPixel(VisionMatrix visionMatrix, int player, int x0, int y0, int x, int y, short groundLevel)
        {
            if (!visionMatrix.IsInside(x, y))
                return;

            var blocked = false;
		
            // 0 means not visited yet
            // 1 means blocked
            // 2 means not blocked and visited

            if (raycastEnabled)
            {
                // Avoid recalculating this pixel blocked if was already visible by another vision of the same player 
                if (!_recalculatePreviousVisible && visionMatrix.IsVisible(player, x, y))
                    return;
			
                if (_cacheVisible)
                    visionMatrix.temporaryVisible[x + y * visionMatrix.width] = 1;
			
                blocked = IsBlocked(visionMatrix, groundLevel, x, y, x0, y0);
            }
		
            if (blocked)
            {
                return;
            } 
		
            if (raycastEnabled && _cacheVisible)
                visionMatrix.temporaryVisible[x + y * visionMatrix.width] = 2;
		
            visionMatrix.SetVisible(player, x, y);
        }

        private void UpdateVision(VisionPosition mp, float visionRange, int player, short groundLevel)
        {
            // clear local cache
            if (raycastEnabled && _cacheVisible)
                Array.Clear(_visionMatrix.temporaryVisible, 0, _visionMatrix.temporaryVisible.Length);
		
            if (!updateMethod)
            {
                UpdateVision1(mp, visionRange, player, groundLevel);
            }
            else
            {
                UpdateVision2(mp, visionRange, player, groundLevel);
            }
        }

        private void UpdateVision2(VisionPosition mp, float visionRange, int player, short groundLevel)
        {
            int radius = Mathf.CeilToInt(visionRange / _localScale.x);
            int x0 = mp.x;
            int y0 = mp.y;
		
            int x = radius;
            int y = 0;
            int xChange = 1 - (radius << 1);
            int yChange = 0;
            int radiusError = 0;
		
            while (x >= y)
            {
                for (var i = x0 - x; i <= x0 + x; i++)
                {
                    DrawPixel(_visionMatrix, player, x0, y0, i, y0 + y, groundLevel);
                    DrawPixel(_visionMatrix, player, x0, y0, i, y0 - y, groundLevel);
                }
                for (var i = x0 - y; i <= x0 + y; i++)
                {
                    DrawPixel(_visionMatrix, player, x0, y0, i, y0 + x, groundLevel);
                    DrawPixel(_visionMatrix, player, x0, y0, i, y0 - x, groundLevel);
                }

                y++;
                radiusError += yChange;
                yChange += 2;
			
                if (((radiusError << 1) + xChange) > 0)
                {
                    x--;
                    radiusError += xChange;
                    xChange += 2;
                }
            }
		
        }

        private void UpdateVision1(VisionPosition mp, float visionRange, int player, short groundLevel)
        {
            var visionPosition = GetWorldPosition(mp.x, mp.y);
		
            var currentRowSize = 0;
            var currentColSize = 0;
		
            var rangeSqr = visionRange * visionRange;
		
            var visionWidth = Mathf.RoundToInt(visionRange / _localScale.x);
            var visionHeight = Mathf.RoundToInt(visionRange / _localScale.y);

            var maxColSize = visionWidth;
            var maxRowSize = visionHeight;

            while (currentRowSize != maxRowSize && currentColSize != maxColSize)
            {
                var x = -currentColSize;
                var y = -currentRowSize;
			
                var dx = 1;
                var dy = 0;

                while (true)
                {
                    // check current
                    var mx = mp.x + x;
                    var my = mp.y + y;
				
                    var p = GetWorldPosition(mx, my);
				
                    var diff = p - visionPosition;
				
                    if (mx >= 0 && mx < width && my >= 0 && my < height)
                    {
                        if (diff.sqrMagnitude < rangeSqr)
                        {
                            var blocked = raycastEnabled && IsBlocked(_visionMatrix, groundLevel, mx, my, mp.x, mp.y);
						
                            if (!blocked)
                            {
                                _visionMatrix.SetVisible(player, mx, my);
                            }
                        }
                    }

                    if (x + dx > currentColSize)
                    {
                        dx = 0;
                        dy = 1;
                    }

                    if (y + dy > currentRowSize)
                    {
                        dx = -1;
                        dy = 0;
                    }

                    if (x + dx < -currentColSize)
                    {
                        dx = 0;
                        dy = -1;
                    }

                    if (y + dy < -currentRowSize)
                    {
                        // completed the cycle
                        break;
                    }
				
                    x += dx;
                    y += dy;
                }
			
                if (currentRowSize < maxRowSize)
                    currentRowSize++;
			
                if (currentColSize < maxColSize)
                    currentColSize++;
            }
        }

        public void Clear()
        {
            _visionMatrix.Clear();
        }

        public void ClearVision()
        {
            _visionMatrix.ClearValues();
        }
        
        public void UpdateVision(Vision vision)
        {
            vision.position = GetMatrixPosition(vision.worldPosition);
            UpdateVision(vision.position, vision.range, vision.player, vision.groundLevel);
        }

        public void UpdateTextures()
        {
            if (_visionTerrain != null)
            {
                _visionTerrain.UpdateTexture(_visionMatrix);
            } 
            _visionTexture.UpdateTexture(_visionMatrix, _activePlayers);
        }

        public void UpdateVisible(Visible visible)
        {
            var halfwidth = Mathf.CeilToInt(visible.bounds.x * 0.5f / _localScale.x);
            var halfheight = Mathf.CeilToInt(visible.bounds.y * 0.5f / _localScale.y);
			
            visible.matrixPosition = GetMatrixPosition(visible.worldPosition);

            var isVisible = false;

            var colStart = Math.Max(visible.matrixPosition.x - halfwidth, 0);
            var colEnd = Math.Min(visible.matrixPosition.x + halfwidth, width - 1);

            var rowStart = Math.Max(visible.matrixPosition.y - halfheight, 0);
            var rowEnd = Math.Min(visible.matrixPosition.y + halfheight, height - 1);

            for (var j = colStart; !isVisible && j <= colEnd; j++)
            {
                for (var k = rowStart; !isVisible && k <= rowEnd; k++)
                {
                    isVisible = _visionMatrix.IsVisible(_activePlayers, j, k);
                }
            }
			
            visible.visible = isVisible;
            visible.gameObject.SetLayerRecursive(isVisible ? _layerVisible : _layerHidden);
        }

        public void RegisterObstacle(VisionObstacle obstacle)
        {
            for (var i = 0; i < width; i++)
            {
                for (var k = 0; k < height; k++)
                {
                    var p = GetWorldPosition(i, k);
                    var currentLevel = _visionMatrix.GetGround(i, k);
					
                    if (currentLevel == 0)
                        currentLevel = obstacle.GetGroundLevel(p);

                    _visionMatrix.SetGround(i, k, currentLevel);
                }
            }		
        }
	
        public short GetGroundLevel(Vector3 position)
        {
            var mp = GetMatrixPosition(position);
            return _visionMatrix.GetGround(mp.x, mp.y);
        }
    }
}