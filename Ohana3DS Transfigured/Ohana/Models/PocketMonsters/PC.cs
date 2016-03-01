using System.IO;

using Ohana3DS_Transfigured.Ohana.Containers;

namespace Ohana3DS_Transfigured.Ohana.Models.PocketMonsters
{
    class PC
    {
        public static RenderBase.OModelGroup load(string file)
        {
            return load(File.Open(file, FileMode.Open));
        }

        /// <summary>
        ///     Loads a PC monster model from Pokémon.
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns>The Model group with the monster meshes</returns>
        public static RenderBase.OModelGroup load(Stream data)
        {
            RenderBase.OModelGroup models = new RenderBase.OModelGroup();

            OContainer container = PkmnContainer.load(data);

            models = BCH.load(new MemoryStream(container.content[0].data));

            return models;
        }
    }
}
