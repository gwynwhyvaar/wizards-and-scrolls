using System;
using System.Numerics;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Constants;
using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Extensions
{
    public static class ScrollsExtensions
    {
        public static void PlaceScrolls(this Scroll[] scrolls, Random random)
        {
            int min = GameConstants.MinDistance;
            int max = GameConstants.MaxDistance;
            // place the scrolls
            foreach(Scroll scroll in scrolls)
            {
                scroll.GenerateRandomPosition(min, max, random);
            }
        }
        public static void GenerateRandomPosition(this Scroll scroll, int min, int max, Random random)
        {
            int xValue, zValue;
            do
            {
                xValue = random.Next(min, max);
                zValue = random.Next(min, max);
                if(random.Next(100)%2 == 0)
                {
                    xValue *= -1;
                }
                if (random.Next(100) % 2 == 0)
                {
                    zValue *= -1;
                }
            }
            while(true);
            scroll.Position = new Vector3(xValue, 0, zValue);
        }
    }
}
