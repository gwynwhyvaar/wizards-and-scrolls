using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Models
{
    public record class GameObject()
    {
        public Model Model { get; set; }
        public bool IsActive { get; set; } = false;
        public BoundingSphere BoundingSphere { get; set; } = new BoundingSphere();
        public Vector3 Position = Vector3.Zero;
    }
}
