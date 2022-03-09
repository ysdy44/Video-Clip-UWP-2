using FanKit.Transformers;
using System.Numerics;

namespace Video_Clip2.Transforms
{
    public sealed class Transform : ICacheTransform
    {

        TransformerRect TransformerRect;
        public Transformer Transformer { get; set; }
        public Transformer StartingTransformer { get; private set; }

        private Transform()
        {
        }
        public Transform(uint width, uint height)
        {
            this.TransformerRect = new TransformerRect(width, height, Vector2.Zero);
            this.Transformer = new Transformer(width, height, Vector2.Zero);
            this.ReloadMatrix();
        }

        public Transform Clone()
        {
            return new Transform
            {
                TransformerRect = this.TransformerRect,
                Transformer = this.Transformer,
                StartingTransformer = this.StartingTransformer
            };
        }

        public Matrix3x2 Matrix { get; private set; }
        public void ReloadMatrix()
        {
            this.Matrix =
                Transformer.FindHomography(this.TransformerRect, this.Transformer);
        }


        public void CacheTransform()
        {
            this.StartingTransformer = this.Transformer;
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Transformer = this.StartingTransformer + vector;
            this.ReloadMatrix();
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Transformer = this.StartingTransformer * matrix;
            this.ReloadMatrix();
        }


    }
}