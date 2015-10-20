﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ohana3DS_Rebirth.Ohana.Containers
{
    public class PkmnContainer
    {
        /// <summary>
        ///     Reads a generic Pokémon container from a File.
        ///     Those containers are the ones that starts with "GR", "MM", "AD" and so on...
        /// </summary>
        /// <param name="fileName">The File Name where the data is located</param>
        /// <returns></returns>
        public static GenericContainer.OContainer load(string fileName)
        {
            return load(new FileStream(fileName, FileMode.Open));
        }

        /// <summary>
        ///     Reads a generic Pokémon container from a Stream.
        ///     Those containers are the ones that starts with "GR", "MM", "AD" and so on...
        /// </summary>
        /// <param name="data">Stream with container data</param>
        /// <returns></returns>
        public static GenericContainer.OContainer load(Stream data)
        {
            BinaryReader input = new BinaryReader(data);
            GenericContainer.OContainer output = new GenericContainer.OContainer();

            output.fileIdentifier = IOUtils.readString(input, 0, 2); //Magic

            ushort sectionCount = input.ReadUInt16();
            for (int i = 0; i < sectionCount; i++)
            {
                GenericContainer.OContainerEntry entry = new GenericContainer.OContainerEntry();

                data.Seek(4 + (i * 4), SeekOrigin.Begin);
                uint startOffset = input.ReadUInt32();
                uint endOffset = input.ReadUInt32();
                uint length = endOffset - startOffset;

                data.Seek(startOffset, SeekOrigin.Begin);
                byte[] buffer = new byte[length];
                input.Read(buffer, 0, (int)length);
                entry.data = buffer;

                output.content.Add(entry);
            }

            return output;
        }
    }
}
