using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using Ohana3DS_Rebirth.Ohana.ModelFormats;

namespace Ohana3DS_Rebirth.Ohana.TextureFormats
{
    class PT
    {
        public static List<RenderBase.OTexture> load(string fileName)
        {
            return load(new MemoryStream(File.ReadAllBytes(fileName)));
        }

        public static List<RenderBase.OTexture> load(Stream data)
        {
            List<RenderBase.OTexture> textures = new List<RenderBase.OTexture>();

            BinaryReader input = new BinaryReader(data);

            uint fileLength;

            string ptMagic = IOUtils.readString(input, 0, 2);
            ushort refCount = input.ReadUInt16();

            data.Seek(4 + refCount * 4, SeekOrigin.Begin);
            fileLength = input.ReadUInt32();

            uint begin;
            uint end;
            uint length;

            data.Seek(0, SeekOrigin.Begin);

            for (int i = 0; i < refCount; i++)
            {
                data.Seek(4 + i * 4, SeekOrigin.Begin);

                begin = input.ReadUInt32();
                end = input.ReadUInt32();
                length = end - begin;

                if (length != 0)
                {
                    data.Seek(begin, SeekOrigin.Begin);

                    byte[] buffer = new byte[length];
                    input.Read(buffer, 0, (int)length);

                    if (buffer[0] == 0x42 && buffer[1] == 0x43 && buffer[2] == 0x48)
                    {
                        RenderBase.OModelGroup group = BCH.load(new MemoryStream(buffer));

                        for (int j = 0; j < group.texture.Count; j++)
                        {
                            textures.Add(group.texture[j]);
                        }
                    }
                }
            }

            data.Close();

            return textures;
        }
    }
}
