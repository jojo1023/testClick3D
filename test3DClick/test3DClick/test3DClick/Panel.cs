using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace test3DClick
{
    public class Panel
    {
        VertexPositionColor[] points = new VertexPositionColor[6];
        VertexPositionTexture[] texturePoints = new VertexPositionTexture[6];
        VertexPositionColorTexture[] colorTexturePoints = new VertexPositionColorTexture[6];
        VertexBuffer triangleBuffer;
        BasicEffect triangleEffect;
        GraphicsDevice _graphicsDevice;
        Camera _camera;
        Matrix scale;
        Vector3 tempScale;
        Quaternion tempRotation;
        Vector3 tempTranslation;
        Texture2D _texture;
        public Vector3 RotationAmount
        {

            get
            {
                return new Vector3(tempRotation.X, tempRotation.Y, tempRotation.Z);
            }
        }
        public Matrix Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }
        Matrix rotation;
        public Matrix Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }
        Matrix translation;
        public Matrix Translation
        {
            get
            {
                return translation;
            }
            set
            {
                translation = value;
            }
        }
        public Matrix World
        {
            get
            {
                return triangleEffect.World;
            }
            set
            {
                triangleEffect.World = value;
            }
        }
        public VertexPositionColor[] ColorPoints
        {
            get
            {
                return points;
            }
        }
        public VertexPositionTexture[] TexturePoints
        {
            get
            {
                return texturePoints;
            }
        }
        public VertexPositionColorTexture[] ColorTexturePoints
        {
            get
            {
                return colorTexturePoints;
            }
        }
        public Color Color
        {
            get
            {
                return points[0].Color;
            }
            set
            {
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].Color = value;
                }
                SetupColor(points, _camera, translation, scale, rotation, _graficsDevice);
            }
        }
        protected GraphicsDevice _graficsDevice;
        public BoundingBox Hitbox
        {
            get
            {
                return new BoundingBox(new Vector3(World.Translation.X + _width / 2, World.Translation.Y - _height / 2, World.Translation.Z) * scale.Translation, new Vector3(World.Translation.X - _width / 2, World.Translation.Y + _height / 2, World.Translation.Z) * scale.Translation);
                //return new BoundingBox(translation.Translation * scale.Translation, (translation.Translation - new Vector3(_width, _height, 0)) * scale.Translation);
            }
        }
        protected float _width;
        protected float _height;
        
        public Panel(VertexPositionColor center, float widith, float height, Camera camera, GraphicsDevice graficsDevice)
        {
            position = Translation.Translation;
            _width = widith;
            _height = height;
            _graficsDevice = graficsDevice;
            //Vector3 temp = center.Position;
            //center.Position = Vector3.Zero;
            VertexPositionColor topRight = new VertexPositionColor(new Vector3(center.Position.X + widith / 2, center.Position.Y + height / 2, center.Position.Z), center.Color);
            VertexPositionColor bottomLeft = new VertexPositionColor(new Vector3(center.Position.X - widith / 2, center.Position.Y - height / 2, center.Position.Z), center.Color);
            points[0] = new VertexPositionColor(new Vector3(center.Position.X - widith / 2, center.Position.Y + height / 2, center.Position.Z), center.Color);
            points[1] = topRight;
            points[2] = bottomLeft;
            points[3] = bottomLeft;
            points[4] = topRight;
            points[5] = new VertexPositionColor(new Vector3(center.Position.X + widith / 2, center.Position.Y - height / 2, center.Position.Z), center.Color);

            SetupColor(points, camera, Matrix.CreateTranslation(center.Position), Matrix.CreateTranslation(Vector3.One), Matrix.Identity, graficsDevice);

            //Translation = Matrix.CreateTranslation(temp);

        }
        public Panel(Vector3 center, float widith, float height, Texture2D texture, Camera camera, GraphicsDevice graficsDevice)
        {
            _texture = texture;
            //Vector3 temp = center;
            //center = Vector3.Zero;
            VertexPositionTexture topRight = new VertexPositionTexture(new Vector3(center.X + widith / 2, center.Y + height / 2, center.Z), new Vector2(1, 1));
            VertexPositionTexture bottomLeft = new VertexPositionTexture(new Vector3(center.X - widith / 2, center.Y - height / 2, center.Z), new Vector2(0, 0));



            texturePoints[0] = new VertexPositionTexture(new Vector3(center.X - widith / 2, center.Y + height / 2, center.Z), new Vector2(0, -1));
            texturePoints[1] = topRight;
            texturePoints[2] = bottomLeft;
            texturePoints[3] = bottomLeft;
            texturePoints[4] = topRight;
            texturePoints[5] = new VertexPositionTexture(new Vector3(center.X + widith / 2, center.Y - height / 2, center.Z), new Vector2(1, 0));

            SetupTexture(texturePoints, texture, camera, Matrix.CreateTranslation(center), Matrix.CreateTranslation(Vector3.One), Matrix.Identity, graficsDevice);
            //Translation = Matrix.CreateTranslation(temp);
        }
        public Panel(Vector3 center, float widith, float height, Texture2D texture, Color tint, Camera camera, GraphicsDevice graficsDevice)
        {
            _texture = texture;
            //Vector3 temp = center;
            //center = Vector3.Zero;
            VertexPositionColorTexture topRight = new VertexPositionColorTexture(new Vector3(center.X + widith / 2, center.Y + height / 2, center.Z), tint, new Vector2(1, 1));
            VertexPositionColorTexture bottomLeft = new VertexPositionColorTexture(new Vector3(center.X - widith / 2, center.Y - height / 2, center.Z), tint, new Vector2(0, 0));

            colorTexturePoints[0] = new VertexPositionColorTexture(new Vector3(center.X - widith / 2, center.Y + height / 2, center.Z), tint, new Vector2(0, 1));
            colorTexturePoints[1] = topRight;
            colorTexturePoints[2] = bottomLeft;
            colorTexturePoints[3] = bottomLeft;
            colorTexturePoints[4] = topRight;
            colorTexturePoints[5] = new VertexPositionColorTexture(new Vector3(center.X + widith / 2, center.Y - height / 2, center.Z), tint, new Vector2(1, 0));
            SetupColorTexture(colorTexturePoints, texture, camera, Matrix.CreateTranslation(center), Matrix.CreateTranslation(Vector3.One), Matrix.Identity, graficsDevice);
            //Translation = Matrix.CreateTranslation(temp);
        }
        public Panel(VertexPositionColorTexture topRight, VertexPositionColorTexture topLeft, VertexPositionColorTexture bottomRight, VertexPositionColorTexture bottomLeft, Texture2D texture, Camera camera, GraphicsDevice graficsDevice)
        {
            _texture = texture;
            //Vector3 temp = center;
            //center = Vector3.Zero;


            colorTexturePoints[0] = topLeft;
            colorTexturePoints[1] = topRight;
            colorTexturePoints[2] = bottomLeft;
            colorTexturePoints[3] = bottomLeft;
            colorTexturePoints[4] = topRight;
            colorTexturePoints[5] = bottomRight;
            SetupColorTexture(colorTexturePoints, texture, camera, Matrix.CreateTranslation(new Vector3(Math.Abs(topRight.Position.X - bottomLeft.Position.X), Math.Abs(topRight.Position.Y - bottomLeft.Position.Y), Math.Abs(topRight.Position.Z - bottomLeft.Position.Z))), Matrix.CreateTranslation(Vector3.One), Matrix.Identity, graficsDevice);
            //Translation = Matrix.CreateTranslation(temp);
        }
        public void Update()
        {
            triangleEffect.View = _camera.View;
            rotation.Decompose(out tempScale, out tempRotation, out tempTranslation);

            //triangleEffect.World = Matrix.CreateWorld(translation.Translation, rotation.Forward, rotation.Up);
            //triangleEffect.World *= Matrix.CreateTranslation(translation.Translation);
            //triangleEffect.World *= Matrix.CreateScale(scale.Translation);
            //triangleEffect.World *= Matrix.CreateRotationX(rotation.Translation.X);
            //triangleEffect.World *= Matrix.CreateRotationY(rotation.Translation.Y);
            //triangleEffect.World *= Matrix.CreateRotationZ(rotation.Translation.Z);
            
            //translation = Matrix.CreateTranslation(Vector3.Zero);
            //triangleEffect.World = Matrix.CreateFromQuaternion(tempRotation) * Matrix.CreateTranslation(tempTranslation) * Matrix.CreateScale(tempScale);
            //translation = Matrix.CreateTranslation(position);
            triangleEffect.World = Matrix.Identity;
            //triangleEffect.World *= Matrix.CreateTranslation(Vector3.Zero);
            triangleEffect.World *= Matrix.CreateRotationZ(45) * translation;
        }
        Vector3 position;
        public void Rotate(Vector3 rotate)
        {
            Vector3 position = translation.Translation;
            translation = Matrix.CreateTranslation(Vector3.Zero);
            translation *= Matrix.CreateFromYawPitchRoll(rotate.X, rotate.Y, rotate.Z);
            translation.Translation = position;

        }
        public void Draw()
        {
            _graphicsDevice.SetVertexBuffer(triangleBuffer);
            foreach (EffectPass pass in triangleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, triangleBuffer.VertexCount / 3);
            }
            
        }
        private void SetupColor(VertexPositionColor[] points, Camera camera, Matrix translation, Matrix scale, Matrix rotation, GraphicsDevice graficsDevice)
        {
            triangleBuffer = new VertexBuffer(graficsDevice, typeof(VertexPositionColor), points.Count(), BufferUsage.WriteOnly);
            triangleBuffer.SetData<VertexPositionColor>(points);
            

            triangleEffect = new BasicEffect(graficsDevice);

            triangleEffect.View = camera.View;
            triangleEffect.Projection = camera.Projection;
            triangleEffect.World = Matrix.Identity;

            this.translation = translation;
            this.scale = scale;
            this.rotation = rotation;

            triangleEffect.VertexColorEnabled = true;
            _camera = camera;
            _graphicsDevice = graficsDevice;
        }

        private void SetupTexture(VertexPositionTexture[] texturePoints, Texture2D texture, Camera camera, Matrix translation, Matrix scale, Matrix rotation, GraphicsDevice graficsDevice)
        {
            triangleBuffer = new VertexBuffer(graficsDevice, typeof(VertexPositionTexture), points.Count(), BufferUsage.WriteOnly);
            triangleBuffer.SetData<VertexPositionTexture>(texturePoints);

            triangleEffect = new BasicEffect(graficsDevice);
            triangleEffect.View = camera.View;
            triangleEffect.Projection = camera.Projection;
            triangleEffect.Texture = texture;
            triangleEffect.TextureEnabled = true;
            triangleEffect.VertexColorEnabled = false;
            _camera = camera;
            _graphicsDevice = graficsDevice;


            triangleEffect.World = Matrix.Identity;

            this.translation = translation;
            this.scale = scale;
            this.rotation = rotation;
        }
        private void SetupColorTexture(VertexPositionColorTexture[] texturePoints, Texture2D texture, Camera camera, Matrix translation, Matrix scale, Matrix rotation, GraphicsDevice graficsDevice)
        {
            triangleBuffer = new VertexBuffer(graficsDevice, typeof(VertexPositionColorTexture), points.Count(), BufferUsage.WriteOnly);
            triangleBuffer.SetData<VertexPositionColorTexture>(texturePoints);

            triangleEffect = new BasicEffect(graficsDevice);
            triangleEffect.View = camera.View;
            triangleEffect.Projection = camera.Projection;
            triangleEffect.Texture = texture;
            triangleEffect.TextureEnabled = true;
            triangleEffect.VertexColorEnabled = true;
            _camera = camera;
            _graphicsDevice = graficsDevice;


            triangleEffect.World = Matrix.Identity;

            this.translation = translation;
            this.scale = scale;
            this.rotation = rotation;
        }
    }
}
