using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Raylib;
using static Raylib.Raylib;

namespace ConsoleApp1
{
    class Game
    {
        Stopwatch stopwatch = new Stopwatch();

        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;

        private float deltaTime = 0.005f;
        
        //This list is used for keeping track of everything in the scene and makes it easy to run everything 
        //through foreach loops.
        List<SceneObject> Hierarchy = new List<SceneObject>();

        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();
        SceneObject shellObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();
        SpriteObject shellSprite = new SpriteObject();

        //Sets up the dimensions of the player's hitbox, then sets them in an array for access later.
        MathHelpers.AABB playerCollider = new MathHelpers.AABB(new MathHelpers.Vector3(0,0,0), new MathHelpers.Vector3(0,0,0));
        SceneObject[] playerCorners = new SceneObject[4]
        {
            new SceneObject(), new SceneObject(), new SceneObject(), new SceneObject()
        };
         
        //The color and dimensions of the green box's hitbox, the box itself is drawn in Draw.
        Color boxColor = Color.GREEN;
        MathHelpers.AABB boxCollider = new MathHelpers.AABB(new MathHelpers.Vector3(300, 300, 0), new MathHelpers.Vector3(400, 400, 0));

        public void Init()
        {
            SetTargetFPS(60);
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            Hierarchy.Add(shellObject);
            Hierarchy.Add(tankObject);


            tankSprite.Load("tankblue_outline.png");
            //tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            //tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);

            turretSprite.Load("barrelBlue.png");
            turretSprite.SetPosition(0, 0);
            //turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            //turretSprite.SetPosition(turretSprite.Width / 2.0f, 0);
            turretSprite.SetPosition(25, 0);

            shellSprite.Load("TankGameShell.png");


            turretObject.AddChild(turretSprite);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turretObject);

            tankObject.SetPosition(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);

        }

        public void Shutdown() { }

        public void Update()
        {
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;
            Timer coolDown = new Timer();

            timer += deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }

            frames++;
            #region Movement
            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_W))
            {
                //MathHelpers.Vector3 facing = new MathHelpers.Vector3(
                //    tankObject.LocalTransform.m[0],
                //    tankObject.LocalTransform.m[1], 1) * deltaTime * 100;
                //tankObject.Translate(facing.x, facing.y);
                MathHelpers.Vector3 facing = new MathHelpers.Vector3(
                    tankObject.LocalTransform.m1,
                    tankObject.LocalTransform.m2, 1) * deltaTime * 100;
                tankObject.Translate(facing.x, facing.y);

            }
            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                //MathHelpers.Vector3 facing = new MathHelpers.Vector3(
                //tankObject.LocalTransform.m[0],
                //tankObject.LocalTransform.m[1], 1) * deltaTime * -100;
                //tankObject.Translate(facing.x, facing.y);
                MathHelpers.Vector3 facing = new MathHelpers.Vector3(
                    tankObject.LocalTransform.m1,
                    tankObject.LocalTransform.m2, 1) * deltaTime * -100;
                tankObject.Translate(facing.x, facing.y);


            }
            if (IsKeyDown(KeyboardKey.KEY_Q))
            {
                turretObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(deltaTime);
            }
            #endregion Movement

            //Spawns a projectile that moves in the direction the barrel was pointed when pressed.
            if (IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                Projectile shell = new Projectile(turretObject.GlobalTransform.m5, -turretObject.GlobalTransform.m4);
                shell.SetPosition(turretObject.GlobalTransform.m7, turretObject.GlobalTransform.m8);
                shellObject.AddChild(shell);


                //MathHelpers.Vector3 facing = new MathHelpers.Vector3 (
                //    tankObject.LocalTransform.m1,
                //    tankObject.LocalTransform.m2, 1) *deltaTime * -1000;
                //tankObject.Translate(facing.x, facing.y);
            }

            //Checks to see if the player's collider interacts with the green box.
            if (boxCollider.Overlaps(playerCollider))
                boxColor = Color.DARKGREEN;
            else
                boxColor = Color.GREEN;

            //Goes through the children of shellObject and checks their lifespans. 
            //If they expire they are removed from the shellObject.
            for (int i = 0; i < shellObject.GetChildCount(); i++)
            {
                if(shellObject.GetChild(i).proDel)
                {
                    shellObject.RemoveChild(shellObject.GetChild(i));

                }
            }

            for (int i = 0; i < shellObject.GetChildCount(); i++)
            {
                Projectile tShell = (Projectile)shellObject.GetChild(i);

                //If the green box isn't currently overlapping with the player
                //it will check if it's overlapping with a projectile; if it is, it will turn red.
                if (!boxCollider.Overlaps(playerCollider))
                {
                    if(tShell.projectileCollider.Overlaps(boxCollider))
                    {
                        boxColor = Color.RED;
                        break;
                    }
                    else
                    {
                        boxColor = Color.GREEN;
                    }
                }
            }

            tankObject.UpdateTransform();

            //Moves the collider in tandem with the tank by "resizing" it to the current dimensions 
            //(and by extent, the position) of the player.
            playerCollider.Resize(new MathHelpers.Vector3(tankObject.GlobalTransform.m7 - (tankSprite.Width / 2), tankObject.GlobalTransform.m8 - (tankSprite.Height / 2), 0),
                                  new MathHelpers.Vector3(tankObject.GlobalTransform.m7 + (tankSprite.Width / 2), tankObject.GlobalTransform.m8 + (tankSprite.Height / 2), 0));

            Vector2 v2 = new Vector2(900, 24);
            lastTime = currentTime;

            //
            foreach (SceneObject i in Hierarchy)
            {
                i.Update(deltaTime);
            }

        }


        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.WHITE);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);

            //MathHelpers.AABB boxCollider = new MathHelpers.AABB(new MathHelpers.Vector3(120, 120, 0), new MathHelpers.Vector3(200, 200, 0));
            //boxCollider.Corners();

            //The green box's sprite.
            DrawRectangle(300, 300, 100, 100, boxColor);

            //Goes through the Hierarchy and draws each sceneobject.
            foreach (SceneObject i in Hierarchy)
            {
                i.Draw();
            }

            //tankObject.Draw();
            EndDrawing();
        }
    }
}
