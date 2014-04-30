using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace xnaplatformer
{
    class GamePlayScreen : GameScreen
    {
        EntityManager player;
        EntityManager enemies;
        Map map;

        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            player = new EntityManager();
            enemies = new EntityManager();
            map = new Map();
            map.LoadContent(content, map, "FirstMap");
            player.LoadContent("Player", content, "Load/Player.cme", "", input);
            enemies.LoadContent("Enemy", content, "Load/Enemy1.txt", "Level1", input);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            player.Update(gameTime, map);
            enemies.Update(gameTime, map);
            map.Update(gameTime);

            Entity e;
            for (int i = 0; i < player.Entities.Count; i++)
            {
                e = player.Entities[i];
                map.UpdateCollision(ref e);
                player.Entities[i] = e;
            }
            for (int i = 0; i < enemies.Entities.Count; i++)
            {
                e = enemies.Entities[i];
                map.UpdateCollision(ref e);
                enemies.Entities[i] = e;
            }

            player.EntityCollision(enemies);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            enemies.Draw(spriteBatch);
        }
    }
}
