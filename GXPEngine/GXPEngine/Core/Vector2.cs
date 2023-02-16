using System;

namespace GXPEngine.Core
{
	public struct Vector2
	{
		public float x;
		public float y;
		
		public Vector2 (float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		
		override public string ToString() {
			return "[Vector2 " + x + ", " + y + "]";
		}



        public Vector2 addVectors(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public Vector2 subVectors(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public Vector2 multiplyVector(Vector2 a, float factor)
        {
            return new Vector2(a.x * factor, a.y * factor);
        }

        public Vector2 divideVector(Vector2 a, int divider)
        {
            return new Vector2(a.x/divider, a.y/divider);
        }

        public Vector2 setMagnetude(Vector2 a, int newMagenetude)
        {
			int oldMagnetude = (int)Math.Sqrt(a.x * a.x + a.y * a.y);

            return new Vector2(a.x * newMagenetude/oldMagnetude, a.y * newMagenetude / oldMagnetude);
        }

		public int distance(Vector2 a, Vector2 b)
		{
			Vector2 vectorOffset = subVectors(a, b);
			return (int)Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
        }
    }
}

