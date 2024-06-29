using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gwynwhyvaar.GameDemos.FuelCell.Dx11.Extensions
{
    public static class BasicEffectExtension
    {
        public static void SetSolidEffect(this BasicEffect effect)
        {
            effect.EnableDefaultLighting();
            effect.PreferPerPixelLighting = true;
            // solid light setting
            effect.AmbientLightColor = Vector3.One;
            effect.SpecularColor = Vector3.Zero;
            effect.EmissiveColor = Vector3.Zero;
            // this part allows drawing of the meshes in solid
            effect.DirectionalLight0.Enabled = false;
            effect.DirectionalLight1.Enabled = false;
            effect.DirectionalLight2.Enabled = false;
        }
    }
}
