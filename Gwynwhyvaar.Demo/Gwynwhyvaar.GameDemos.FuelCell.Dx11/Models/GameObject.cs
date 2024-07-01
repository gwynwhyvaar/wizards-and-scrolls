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
        public BoundingSphere CalculateBoundingSphere()
        {
            BoundingSphere mergeSphere =new BoundingSphere();
            BoundingSphere[] boundingSpheres;
            int index = 0;
            int meshCount =Model.Meshes.Count;

            boundingSpheres =new BoundingSphere[meshCount];
            foreach (ModelMesh mesh in Model.Meshes)
            {
                boundingSpheres[index++] =mesh.BoundingSphere;
            }

            mergeSphere = boundingSpheres[0];
            if(Model.Meshes.Count > 1)
            {
                index = 1;
                do
                {
                    mergeSphere = Microsoft.Xna.Framework.BoundingSphere.CreateMerged(mergeSphere, boundingSpheres[index]);
                    index++;
                }
                while(index< Model.Meshes.Count);
            }
            mergeSphere.Center.Y = 0;
            return mergeSphere;
        }
        public void DrawBoundingSphere(Matrix view, Matrix projection, GameObject boundingSphereModel)
        {
            Matrix scaleMatrix = Matrix.CreateScale(BoundingSphere.Radius);
            Matrix translateMatrix =Matrix.CreateTranslation(BoundingSphere.Center);
            Matrix worldMatrix =scaleMatrix * translateMatrix;

            foreach(ModelMesh mesh in boundingSphereModel.Model.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.World =worldMatrix;
                    effect.View =view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}
