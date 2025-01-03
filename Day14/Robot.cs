namespace Day14
{
    internal class Robot(int posX, int posY, int velX, int velY)
    {
        private static int maxX;
        private static int maxY;

        public int PosX = posX;
        public int PosY = posY;
        private readonly int Vx = velX;
        private readonly int Vy = velY;

        static public void SetMax(int x, int y)
        {
            maxX = x;
            maxY = y;
        }

        public void Move()
        {
            PosX = (PosX + Vx + maxX) % maxX;
            PosY = (PosY + Vy + maxY) % maxY;
        }

        public void MoveN(int n)
        {
            PosX = (PosX + (Vx + maxX) * n) % maxX;
            PosY = (PosY + (Vy + maxY) * n) % maxY;
        }

        // return quadrant number (1-4) or 0 if on middle row or column
        public int GetQuadrant()
        {
            if (PosX < maxX / 2 && PosY < maxY / 2)
                return 1;
            if (PosX > maxX / 2 && PosY < maxY / 2)
                return 2;
            if (PosX < maxX / 2 && PosY > maxY / 2)
                return 3;
            if (PosX > maxX / 2 && PosY > maxY / 2)
                return 4;
            return 0;
        }
    }
}
