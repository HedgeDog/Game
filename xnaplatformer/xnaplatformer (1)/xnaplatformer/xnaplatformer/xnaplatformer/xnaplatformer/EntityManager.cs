using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace xnaplatformer
{
    class EntityManager
    {
        List<Entity> entities;
        List<List<string>> attributes, contents;
        FileManager fileManager;
        InputManager input;

        public List<Entity> Entities
        {
            get { return entities; }
        }

        public void LoadContent(string entityType, ContentManager Content, string fileName, string identifier, InputManager input)
        {
            entities = new List<Entity>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            fileManager = new FileManager();
            this.input = input;

            if (identifier == string.Empty)
            {
                fileManager.LoadContent(fileName, attributes, contents);
            }
            else
            {
                fileManager.LoadContent(fileName, attributes, contents, identifier);
            }

            for (int i = 0; i < attributes.Count; i++)
            {
                Type newClass = Type.GetType("xnaplatformer." + entityType);
                entities.Add(((Entity)Activator.CreateInstance(newClass)));
                entities[i].LoadContent(Content, attributes[i], contents[i], this.input);
            }
        }

        public void UnloadContent()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].UnloadContent();
            } 
        }

        public void Update(GameTime gameTime, Map map)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime, input, map.collision, map.layer);
                if (entities[i].Health <= 0)
                {
                    entities[i].OnDeath();
                    entities.Remove(entities[i]);
                }
            }
        }

        public void EntityCollision(EntityManager E2)
        {
            foreach (Entity e in Entities)
            {
                foreach (Entity e2 in E2.Entities)
                {
                    if (e.Rect.intersects(e2.Rect))
                    {
                        e.OnCollision(e2);
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw(spriteBatch);
            }
        }
    }
}
