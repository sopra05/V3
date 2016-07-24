using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework;

namespace V3.Map
{
    /// <summary>
    /// Parser for the tmx format of the Tiled Map Editor.
    /// Reads XML file and returns corresponding data objects.
    /// </summary>
    public sealed class TiledParser
    {
        private string mFileName;
        // Map Data:
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public SortedList<int, Tileset> TileSets { get; } = new SortedList<int, Tileset>();
        public List<int[,]> MapLayers { get; } = new List<int[,]>();
        public List<Area> Areas { get; } = new List<Area>();

        /// <summary>
        /// Parse the tmx file and hold data in instance properties.
        /// </summary>
        public void Parse(string fileName)
        {
            mFileName = fileName;
            string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string fullPath = directory + "/Content/Maps/" + mFileName + ".tmx";
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128))  // Running on Unix
                fullPath = fullPath.Substring(5);
#if DEBUG
            Console.WriteLine("Loading Map: " + fullPath);
#endif
            XmlReader reader = XmlReader.Create(fullPath);
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "map":
                            ParseMapData(reader);
                            break;
                        case "tileset":
                            ParseTilesetData(reader);
                            break;
                        case "layer":
                            ParseLayerData(reader);
                            break;
                        case "objectgroup":
                            ParseObjectgroup(reader);
                            break;
                    }
                }
            }
        }

        private void ParseMapData(XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "width":
                        MapWidth = reader.ReadContentAsInt();
                        break;
                    case "height":
                        MapHeight = reader.ReadContentAsInt();
                        break;
                    case "tilewidth":
                        TileWidth = reader.ReadContentAsInt();
                        break;
                    case "tileheight":
                        TileHeight = reader.ReadContentAsInt();
                        break;
                }
            }
            reader.MoveToElement();
        }

        private void ParseTilesetData(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                List<string> tilesetAttributes = new List<string>();
                // Read attributes firstgid, name, tilewidth, tileheight, tilecount, columns.
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    tilesetAttributes.Add(reader[i]);
                }
                reader.MoveToElement();
                // Read attributes for tileoffset x and y if existing.
                while (reader.Read())
                {
                    if (reader.Name == "tileoffset")
                    {
                        if (reader.IsStartElement())
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                tilesetAttributes.Add(reader[i]);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (reader.Name == "tile" || reader.Name == "tileset")
                    {
                        break;
                    }
                }
                if (tilesetAttributes.Count == 6)
                {
                    TileSets.Add(int.Parse(tilesetAttributes[0]), new Tileset(tilesetAttributes[1], int.Parse(tilesetAttributes[2]),
                        int.Parse(tilesetAttributes[3]), int.Parse(tilesetAttributes[5])));
                }
                else if (tilesetAttributes.Count == 8)
                {
                    TileSets.Add(int.Parse(tilesetAttributes[0]), new Tileset(tilesetAttributes[1], int.Parse(tilesetAttributes[2]),
                        int.Parse(tilesetAttributes[3]), int.Parse(tilesetAttributes[5]),
                        int.Parse(tilesetAttributes[6]), int.Parse(tilesetAttributes[7])));
                }
                else
                {
                    throw new Exception("Error parsing tileset element in " + mFileName + ".tmx. Does not contain necessary attributes.");
                }
                ParseCollisionData(reader, int.Parse(tilesetAttributes[0]));
            }
        }

        private void ParseLayerData(XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "width")
                {
                    // TODO: Try catching exceptions and throw specific ones.
                    int width = reader.ReadContentAsInt();
                    reader.MoveToNextAttribute();
                    int height = reader.ReadContentAsInt();
                    reader.MoveToElement();
                    reader.ReadToDescendant("data");
                    MapLayers.Add(new int[height, width]);
                    int currentLayerIndex = MapLayers.Count - 1;
                    // Map data is in CSV format, therefore split at comma.
                    string[] layerData = reader.ReadString().Split(',');
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            MapLayers[currentLayerIndex][i, j] = int.Parse(layerData[i * width + j]);
                        }
                    }
                }
            }
            reader.MoveToElement();
        }

        private void ParseCollisionData(XmlReader reader, int currentTileset)
        {
            do
            {
                if (!reader.IsStartElement() && reader.Name == "tileset")
                {
                    // If the end of the tileset note is reached, leave loop.
                    break;
                }
                if (reader.IsStartElement() && reader.Name == "tile" && reader.HasAttributes)
                {
                    string tileId = reader[0];
                    reader.MoveToElement();
                    while (reader.ReadToDescendant("property"))
                    {
                        if (reader.MoveToAttribute("name") && reader.Value == "collision")
                        {
                            reader.MoveToNextAttribute();
                            string collisionData = reader.Value;
                            Tileset tileset = TileSets[currentTileset];
                            if (tileId != null) tileset.AddCollisionData(int.Parse(tileId), collisionData);
                        }
                    }
                }
            }
            while (reader.Read()) ;
        }

        private void ParseObjectgroup(XmlReader reader)
        {
            do
            {
                if (!reader.IsStartElement() && reader.Name == "objectgroup")
                    break;
                if (reader.IsStartElement() && reader.Name == "object")
                {
                    ParseAreaData(reader);
                }
            } while (reader.Read());
        }

        private void ParseAreaData(XmlReader reader)
        {
            string type;
            string name = "";
            double density = 0;
            double chance = 0;
            Rectangle rectangle;
            if (reader.AttributeCount == 7)
            {
                name = reader[1];
                type = reader[2];
                if (!(reader[3] != null && reader[4] != null && reader[5] != null && reader[6] != null))
                    return;
                rectangle = new Rectangle(int.Parse(reader[3]), int.Parse(reader[4]), int.Parse(reader[5]), int.Parse(reader[6]));
            }
            else if (reader.AttributeCount == 6)
            {
                type = reader[1];
                if (!(reader[2] != null && reader[3] != null && reader[4] != null && reader[5] != null))
                    return;
                rectangle = new Rectangle(int.Parse(reader[2]), int.Parse(reader[3]), int.Parse(reader[4]), int.Parse(reader[5]));
            }
            else
            {
                throw new Exception("Error parsing the map. One of the objects has not the right number of attributes, specifically: " + reader.AttributeCount);
            }
            reader.MoveToElement();
            while (reader.Read())
            {
                if (!reader.IsStartElement() && reader.Name == "properties")
                    break;
                if (reader.Name == "property" && reader.HasAttributes)
                {
                    if (reader[2] == null) return;
                    if (reader[0] == "chance")
                    {
                        chance = double.Parse(reader[2], CultureInfo.InvariantCulture);
                    }
                    else if (reader[0] == "density")
                    {
                        density = double.Parse(reader[2], CultureInfo.InvariantCulture);
                    }
                    reader.MoveToElement();
                }
            }
            Area area = new Area(type, rectangle, density, chance, name);
            Areas.Add(area);
        }
    }
}
