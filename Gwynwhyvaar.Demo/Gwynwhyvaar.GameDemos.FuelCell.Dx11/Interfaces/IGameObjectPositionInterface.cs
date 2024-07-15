using System;

using Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Interfaces
{
    public interface IGameObjectPositionInterface
    {
        public void PlaceScrollsAndRockBarriers(Scroll[] scrolls, RockBarrier[] rockBarriers, Random random, CloudsGameObject[] clouds);
    }
}
