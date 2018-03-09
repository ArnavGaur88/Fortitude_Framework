using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FortitudeFramework
{
    class FortitudeFrameworkClass
    {
    }

    //Game information
    public struct GameInformation
    {
        private static int gridWidth, gridHeight;
        private static int screenWidth, screenHeight;
        private static SpriteFont font;

        //-------------------------------------------SETTERS----------------------------------------------------------

        public static void SetDimensions(int width, int height)
        {
            screenWidth = width;
            screenHeight = height;
        }

        public static void SetGridDimensions(int width, int height)
        {
            gridWidth = width;
            gridHeight = height;
        }

        public static void SetFont(SpriteFont f)
        {
            font = f;
        }

        //-------------------------------------------SETTERS----------------------------------------------------------

        //-------------------------------------------GETTERS----------------------------------------------------------

        public static SpriteFont getFont()
        {
            return font;
        }

        public static int getGridWidth()
        {
            return gridWidth;
        }

        public static int getGridHeight()
        {
            return gridHeight;
        }

        public static int getScreenWidth()
        {
            return screenWidth;
        }

        public static int getScreenHeight()
        {
            return screenHeight;
        }

        //-------------------------------------------GETTERS----------------------------------------------------------
    }

    namespace Physics
    {
        public struct collisionRectangle
        {
            private Vector2 position;
            public Vector2 Top, Bottom, Left, Right;
            public int height, width;

            //Which side the collision is taking place...
            public bool left, right, up, down;

            public collisionRectangle(Vector2 pos, int wdt, int hgt)
            {
                position = pos;
                height = hgt;
                width = wdt;

                Top = new Vector2(position.X, position.Y);
                Bottom = new Vector2(position.X, position.Y + height);
                Left = new Vector2(position.X, position.Y);
                Right = new Vector2(position.X + width, position.Y);

                //No collision is taking place
                left = false;
                right = false;
                up = false;
                down = false;
            }

            public Vector2 getPosition()
            {
                return position;
            }

            //----------------------------------------------COLLISION-CHECKING---------------------------------------

            public bool checkCollision(collisionCircle circe)
            {
                Vector2 distanceVector = Vector2.Zero;

                //Check position of circle
                //Check if to the left...
                if (circe.getCenter().X < position.X)
                {
                    if (circe.getCenter().Y >= position.Y && circe.getCenter().Y < (position.Y + height))
                    {
                        //Circle is to the left, and within the correct height co-ordinates
                        distanceVector = Left - circe.getCenter();
                        if (distanceVector.Length() < circe.getRadius())
                            return true;
                        else
                            return false;
                    }
                }

                //Check if to the right...
                else if (circe.getCenter().X > (position.X + width))
                {
                    if (circe.getCenter().Y >= position.Y && circe.getCenter().Y < (position.Y + height))
                    {
                        //Circle is to the right, and within the bounds of the right side
                        distanceVector = Right - circe.getCenter();
                        if (distanceVector.Length() < circe.getRadius())
                            return true;
                        else
                            return false;
                    }
                }

                //Check if above...
                else if (circe.getCenter().Y < position.Y)
                {
                    if (circe.getCenter().X >= position.X && circe.getCenter().X < (position.X + width))
                    {
                        distanceVector = Top - circe.getCenter();
                        if (distanceVector.Length() < circe.getRadius())
                            return true;
                        else
                            return false;
                    }
                }

                //check if below...
                else if (circe.getCenter().Y > position.Y)
                {
                    distanceVector = Bottom - circe.getCenter();
                    if (distanceVector.Length() < circe.getRadius())
                        return true;
                    else
                        return false;
                }

                return false;
            }

            public bool checkCollision(collisionRectangle rect)
            {
                Vector2 rectPos = rect.position;

                //if to the far left...
                if (rectPos.X > position.X)
                {
                    if (position.X + width < rectPos.X)
                    {
                        right = false;
                        return false;
                    }
                    else
                        right = true;
                }

                else
                {
                    if (rectPos.X + rect.width < position.X)
                    {
                        left = false;
                        return false;
                    }
                    else
                        left = true;
                }

                if (rectPos.Y > position.Y)
                {
                    if (position.Y + height < rectPos.Y)
                    {
                        down = false;
                        return false;
                    }
                    else
                        down = true;
                }

                else
                {
                    if (rectPos.Y + rect.height < position.Y)
                    {
                        up = false;
                        return false;
                    }
                    else
                        up = true;
                }

                return true;
                
            }
            //----------------------------------------------COLLISION-CHECKING---------------------------------------

            //----------------------------------------------UPDATE-POSITION------------------------------------------
            public void update(Vector2 originPosition)
            {
                position = originPosition;

                //Make all collision sides false, they become true when checkCollision is run again.
                //But, we assume they're false when the update happens...
                left = false;
                right = false;
                up = false;
                down = false;
            }
            //----------------------------------------------UPDATE-POSITION------------------------------------------
        }

        public struct collisionCircle
        {
            //NOTE: Circle has no position, it moves along it's center, and hence only needs a center

            private Vector2 center;
            private float radius;

            //Access Members (Read-Only)

            public Vector2 getCenter()
            {
                return center;
            }

            public float getRadius()
            {
                return radius;
            }

            //------------------------------------------------COLLISION-CHECKING--------------------------------------

            public bool checkCollision(collisionRectangle rect)
            {
                //BOOYAH!!!!
                if (rect.checkCollision(this))
                    return true;
                else
                    return false;
            }

            public bool checkCollision(collisionCircle circe)
            {
                Vector2 distanceVector2 = center - circe.getCenter();
                float radii = radius + circe.getRadius();

                if (distanceVector2.Length() < radii)
                {
                    return true;
                }
                else
                    return false;
            }
            //------------------------------------------------COLLISION-CHECKING--------------------------------------

            //------------------------------------------------UPDATE-POSITION-----------------------------------------
            public void update(Vector2 originPosition)
            {
                center = originPosition;
            }
            //------------------------------------------------UPDATE-POSITION-----------------------------------------
        }
    }

    public struct BackgroundTile
    {
        Texture2D Sprite;
        Rectangle ScalingRectangle;
        
        //If tiles are animated, then they are entities...
        public BackgroundTile(Texture2D tileText, Vector2 pos, int width, int height)
        {
            Sprite = tileText;

            ScalingRectangle = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Sprite, ScalingRectangle, Color.White);
        }
    }

    public struct Backgrounds2D
    {
        public List<BackgroundTile> backgroundTiles;                     //The various tiles to be drawn
        //public static Entity[] backEntities;                              //The various background entities
        public List<Physics.collisionRectangle> collisionPositions;      //The various positions entities collide with

        //In case of parallaxing backgrounds, or even a normal one to be drawn behind the tiles...
        public List<Texture2D> background;                               //Set indices to 1 if only one back...
        private Rectangle[] backPositions;                           //Where the background is situated...

        private AnimationsStruct animatable;                         //Entity that helps in animations...
        private int animationTiming;                                 //timing the animation...
        private Rectangle[] sourceRect;                               //Rectangle used for animations...


        //---------------------------------------------------------INITIALIZATION-------------------------------------

        //NOTE: background array is filled by the programmer within the game, there is no function in order to 
        //populate this array.
        //Call all initialization functions only after the backgrounds array as been initialized

        public void Initialize()
        {
            background = new List<Texture2D>();

            collisionPositions = new List<Physics.collisionRectangle>();
            backgroundTiles = new List<BackgroundTile>();

            animationTiming = 0;
        }

        public void InitializeBackground()
        {
            if (background.Count > 0)
            {
                backPositions = new Rectangle[background.Count];
                for (int i = 0; i < backPositions.Length; i++)
                {
                    //backPositions.Add(new Rectangle(0, 0, screenWidth, screenHeight));

                    backPositions[i].X = 0;
                    backPositions[i].Y = 0;
                    backPositions[i].Width = GameInformation.getScreenWidth();
                    backPositions[i].Height = GameInformation.getScreenHeight();
                }
            }
        }

        public void InitializeAnimation(int rows, int cols, int totalFrames, bool sideWays)
        {
            //Single animated background...
            //background list will contain only one texture...

            animatable = new AnimationsStruct();

            //Frame width and height...
            int frameWidth, frameHeight;

            if(background.Count > 0)
            {
                frameWidth = background[0].Width / rows;
                frameHeight = background[0].Height / cols;
            }
            else
            {
                frameWidth = 0;
                frameHeight = 0;
            }

            //setting up for animations...
            animatable.InitializeAnimations(rows, cols, totalFrames, frameWidth, frameHeight, sideWays);

            sourceRect = new Rectangle[1];

            sourceRect[0].X = (int)animatable.getFrame().X;
            sourceRect[0].Y = (int)animatable.getFrame().Y;
            sourceRect[0].Width = frameWidth;
            sourceRect[0].Height = frameHeight;
        }

        public void InitializeSideScroller(bool sideWays)
        {
            //In this case, backPositions should have 3 positions, 3 frames.
            //backPositions = new Rectangle[3];

            //First position
            if (sideWays == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    backPositions[i].X = i * GameInformation.getScreenWidth();
                    backPositions[i].Y = 0;
                    backPositions[i].Width = GameInformation.getScreenWidth();
                    backPositions[i].Height = GameInformation.getScreenHeight();

                    //backPositions.Add(new Rectangle(i * screenWidth, 0, screenWidth, screenHeight));
                }
            }
            else
            {
                for(int i = 0; i < 3; i++)
                {
                    backPositions[i].Y = i * -(GameInformation.getScreenHeight());
                    backPositions[i].X = 0;
                    backPositions[i].Width = GameInformation.getScreenWidth();
                    backPositions[i].Height = GameInformation.getScreenHeight();
                }
            }
        }

        public void InitializeParallaxing(int layers)
        {
            //if()
        }
        //---------------------------------------------------------INITIALIZATION-------------------------------------

        //---------------------------------------------------------UPDATION-------------------------------------------
        public void UpdateSideScroller(bool sideWays, int speed)
        {
            if (sideWays == true)
            {
                //In this case, background length is 1. The single background is drawn repeatedly...
                if (backPositions.Length > 0)
                {
                    if (backPositions[0].X <= -(2 * GameInformation.getScreenWidth()))
                        backPositions[0].X = 0;

                    backPositions[0].X -= speed;
                    backPositions[1].X = backPositions[0].X + GameInformation.getScreenWidth();
                    backPositions[2].X = backPositions[0].X + (GameInformation.getScreenWidth() * 2);
                }
            }
            else
            {
                //In this case, background length is 1. The single background is drawn repeatedly...
                if(backPositions.Length > 0)
                {
                    if (backPositions[0].Y >= (2 * GameInformation.getScreenHeight()))
                        backPositions[0].Y = 0;

                    backPositions[0].Y += speed;
                    backPositions[1].Y = backPositions[0].Y - GameInformation.getScreenHeight();
                    backPositions[2].Y = backPositions[0].Y - GameInformation.getScreenHeight();
                }
            }
        }

        public void UpdateAnimated(GameTime gameTime, int frameRate)
        {
            if (animationTiming < frameRate)
                animationTiming += 1;
            else
                animationTiming = 0;

            if (animationTiming >= frameRate)
            {
                if (animatable.currentFrame <= animatable.totalFrames)
                {
                    animatable.currentFrame += 1;
                }
                else
                {
                    animatable.currentFrame = 0;
                }
            }

            sourceRect[0].X = (int)animatable.getFrame().X;
            sourceRect[0].Y = (int)animatable.getFrame().Y;

        }
        //---------------------------------------------------------UPDATION-------------------------------------------

        //---------------------------------------------------------DRAWING--------------------------------------------
        //NOTE: Fill the values of these static entities in each game's Game1 method...
        public void DrawTiles(SpriteBatch batch)
        {
            foreach (BackgroundTile tile in backgroundTiles)
            {
                tile.Draw(batch);
            }
        }

        public void DrawBackground(SpriteBatch batch)
        {
            //In case only one background...
            if (background.Count > 0)
            {
                batch.Draw(background[0], backPositions[0], Color.White);
            }
        }

        public void DrawSideScroller(SpriteBatch batch)
        {
            //In this case, background Length is 1, indexed at 0.
            //backPositions contains 3 frames...
            if (background.Count > 0)
            {
                batch.Draw(background[0], backPositions[0], Color.White);
                batch.Draw(background[0], backPositions[1], Color.White);
                batch.Draw(background[0], backPositions[2], Color.White);
            }
        }

        public void DrawAnimation(SpriteBatch batch)
        {
            batch.Draw(background[0], backPositions[0], sourceRect[0], Color.White);

            /*batch.DrawString(GameInformation.getFont(), "Frames[1] = " + animatable.getFrameIndex(1).X,
                new Vector2(200, 200), Color.Ivory);*/
        }
        //---------------------------------------------------------DRAWING--------------------------------------------
    }

    public struct AnimationsStruct
    {
        //Only used to populate frames, get concept of currentFrames, not for drawing...
        Dictionary<int, Vector2> Frames;
        public int totalFrames;
        public int currentFrame;
        int frameWidth, frameHeight;

        //---------------------------------------------GETTERS---------------------------------------------------------
        public int getFrameWidth()
        {
            return frameWidth;
        }

        public int getFrameHeight()
        {
            return frameHeight;
        }

        public Vector2 getFrameIndex(int key)
        {
            if (Frames.ContainsKey(key))
                return Frames[key];
            else
                return Frames[0];
        }

        public Vector2 getFrame()
        {
            if (Frames.ContainsKey(currentFrame))
                return Frames[currentFrame];
            else
                return Frames[0];
        }

        public Dictionary<int, Vector2> getFrames()
        {
            return Frames;
        }
        //---------------------------------------------GETTERS---------------------------------------------------------
        public void InitializeAnimations(int rows, int columns, int totalFrames, int frameWidth, int frameHeight, bool sideWays)
        {
            //First of all, Initialize Frames
            Frames = new Dictionary<int, Vector2>();

            //Set the value of currentFrame...
            currentFrame = 0;

            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            //This method assumes that the sprite atlas frames are positioned correctly...
            int frameIndex = 0;                 //Index to be stored as animation frame number
            this.totalFrames = totalFrames;     //The total number of frames in texture atlas...
            int xValue = 0;                     //xValue of particular frame
            int yValue = 0;                     //yValue of particular frame

            if (sideWays == true)
            {
                while (frameIndex < totalFrames)
                {
                    if (xValue >= rows)
                    {
                        xValue = 0;
                        yValue += 1;
                    }

                    if (yValue > columns)
                        break;

                    Frames.Add(frameIndex, new Vector2(frameWidth * xValue, frameHeight * yValue));

                    xValue += 1;

                    frameIndex += 1;
                }
            }
            else
            {
                while (frameIndex < totalFrames)
                {
                    if (yValue >= columns)
                    {
                        yValue = 0;
                        xValue += 1;
                    }

                    if (xValue > rows)
                        break;

                    Frames.Add(frameIndex, new Vector2(frameWidth * xValue, frameHeight * yValue));

                    yValue += 1;

                    frameIndex += 1;
                }
            }
        }
    }

    //Entities collide, animate and draw
    //Presenting...
    public class Entity
    {
        protected Texture2D Sprite;                           //Graphics of the entity
        protected Vector2 Position;                           //Position of the entity
        protected float xSpeed, ySpeed;                       //The speed with which the entity moves...
        protected const float Gravity = 1f;                //The gravity that needs to be applied on entities if platformer
        protected int acceleration;                         //Acceleration used for falling...
        protected float fallingSpeed = 0;                     //The speed with which the entity will fall
        protected float jumpingSpeed = 0;                     //The speed with which the entity jumps

        //Width and height to be drawn...
        protected int drawWidth, drawHeight;

        //Platformer states
        protected bool isJumping = false;                     //Whether the entity is jumping
        protected int jumpTiming = 0;                         //Timing of how long jump should last...
        protected int jumpHeight = 0;                         //How high player has jumped...
        protected bool jumpApex = false;                      //Stay in air for some time
        protected bool isFalling = false;                     //Whether the entity is falling
        protected enum SideFacing
        {
            LEFT,
            RIGHT,
            UP,
            DOWN,
            NEUTRAL
        }

        //Deleting and killing entities...
        protected bool timeForTermination;                      //Play dying sequence...
        protected bool timeForDeletion;                        //Set the entity reference to null..

        protected SideFacing facing;                           //In case of platformers, where the entity is facing...

        //Rotating RPG Information
        protected Vector2 origin;                             //Origin of rotation
        protected float angle;                                //Rotation Angle in radians

        //Timing
        protected int deltaTime = 0;                           //It will have an individual update...

        //Collisions...
        protected Physics.collisionCircle collisionCircle;          //Usually for collisions in top-down games
        protected Physics.collisionRectangle collisionRectangle;    //Usually for collisions in platformers...
        protected int selectedCollisionEntity = 0;                  //Which entity is chosen, 1, 2, or 3 for both

        //Animation Controls...
        protected AnimationsStruct animations;                            //Structure that takes care of frames...
        protected int animationTiming;                                    //The frame rate checker
        protected int frameRate;                                          //The frame rate
        protected Rectangle drawRectangle;                                //Rectangle to scale entity...
        protected Rectangle sourceRect;                                   //Rectangle for frames of animation...

        //----------------------------------------------INITIALIZATION----------------------------------------------
        public void Initialize(Texture2D sprite, Vector2 pos)
        {
            Sprite = sprite;
            Position = pos;

            jumpTiming = 0;
            isJumping = false;
            jumpApex = false;

            facing = SideFacing.RIGHT;

            animationTiming = 0;
        }

        public void InitializeSize(int dWidth, int dHeight)
        {
            //Width and height to be drawn...
            drawWidth = dWidth;
            drawHeight = dHeight;
        }

        public void InitializeRPG(Vector2 origin)
        {
            angle = 0;                                  //Angle measured in radians
            this.origin = origin;
        }

        public void InitializeAnimation(int totalFrames, int rows, int columns, bool sideWays, int frameRate)
        {
            animations = new AnimationsStruct();

            int frameWidth = Sprite.Width / rows;
            int frameHeight = Sprite.Height / columns;

            animations.InitializeAnimations(rows, columns, totalFrames, frameWidth, frameHeight, sideWays);
            sourceRect = new Rectangle((int)animations.getFrame().X, (int)animations.getFrame().Y, frameWidth, frameHeight);
            drawRectangle = new Rectangle((int)Position.X, (int)Position.Y, drawWidth, drawHeight);
        }

        public void InitializeCollisionEntity(Physics.collisionCircle circe)
        {
            collisionCircle = circe;
            selectedCollisionEntity += 1;
        }

        public void InitializeCollisionEntity(Physics.collisionRectangle rect)
        {
            collisionRectangle = rect;
            selectedCollisionEntity += 2;
        }
        //----------------------------------------------INITIALIZATION----------------------------------------------


        //Access Position (Read-Only)
        public Vector2 getPosition()
        {
            return Position;
        }

        //Access width and height (Read-Only)
        public int getWidth()
        {
            return drawWidth;
        }

        public int getHeight()
        {
            return drawHeight;
        }

        //-----------------------------------COLLISION-ENTITITES---------------------------------------------------
        public Physics.collisionCircle returnCollisionCircle()
        {
            return collisionCircle;
        }

        public Physics.collisionRectangle returnCollisionRectangle()
        {
            return collisionRectangle;
        }
        //-----------------------------------COLLISION-ENTITITES--------------------------------------------------

        //------------------------------------------RPG-----------------------------------------------------------
        //RPG Rotation
        protected void increaseAngle(float f)
        {
            angle += f;
        }

        protected void decreaseAngle(float f)
        {
            angle -= f;
        }

        protected Vector2 getDirection()
        {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
        //------------------------------------------RPG-----------------------------------------------------------

        //Implementing individual updates...
        //-------------------------------------------PLATFORMER------------------------------------------------
        /*protected void fallingUpdate(float Speed)
        {
            if (isFalling == true)
            {
                if (fallingSpeed < Speed)
                    fallingSpeed += 0.2f;

                else fallingSpeed = Speed;
            }

            else
                fallingSpeed = 0;

            Position.Y += fallingSpeed;
        }*/

        protected void fallingUpdate(float Speed)
        {
            if (isFalling == true)
                //fallingSpeed = Speed;
                Position.Y += Speed;

            /*else
                fallingSpeed = 0;*/

            //Position.Y += fallingSpeed;
        }

        //Add acceleration, keep adding it to the ySpeed...
        protected void RealfallingUpdate(float Speed)
        {
            if (isFalling == true)
            {
                if (acceleration < Speed)
                    acceleration += (int)Gravity;
            }
            else
                acceleration = 0;

            Position.Y += acceleration;
        }

        protected void checkFalling(List<Physics.collisionRectangle> colPos)
        {
            //Check falling is only performed when collision rectangle is selected
            if (selectedCollisionEntity != 2)
            {
                isFalling = false;
            }

            if(selectedCollisionEntity == 2)
            {
                if (isJumping == false && jumpApex == false)
                {
                    if (collisionRectangle.down == true)
                        isFalling = false;
                    else
                        isFalling = true;
                }
            }
        }

        protected void jumpingUpdate(float speed, int jumpTime, int apexTime)
        {
                if (isJumping == true)
                {
                    jumpTiming += 1;
                    Position.Y -= speed;
                }

                if(jumpTiming > jumpTime)
                {
                    isJumping = false;
                    jumpApex = true;
                }

                if(jumpApex == true)
                {
                    jumpTiming += 1;

                    if(jumpTiming > jumpTime + apexTime)
                    {
                        jumpApex = false;
                        jumpTiming = 0;
                    }
                }
        }

        /*protected void jumpingUpdateReal(float initialYValue, float Speed)
        {
            if (isJumping == true)
            {
                if (initialYValue - Position.Y <= GameInformation.getGridHeight() * 3)
                    isJumping = false;
                else
                    Position.Y -= Speed;
            }
        }*/

        protected void positionUpdate()
        {
            Position.X += xSpeed;
            Position.Y += ySpeed;

            drawRectangle.X = (int)Position.X;
            drawRectangle.Y = (int)Position.Y;
        }

        protected bool collisionChecking(List<Physics.collisionRectangle> colPos)
        {
            if (selectedCollisionEntity != 0)
                if (selectedCollisionEntity == 1)
                {
                    foreach (Physics.collisionRectangle rect in colPos)
                    {
                        if (collisionCircle.checkCollision(rect) == true)
                        {
                            return true;
                        }
                    }

                    return false;
                }
                else if (selectedCollisionEntity == 2)
                {
                    foreach (Physics.collisionRectangle rect in colPos)
                    {
                        if (collisionRectangle.checkCollision(rect) == true)
                            return true;
                    }
                }
                    return false;
        }
        //-------------------------------------------PLATFORMER------------------------------------------------

        public void updateCollisionEntity()
        {
            if(selectedCollisionEntity == 1)
            {
                //Collision Circle...
                collisionCircle.update(new Vector2(Position.X + (drawWidth / 2), Position.Y + (drawHeight / 2)));     //Take circle to mid position
            }

            else if (selectedCollisionEntity == 2)
            {
                //Collision Rectangle...
                collisionRectangle.update(Position);
            }

            else if (selectedCollisionEntity == 3)
            {
                collisionCircle.update(new Vector2(drawWidth / 2, drawHeight / 2));
                collisionRectangle.update(Position);
            }
        }

        public void UpdateAnimations()
        {
            sourceRect.X = (int)animations.getFrame().X;
            sourceRect.Y = (int)animations.getFrame().Y;
        }

        //---------------------------------------------DRAW-FUNCTIONS--------------------------------------------
        public void DrawRotation(SpriteBatch batch)
        {
            batch.Draw(Sprite, drawRectangle, sourceRect, Color.White, angle, origin, SpriteEffects.None, 0f);
        }

        public void DrawPlatformer(SpriteBatch batch)
        {
            //Whether facing left or right...
            switch(facing)
            {
                case SideFacing.LEFT:
                    batch.Draw(Sprite, drawRectangle, sourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                    break;

                case SideFacing.RIGHT:
                    batch.Draw(Sprite, drawRectangle, sourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    break;
            }

            //batch.Draw(Sprite, drawRectangle, sourceRect, Color.White);
        }

        public void Draw(SpriteBatch batch)
        {
            //batch.Draw(Sprite, drawRectangle, sourceRect, Color.White);
            //batch.Draw(Sprite, sourceRect, drawRectangle, Color.White);
            batch.Draw(Sprite, Position, Color.White);
        }
        //---------------------------------------------DRAW-FUNCTIONS--------------------------------------------
    }

    public struct Camera2D
    {
        //Add functions for following character
        //Add functions for reaching a certain point 
        //Add functions for rotating and scaling too

        public Matrix cameraMatrix;

        public void Initialize(Matrix cameraMatrix)
        {
            this.cameraMatrix = cameraMatrix;
        }


        //NOTE: For playerPos, add the position which the camera should follow, not the actual player position...
        public void updateMatrix(Vector2 playerPos, int screenWidth, int screenHeight, float scaleX, float scaleY, float rotation)
        {
            Vector2 pos = new Vector2();
            pos.X = playerPos.X - (screenWidth / 2);
            pos.Y = playerPos.Y - (screenHeight / 4);

            //Always remember, createTranslation takes the values in negative form...

            cameraMatrix = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
                Matrix.CreateScale(scaleX, scaleY, 1) *
                Matrix.CreateRotationZ(rotation);
        }
    }
}
