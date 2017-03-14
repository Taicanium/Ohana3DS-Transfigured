using Ohana3DS_Rebirth.Ohana.Models.PocketMonsters;
using Ohana3DS_Transfigured.Ohana.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ohana3DS_Transfigured.Ohana.Models.PocketMonsters
{

    class CP
    {
        /// <summary>
        ///     Loads a CP overworld character model from Pokémon.
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns>The Model group with the character meshes</returns>
        public static RenderBase.OModelGroup load(Stream data)
        {
            RenderBase.OModelGroup models = new RenderBase.OModelGroup();

            OContainer container = PkmnContainer.load(data);
            models = CM.load(new MemoryStream(container.content[1].data));

            return models;
        }
    }
}
