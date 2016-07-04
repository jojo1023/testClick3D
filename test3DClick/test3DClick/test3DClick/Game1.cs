using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace test3DClick
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        Camera camera;
        Panel testPanel;
        //Panel testPanel2;
        protected override void Initialize()
        {
            camera = new Camera(new Vector3(0, 00, 100), new Vector3(0, 0, 0), Vector3.Up, 5, 300f, MathHelper.ToRadians(45), GraphicsDevice.Viewport.AspectRatio);
            IsMouseVisible = true;
            base.Initialize();
        }
        VertexBuffer triangleBuffer;
        BasicEffect triangleEffect;
        VertexPositionColor[] points = new VertexPositionColor[6];
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            testPanel = new Panel(new VertexPositionColor(new Vector3(-10, -10, 0), Color.Gold), 10, 10, camera, GraphicsDevice);
            //testPanel2 = new Panel(new VertexPositionColor(new Vector3(3, 3, 0), Color.Black), 10, 10, camera, GraphicsDevice);
            //testPanel.Rotate(new Vector3(0, 0, 45));
            testPanel.Rotation *= Matrix.CreateRotationZ(MathHelper.ToRadians(0));
            points[0] = new VertexPositionColor(new Vector3(20, 20, 0), Color.White);
            points[1] = new VertexPositionColor(new Vector3(20, 10, 0), Color.White);
            points[2] = new VertexPositionColor(new Vector3(10, 20, 0), Color.White);
            points[3] = new VertexPositionColor(new Vector3(10, 10, 0), Color.White);
            points[4] = new VertexPositionColor(new Vector3(10, 20, 0), Color.White);
            points[5] = new VertexPositionColor(new Vector3(20, 10, 0), Color.White);
            triangleBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), points.Count(), BufferUsage.WriteOnly);
            triangleBuffer.SetData<VertexPositionColor>(points);

            triangleEffect = new BasicEffect(GraphicsDevice);

            triangleEffect.VertexColorEnabled = true;

            triangleEffect.View = camera.View;
            triangleEffect.Projection = camera.Projection;
            triangleEffect.World = Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateRotationX(MathHelper.ToRadians(0)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90));


            //testPanel.Rotation *= Matrix.CreateRotationY(MathHelper.ToRadians(45));
            mouseRay = new Ray(Vector3.Zero, new Vector3(0, 0, -1));
        }
        Ray mouseRay;
        protected override void UnloadContent()
        {
        }
        MouseState ms;
        MouseState lastMs;
        protected override void Update(GameTime gameTime)
        {
            ms = Mouse.GetState();

            Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(new Vector3(ms.X, ms.Y, 0), camera.Projection, camera.View, Matrix.Identity);
            Vector3 farPoint = GraphicsDevice.Viewport.Unproject(new Vector3(ms.X, ms.Y, 1), camera.Projection, camera.View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            //direction.Normalize();
            mouseRay.Direction = direction;
            mouseRay.Position = nearPoint;
            //BoundingBox testBox = new BoundingBox(new Vector3(-2.5f, -2.5f, -1), new Vector3(2.5f, 2.5f, 1));
            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (testPanel.Hitbox.Intersects(mouseRay) != null)//new BoundingBox(new Vector3(10, 10, 0), points[0].Position)
                {
                    float? test = mouseRay.Intersects(new Plane(points[0].Position, points[1].Position, points[2].Position));

                    if (testPanel.Color == Color.LightBlue)
                    {
                        testPanel.Color = Color.Gold;
                    }
                    else
                    {
                        testPanel.Color = Color.LightBlue;
                    }
                }
            }
            //if (mouseRay.Intersects(testPanel2.Hitbox) != null)
            //{
            //    float? test = mouseRay.Intersects(testPanel2.Hitbox);
            //    if (testPanel2.Color == Color.Black)
            //    {
            //        testPanel2.Color = Color.White;
            //    }
            //    else
            //    {
            //        testPanel2.Color = Color.Black;
            //    }
            //}
            testPanel.Update();
            //testPanel2.Update();

            lastMs = ms;
            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            //testPanel2.Draw();
            testPanel.Draw();
            GraphicsDevice.SetVertexBuffer(triangleBuffer);

            foreach (EffectPass pass in triangleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, triangleBuffer.VertexCount / 3);
            }
            base.Draw(gameTime);
        }

    }
}
