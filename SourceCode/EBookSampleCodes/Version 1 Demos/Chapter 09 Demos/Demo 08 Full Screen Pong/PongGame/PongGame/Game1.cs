using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace PongGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Game World

        SpriteFont font;

        Texture2D ballTexture;
        Rectangle ballRectangle;
        float ballX;
        float ballY;
        float ballXSpeed = 3;
        float ballYSpeed = 3;

        Texture2D lPaddleTexture;
        Rectangle lPaddleRectangle;
        float lPaddleSpeed = 4;
        float lPaddleY;
        int lPlayerScore;

        Texture2D rPaddleTexture;
        Rectangle rPaddleRectangle;
        float rPaddleY;
        int rPlayerScore;

        // Distance of paddles from screen edge
        int margin;

        // Game score message
        string message = "0:0";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 240;
            Content.RootDirectory = "Content";
            Guide.IsScreenSaverEnabled = false;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("MessageFont");

            ballTexture = Content.Load<Texture2D>("ball");
            lPaddleTexture = Content.Load<Texture2D>("lpaddle");
            rPaddleTexture = Content.Load<Texture2D>("rpaddle");

            ballRectangle = new Rectangle(
                0,0,
                GraphicsDevice.Viewport.Width / 20,
                GraphicsDevice.Viewport.Width / 20);

            ballX = GraphicsDevice.Viewport.Width / 2;
            ballY = GraphicsDevice.Viewport.Height / 2;

            margin = GraphicsDevice.Viewport.Width / 20;

            lPaddleRectangle = new Rectangle(
              margin, 0,
              GraphicsDevice.Viewport.Width / 20,
              GraphicsDevice.Viewport.Height / 5);

            rPaddleRectangle = new Rectangle(
              GraphicsDevice.Viewport.Width - lPaddleRectangle.Width - margin, 0,
              GraphicsDevice.Viewport.Width / 20,
              GraphicsDevice.Viewport.Height / 5);

            lPaddleY = lPaddleRectangle.Y;
            rPaddleY = rPaddleRectangle.Y;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            ballX = ballX + ballXSpeed;
            ballY = ballY + ballYSpeed;

            if (ballX < 0)
            {
                rPlayerScore++;
                message = lPlayerScore.ToString() + ":" + rPlayerScore.ToString();
                ballXSpeed = -ballXSpeed;
            }
            
            if ( ballX + ballRectangle.Width > GraphicsDevice.Viewport.Width)
            {
                lPlayerScore++;
                message = lPlayerScore.ToString() + ":" + rPlayerScore.ToString();
                ballXSpeed = -ballXSpeed;
            }

            if (ballY < 0 || ballY + ballRectangle.Height > GraphicsDevice.Viewport.Height)
            {
                ballYSpeed = -ballYSpeed;
            }

            ballRectangle.X = (int)(ballX + 0.5f);
            ballRectangle.Y = (int)(ballY + 0.5f);

            // Use touch to control the left paddle

            TouchCollection touches = TouchPanel.GetState();

            if (touches.Count > 0)
            {
                if (touches[0].Position.Y > GraphicsDevice.Viewport.Height / 2)
                {
                    TouchLocation x = touches[0];
                    lPaddleY = lPaddleY + lPaddleSpeed;
                }
                else
                {
                    lPaddleY = lPaddleY - lPaddleSpeed;
                }
            }

            lPaddleRectangle.Y = (int)(lPaddleY + 0.5f);

            // Make the right paddle always follow the ball

            rPaddleRectangle.Y = ballRectangle.Y;

            // Ball - Bat collisons

            if (ballRectangle.Intersects(lPaddleRectangle))
            {
                ballXSpeed = -ballXSpeed;
            }

            if (ballRectangle.Intersects(rPaddleRectangle))
            {
                ballXSpeed = -ballXSpeed;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            float messageWidth = font.MeasureString(message).X;

            spriteBatch.DrawString(
                font,
                message,
                new Vector2((GraphicsDevice.Viewport.Width - messageWidth) / 2, margin),
                Color.White);
            
            spriteBatch.Draw(ballTexture, ballRectangle, Color.White);
            spriteBatch.Draw(lPaddleTexture, lPaddleRectangle, Color.White);
            spriteBatch.Draw(rPaddleTexture, rPaddleRectangle, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
