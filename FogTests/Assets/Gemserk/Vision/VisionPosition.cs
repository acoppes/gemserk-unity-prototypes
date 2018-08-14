using System;

namespace Gemserk.Vision
{
    public struct VisionPosition : IEquatable<VisionPosition>
    {
        public int x;
        public int y;

        public VisionPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public VisionPosition Move(int x, int y)
        {
            return new VisionPosition(this.x + x, this.y + y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VisionPosition && Equals((VisionPosition) obj);
        }

        public bool Equals(VisionPosition other)
        {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }
    }
}