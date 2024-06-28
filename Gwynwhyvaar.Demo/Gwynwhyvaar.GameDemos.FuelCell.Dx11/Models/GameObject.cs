using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models
{
    public record class GameObject(Model Model, bool IsActive, BoundingSphere BoundingSphere)
    {
        Vector3 Position = Vector3.Zero;
    }
}
