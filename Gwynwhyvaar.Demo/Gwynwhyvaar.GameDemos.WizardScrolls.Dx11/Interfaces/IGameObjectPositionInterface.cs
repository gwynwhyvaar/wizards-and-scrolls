using System;
using System.Collections.Generic;

using Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Models;

namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Interfaces
{
    public interface IGameObjectPositionInterface
    {
        public void PlaceScrollsAndRockBarriers(Scroll[] scrolls, RockBarrier[] rockBarriers, Random random, CloudsGameObject[] clouds, List<FoliageGameObject> foliages, PowerUpGameObject[] powerUps);
    }
}
