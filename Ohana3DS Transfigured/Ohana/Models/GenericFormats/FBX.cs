using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace Ohana3DS_Transfigured.Ohana.Models.GenericFormats
{
    class FBX
    {
        public static void export(RenderBase.OModelGroup model, string fileName, int modelIndex)
        {
            RenderBase.OModel mdl = model.model[modelIndex];

            string FBXHeader = "FBXHeaderExtension: { FBXHeaderVersion: 1003  FBXVersion: 7500  CreationTimeStamp: { Version: 1000  Year: (*YEAR*)  Month: (*MONTH*)  Day: (*DAY*)  Hour: (*HOUR*)  Minute: (*MINUTE*)  Second: (*SECOND*)  Millisecond: (*MILLISECOND*) }  Creator: \"Ohana3DS  Transfigured  version  (*VERSION*)\"  SceneInfo: \"SceneInfo::GlobalInfo\", \"UserData\"  { Type: \"UserData\"  Version: 100  MetaData: { Version: 100  Title: \"\"  Subject: \"\"  Author: \"\"  Keywords: \"\"  Revision: \"\"  Comment: \"\" }  Properties70: { P: \"DocumentUrl\", \"KString\", \"Url\", \"\", \"(*FILENAME*)\"  P: \"SrcDocumentUrl\", \"KString\", \"Url\", \"\", \"(*FILENAME*)\"  P: \"Original\", \"Compound\", \"\", \"\"  P: \"Original|ApplicationVendor\", \"KString\", \"\", \"\", \"Quibilia\"  P: \"Original|ApplicationName\", \"KString\", \"\", \"\", \"Ohana3DS  Transfigured\"  P: \"Original|ApplicationVersion\", \"KString\", \"\", \"\", \"(*VERSION*)\"  P: \"Original|DateTime_GMT\", \"DateTime\", \"\", \"\", \"(*TIMESTAMP*)\"  P: \"Original|FileName\", \"KString\", \"\", \"\", \"(*FILENAME*)\"  P: \"LastSaved\", \"Compound\", \"\", \"\"  P: \"LastSaved|ApplicationVendor\", \"KString\", \"\", \"\", \"Quibilia\"  P: \"LastSaved|ApplicationName\", \"KString\", \"\", \"\", \"Ohana3DS  Transfigured\"  P: \"LastSaved|ApplicationVersion\", \"KString\", \"\", \"\", \"(*VERSION*)\"  P: \"LastSaved|DateTime_GMT\", \"DateTime\", \"\", \"\", \"(*TIMESTAMP*)\" } } }  GlobalSettings: { Version: 1000  Properties70: { P: \"UpAxis\", \"int\", \"Integer\", \"\",1  P: \"UpAxisSign\", \"int\", \"Integer\", \"\",1  P: \"FrontAxis\", \"int\", \"Integer\", \"\",2  P: \"FrontAxisSign\", \"int\", \"Integer\", \"\",1  P: \"CoordAxis\", \"int\", \"Integer\", \"\",0  P: \"CoordAxisSign\", \"int\", \"Integer\", \"\",1  P: \"OriginalUpAxis\", \"int\", \"Integer\", \"\",2  P: \"OriginalUpAxisSign\", \"int\", \"Integer\", \"\",1  P: \"UnitScaleFactor\", \"double\", \"Number\", \"\",2.54  P: \"OriginalUnitScaleFactor\", \"double\", \"Number\", \"\",2.54  P: \"AmbientColor\", \"ColorRGB\", \"Color\", \"\",0,0,0  P: \"DefaultCamera\", \"KString\", \"\", \"\", \"Producer  Perspective\"  P: \"TimeMode\", \"enum\", \"\", \"\",6  P: \"TimeProtocol\", \"enum\", \"\", \"\",2  P: \"SnapOnFrameMode\", \"enum\", \"\", \"\",0  P: \"TimeSpanStart\", \"KTime\", \"Time\", \"\",0  P: \"TimeSpanStop\", \"KTime\", \"Time\", \"\",153953860000  P: \"CustomFrameRate\", \"double\", \"Number\", \"\",-1  P: \"TimeMarker\", \"Compound\", \"\", \"\"  P: \"CurrentTimeMarker\", \"int\", \"Integer\", \"\",-1 } }";

            // INSERT TEXTURE CONVERSION CODE HERE; see DAE.cs

            // Parts of the string that are (*ENCASED*) will be String.Replace()'d with variables.
            string FBXBody = "Definitions: { Version: 100  Count: (*DEFCOUNT*)  ObjectType: \"GlobalSettings\" { Count: 1 } ";

            foreach (RenderBase.OMesh mesh in mdl.mesh)
            {
                // Still RE'ing the format...
            }
        }
    }
}
