using System;

using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Constants;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces;
using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Models;

using Microsoft.Xna.Framework;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Concrete
{
    public class GameObjectPosition : IGameObjectPositionInterface
    {
        public void PlaceScrollsAndRockBarriers(Scroll[] scrolls, RockBarrier[] rockBarriers, Random random, CloudsGameObject[] clouds, FoliageGameObject[] foliages)
        {
            int min = GameConstants.MinDistance;
            int max = GameConstants.MaxDistance;

            Vector3 tempCenter;

            // place scrolls
            foreach (Scroll scroll in scrolls)
            {
                scroll.Position = GenerateRandomPosition(min, max, scrolls, rockBarriers, random);
                tempCenter = scroll.BoundingSphere.Center;
                tempCenter.X = scroll.Position.X;
                tempCenter.Z = scroll.Position.Z;
                scroll.BoundingSphere = new BoundingSphere(tempCenter, scroll.BoundingSphere.Radius);
                scroll.IsRetrieved = false;
            }

            // place rock barriers
            foreach (RockBarrier barrier in rockBarriers)
            {
                barrier.Position = GenerateRandomPosition(min, max, scrolls, rockBarriers, random);
                tempCenter = barrier.BoundingSphere.Center;
                tempCenter.X = barrier.Position.X;
                tempCenter.Z = barrier.Position.Z;
                barrier.BoundingSphere = new BoundingSphere(tempCenter, barrier.BoundingSphere.Radius);
            }

            // place clouds
            foreach (CloudsGameObject cloud in clouds)
            {
                cloud.Position = GenerateRandomPosition(min, max, random);
                cloud.Position.Y = 20;
            }

            // place foliage
            foreach (FoliageGameObject foliage in foliages)
            {
                foliage.Position = GenerateRandomPosition(min, max, random);
                // leave the default Y position -no need to change it.
                // foliage.Position.Y = 20;
            }
        }
        private Vector3 GenerateRandomPosition(int min, int max, Scroll[] scrolls, RockBarrier[] rockBarriers, Random random)
        {
            int xValue, zValue;
            do
            {
                xValue = random.Next(min, max);
                zValue = random.Next(min, max);
                if (random.Next(100) % 2 == 0)
                {
                    xValue *= -1;
                }
                if (random.Next(100) % 2 == 0)
                {
                    zValue *= -1;
                }
            }
            while (IsOccupied(xValue, zValue, scrolls, rockBarriers));
            return new Vector3(xValue, 0, zValue);
        }
        private Vector3 GenerateRandomPosition(int min, int max, Random random)
        {
            int xValue, zValue;
            xValue = random.Next(min, max);
            zValue = random.Next(min, max);
            if (random.Next(100) % 2 == 0)
            {
                xValue *= -1;
            }
            if (random.Next(100) % 2 == 0)
            {
                zValue *= -1;
            }
            return new Vector3(xValue, 0, zValue);
        }
        private bool IsOccupied(int xValue, int zValue, Scroll[] scrolls, RockBarrier[] rockBarriers)
        {
            foreach (GameObject gameObject in scrolls)
            {
                if (((int)(MathHelper.Distance(xValue, gameObject.Position.X)) < 15) && ((int)(MathHelper.Distance(zValue, gameObject.Position.Z)) < 15))
                {
                    return true;
                }
            }
            foreach (GameObject gameObject in rockBarriers)
            {
                if (((int)(MathHelper.Distance(xValue, gameObject.Position.X)) < 15) && ((int)(MathHelper.Distance(zValue, gameObject.Position.Z)) < 15))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
