using System;

namespace Gemserk.Vision
{
    public struct VisionMatrix
    {
        public int width;
        public int height;

        public int[] values;
        public short[] ground;
        private int[] visited;

        public int[] temporaryVisible;
		
        public void Init(int width, int height, int value, short groundLevel)
        {
            this.width = width;
            this.height = height;

            var length = width * height;
			
            values = new int[length];
            ground = new short[length];
            visited = new int[length];

            temporaryVisible = new int[length];
			
            Clear(value, groundLevel);
        }

        public bool IsInside(int i, int j)
        {
            return i >= 0 && i < width && j >= 0 && j < height;
        }

        public void SetVisible(int playerFlags, int i, int j)
        {
            visited[i + j * width] |= playerFlags;
            values[i + j * width] |= playerFlags;
        }

        public bool IsVisible(int playerFlags, int i, int j)
        {
            return (values[i + j * width] & playerFlags) > 0;
        }
		
        public bool IsVisible(int playerFlags, int i)
        {
            return (values[i] & playerFlags) > 0;
        }
		
        public bool WasVisible(int playerFlags, int i)
        {
            return (visited[i] & playerFlags) > 0;
        }

        public short GetGround(int i, int j)
        {
            return ground[i + j * width];
        }
		
        public short GetGround(int i)
        {
            return ground[i];
        }
		
        public void SetGround(int i, int j, short ground)
        {
            this.ground[i + j * width] = ground;
        }

        public void Clear(int value, short groundLevel)
        {
            for (var i = 0; i < width * height; i++)
            {
                visited[i] = 0;
                ground[i] = groundLevel;
                values[i] = value;
            }
        }
		
        public void ClearValues()
        {
            Array.Clear(values, 0, values.Length);
        }

        public void Clear()
        {
            Array.Clear(values, 0, values.Length);
            Array.Clear(visited, 0, visited.Length);
        }

    }
}