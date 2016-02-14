using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

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
            else if (PKMagic2 == "BM")
            {
                data.Seek(4, SeekOrigin.Begin);

                int bchTableIndex = 0;

                uint[] begins = new uint[100];
                uint[] ends = new uint[100];

                begins[bchTableIndex] = input.ReadUInt32();
            	ends[bchTableIndex] = input.ReadUInt32();

            	while (ends[bchTableIndex] < data.Length)
	            {
		            bchTableIndex++;

            		begins[bchTableIndex] = input.ReadUInt32();
            		ends[bchTableIndex] = input.ReadUInt32();
            	}

                for (int i = 0; i < bchTableIndex; i++)
                {
                    data.Seek(begins[bchTableIndex], SeekOrigin.Begin);

                    byte[] buffer = new byte[(int)ends[bchTableIndex] - (int)begins[bchTableIndex]];

                    data.Read(buffer, 0, (int)ends[bchTableIndex] - (int)begins[bchTableIndex]);

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

                return group;
            }
            else if (PKMagic2 == "PF")
            {
                data.Seek(4, SeekOrigin.Begin);

                uint begin;
                uint end;
                uint length;

                bool eof = false;

                for (int i = 0; eof == false; i++)
                {
                    data.Seek(8 + i * 8, SeekOrigin.Begin);

                    try
                    {
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
                    catch
                    {
                        eof = true;
                    }
                }
            }

            data.Close();

            if (group.skeletalAnimation.list.Count > 0)
            {
                MessageBox.Show("This animation file contains skeletal animations.");
            }

            return group;
        }
    }
}
