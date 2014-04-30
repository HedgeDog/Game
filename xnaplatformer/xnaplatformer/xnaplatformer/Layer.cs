using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace xnaplatformer
{
    public class Layer
    {
        List<Tile> tiles;
        List<List<string>> attributes, contents;
        List<string> motion, solid;
        FileManager fileManager;
        ContentManager content;
        Texture2D tileSheet;
        string[] getMotion;
        string nullTile;

        static public Vector2 TileDimensions
        {
            get { return new Vector2(16, 16); }
        }

        public void LoadContent(Map map, string layerID)
        {
            tiles = new List<Tile>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            motion = new List<string>();
            solid = new List<string>();
            fileManager = new FileManager();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            fileManager.LoadContent("Load/Maps/" + map.ID + ".txt", attributes, contents, layerID);

            int indexY = 0;

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "TileSet":
                            tileSheet = content.Load<Texture2D>("TileSets/" + contents[i][j]);
                            break;
                        case "Solid":
                            solid.Add(contents[i][j]);
                            break;
                        case "Motion":
                            motion.Add(contents[i][j]);
                            break;
                        case "NullTile" :
                            nullTile = contents[i][j];
                            break;
                        case "StartLayer":
                            Tile.Motion tempMotion = Tile.Motion.Static;
                            Tile.State tempState;

                            for (int k = 0; k < contents[i].Count; k++)
                             {
                                if (contents[i][k] != nullTile)
                                {
                                    string[] split = contents[i][k].Split(',');
                                    tiles.Add(new Tile());

                                    if (solid.Contains(contents[i][k]))
                                        tempState = Tile.State.Solid;
                                    else
                                        tempState = Tile.State.Passive;

                                    foreach (string m in motion)
                                    {
                                        getMotion = m.Split(':');
                                        if (getMotion[0] == contents[i][k])
                                        {
                                            tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                            break;
                                        }
                                    }

                                    tiles[tiles.Count - 1].SetTile(tempState, tempMotion, new Vector2(k * TileDimensions.X, indexY * TileDimensions.X), tileSheet,  //tile dimensions are here
                                        new Rectangle(int.Parse(split[0]) * 16, int.Parse(split[1]) * 16, 16, 16));
                                }
                            } 
                            indexY++;
                            break;
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                 tiles[i].Update(gameTime);
            }
        }

        public void UpdateCollision(ref Entity e)
        {
            for (int i = 0; i < tiles.Count; i++)
            {

                 tiles[i].UpdateCollision(ref e);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                    tiles[i].Draw(spriteBatch);
            }
        }
    }
}
