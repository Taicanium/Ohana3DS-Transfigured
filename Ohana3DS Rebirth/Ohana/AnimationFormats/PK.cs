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
            RenderBase.OModelGroup tempGroup;
            byte[] buffer;

            BinaryReader input = new BinaryReader(data);

            string PKMagic2 = IOUtils.readString(input, 0, 2);
            data.Seek(0, SeekOrigin.Begin);

            if (PKMagic2 == "PB")
            {
                data.Seek(4, SeekOrigin.Begin);

                uint begin;
                uint end;
                uint length;

                bool eof = false;

                for (int i = 0; eof == false; i++)
                {
                    try
                    {
                        data.Seek(12 + i * 8, SeekOrigin.Begin);
                    }
                    catch
                    {
                        eof = true;
                    }

                    try
                    {
                        begin = input.ReadUInt32();
                        end = input.ReadUInt32();

                        if (begin < data.Length && end < data.Length)
                        {
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

                                buffer = new byte[length];
                                input.Read(buffer, 0, (int)length);

                                if (buffer[0] == 0x42 && buffer[1] == 0x43 && buffer[2] == 0x48)
                                {
                                    tempGroup = BCH.load(new MemoryStream(buffer));

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
                    }
                    catch
                    {
                        eof = true;
                    }
                }
            }
            else if (PKMagic2 == "PK")
            {
                data.Seek(8, SeekOrigin.Begin);

                uint begin;
                uint end;
                uint length;

                bool eof = false;

                begin = 0;
                end = 0;

                for (int i = 0; eof == false; i++)
                {
                    try
                    {
                        data.Seek(12 + i * 8, SeekOrigin.Begin);
                    }
                    catch
                    {
                        eof = true;
                    }

                    try
                    {
                        begin = input.ReadUInt32();
                        end = input.ReadUInt32();

                        if (begin < data.Length && end < data.Length)
                        {
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

                                buffer = new byte[length];
                                input.Read(buffer, 0, (int)length);

                                if (buffer[0] == 0x42 && buffer[1] == 0x43 && buffer[2] == 0x48)
                                {
                                    tempGroup = BCH.load(new MemoryStream(buffer));

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
                    }
                    catch
                    {
                        eof = true;
                    }
                }
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

                    buffer = new byte[(int)ends[bchTableIndex] - (int)begins[bchTableIndex]];

                    data.Read(buffer, 0, (int)ends[bchTableIndex] - (int)begins[bchTableIndex]);

                    tempGroup = BCH.load(new MemoryStream(buffer));

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
                    try
                    {
                        data.Seek(12 + i * 8, SeekOrigin.Begin);
                    }
                    catch
                    {
                        eof = true;
                    }

                    try
                    {
                        begin = input.ReadUInt32();
                        end = input.ReadUInt32();

                        if (begin < data.Length && end < data.Length)
                        {
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

                                buffer = new byte[length];
                                input.Read(buffer, 0, (int)length);

                                if (buffer[0] == 0x42 && buffer[1] == 0x43 && buffer[2] == 0x48)
                                {
                                    tempGroup = BCH.load(new MemoryStream(buffer));

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
