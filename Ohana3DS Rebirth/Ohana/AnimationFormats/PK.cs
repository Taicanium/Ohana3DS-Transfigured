using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using Ohana3DS_Rebirth.Ohana.ModelFormats;

namespace Ohana3DS_Rebirth.Ohana.AnimationFormats
{
    class PK
    {
        public static RenderBase.OModelGroup load(string fileName)
        {
            return load(new MemoryStream(File.ReadAllBytes(fileName)));
        }

        public static RenderBase.OModelGroup load(Stream data)
        {
            RenderBase.OModelGroup group = new RenderBase.OModelGroup();

            BinaryReader input = new BinaryReader(data);

            uint fileLength;

            string PKMagic2 = IOUtils.readString(input, 0, 2);
            data.Seek(0, SeekOrigin.Begin);
            string PKMagic3 = IOUtils.readString(input, 0, 3);
            data.Seek(0, SeekOrigin.Begin);

            if (PKMagic3 == "PBJ")
            {
                data.Seek(4, SeekOrigin.Begin);
                uint refCountPos = input.ReadUInt32();
                data.Seek(refCountPos, SeekOrigin.Begin);
            }
            else if (PKMagic3 == "PKj")
            {
                data.Seek(4, SeekOrigin.Begin);
                uint refCountPos = input.ReadUInt32();
                data.Seek(refCountPos, SeekOrigin.Begin);
            }
            else
            {
                data.Seek(3, SeekOrigin.Begin);
            }

            ushort refCount = input.ReadUInt16();

            data.Seek(4 + refCount * 8, SeekOrigin.Begin);
            fileLength = input.ReadUInt32();

            uint begin;
            uint end;
            uint length;

            for (int i = 0; i < refCount; i++)
            {
                data.Seek(4 + i * 8, SeekOrigin.Begin);

                begin = input.ReadUInt32();
                end = input.ReadUInt32();

                //PK files seem to vary in their order of variables.
                if (end > begin)
                {
                    length = end - begin;
                }
                else
                {
                    length = begin - end;
                }

                if (length > 0)
                {
                    if (end > begin)
                    {
                        data.Seek(begin, SeekOrigin.Begin);
                    }
                    else
                    {
                        data.Seek(end, SeekOrigin.Begin);
                    }

                    byte[] buffer = new byte[length];
                    input.Read(buffer, 0, (int)length);

                    if (buffer[0] == 0x42 && buffer[1] == 0x43 && buffer[2] == 0x48)
                    {
                        RenderBase.OModelGroup tempGroup = BCH.load(new MemoryStream(buffer));
                        
                        for (int j = 0; j < tempGroup.skeletalAnimation.list.Count; j++)
                        {
                            group.skeletalAnimation.list.Add(tempGroup.skeletalAnimation.list[j]);
                        }

                        for (int j = 0; j < tempGroup.materialAnimation.list.Count; j++)
                        {
                            group.materialAnimation.list.Add(tempGroup.materialAnimation.list[j]);
                        }

                        for (int j = 0; j < tempGroup.visibilityAnimation.list.Count; j++)
                        {
                            group.visibilityAnimation.list.Add(tempGroup.visibilityAnimation.list[j]);
                        }
                    }
                }
            }

            data.Close();

            return group;
        }
    }
}
