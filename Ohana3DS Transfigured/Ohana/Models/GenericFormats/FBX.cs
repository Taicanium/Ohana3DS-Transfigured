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

            bool hasShiny = false;

            // Attempt to detect packaged textures. See http://www.github.com/Quibilia/Ohana3DS-Transfigured for directions.
            try
            {
                string modelDir;
                string sDir;
                string[] texDirs;
                string[] texFiles;

                modelDir = Path.GetFileNameWithoutExtension(fileName).Remove(4);
                sDir = Path.GetDirectoryName(fileName);
                texDirs = Directory.GetDirectories(Path.Combine(sDir, "../../Textures/"));

                foreach (string texDir in texDirs)
                {
                    if (texDir.Contains(modelDir))
                    {
                        texFiles = Directory.GetFiles(texDir);

                        if (Directory.Exists(texDir.Replace("Textures", "Models/DAE")) == false && texDir.Contains("Xtra") == false)
                        {
                            Directory.CreateDirectory(texDir.Replace("Textures", "Models/DAE"));
                        }

                        bool hasIris = false;

                        foreach (string texFile in texFiles)
                        {
                            if (texFile.Contains("Iris1"))
                            {
                                hasIris = true;
                            }
                        }

                        foreach (string texFile in texFiles)
                        {
                            if (texFile.Contains("Xtra") == false)
                            {
                                Bitmap tempBMP = (Bitmap)Bitmap.FromFile(texFile);

                                if (texFile.Contains("Body") && texFile.Contains("Nor") == false)
                                {
                                    int w = tempBMP.Width / 2;

                                    Bitmap scaledBMP = new Bitmap(tempBMP, tempBMP.Width / 2, tempBMP.Height);
                                    Bitmap reverseBMP = new Bitmap(tempBMP, tempBMP.Width / 2, tempBMP.Height);
                                    reverseBMP.RotateFlip(RotateFlipType.RotateNoneFlipX);

                                    Bitmap finalBMP = new Bitmap(tempBMP.Width, tempBMP.Height);

                                    for (int x = 0; x < scaledBMP.Width; x++)
                                    {
                                        for (int y = 0; y < scaledBMP.Height; y++)
                                        {
                                            finalBMP.SetPixel(x, y, scaledBMP.GetPixel(x, y));
                                        }
                                    }

                                    for (int x = 0; x < reverseBMP.Width; x++)
                                    {
                                        for (int y = 0; y < reverseBMP.Height; y++)
                                        {
                                            finalBMP.SetPixel(x + reverseBMP.Width, y, reverseBMP.GetPixel(x, y));
                                        }
                                    }

                                    if (texFile.Contains("BodyA2") == false && texFile.Contains("BodyB2") == false && texFile.Contains("Body2") == false)
                                    {
                                        finalBMP.Save(texFile.Replace("Textures", "Models/DAE"));

                                        model.texture.Add(new RenderBase.OTexture(finalBMP, texFile.Replace("Textures", "Models/DAE")));
                                    }
                                }
                                else if (texFile.Contains("Iris1"))
                                {
                                    Bitmap eyeBMP = (Bitmap)Bitmap.FromFile(texFile.Replace("Iris", "Eye"));

                                    foreach (string norFile in texFiles)
                                    {
                                        if (norFile.Contains("Xtra") == false && norFile.Contains("EyeNor"))
                                        {
                                            if (eyeBMP.Width == eyeBMP.Height)
                                            {
                                                Bitmap norBMP = (Bitmap)Bitmap.FromFile(norFile);
                                                Bitmap thisNor = new Bitmap(norBMP.Width / 4, norBMP.Height / 4);
                                                Bitmap thisEye = new Bitmap(eyeBMP.Width / 2, eyeBMP.Height / 4);
                                                Bitmap irisBMP = new Bitmap(norBMP.Width / 4, norBMP.Height / 4);
                                                Bitmap finalBMP = new Bitmap(norBMP.Width, norBMP.Height);

                                                int currentEye = 0;

                                                for (int eyeX = 0; eyeX < 4; eyeX++)
                                                {
                                                    for (int eyeY = 0; eyeY < 4; eyeY++)
                                                    {
                                                        for (int x = eyeX * thisNor.Width; x < (eyeX + 1) * thisNor.Width; x++)
                                                        {
                                                            for (int y = eyeY * thisNor.Height; y < (eyeY + 1) * thisNor.Height; y++)
                                                            {
                                                                thisNor.SetPixel(x - (eyeX * thisNor.Width), y - (eyeY * thisNor.Height), norBMP.GetPixel(x, y));
                                                            }
                                                        }

                                                        for (int x = currentEye * thisEye.Width; x < (currentEye + 1) * thisEye.Width; x++)
                                                        {
                                                            for (int y = eyeY * thisEye.Height; y < (eyeY + 1) * thisEye.Height; y++)
                                                            {
                                                                thisEye.SetPixel(x - (currentEye * thisEye.Width), y - (eyeY * thisEye.Height), eyeBMP.GetPixel(x, y));
                                                            }
                                                        }

                                                        for (int x = 0; x < thisNor.Width; x++)
                                                        {
                                                            for (int y = 0; y < thisNor.Height; y++)
                                                            {
                                                                if (thisNor.GetPixel(x, y).A == 0x00)
                                                                {
                                                                    irisBMP.SetPixel(x, y, thisEye.GetPixel(x, y));
                                                                }
                                                                else
                                                                {
                                                                    irisBMP.SetPixel(x, y, tempBMP.GetPixel(x, y));
                                                                }
                                                            }
                                                        }

                                                        for (int x = 0; x < irisBMP.Width; x++)
                                                        {
                                                            for (int y = 0; y < irisBMP.Height; y++)
                                                            {
                                                                if (irisBMP.GetPixel(x, y).A == 0x00)
                                                                {
                                                                    irisBMP.SetPixel(x, y, Color.White);
                                                                }
                                                            }
                                                        }

                                                        for (int x = eyeX * thisNor.Width; x < (eyeX + 1) * thisNor.Width; x++)
                                                        {
                                                            for (int y = eyeY * thisNor.Width; y < (eyeY + 1) * thisNor.Width; y++)
                                                            {
                                                                finalBMP.SetPixel(x, y, irisBMP.GetPixel(x - (eyeX * thisNor.Width), y - (eyeY * thisNor.Height)));
                                                            }
                                                        }
                                                    }

                                                    if (eyeX % 2 != 0)
                                                    {
                                                        currentEye++;
                                                    }
                                                }

                                                finalBMP.Save(texFile.Replace("Textures", "Models/DAE"));
                                                model.texture.Add(new RenderBase.OTexture(finalBMP, texFile.Replace("Textures", "Models/DAE")));
                                            }
                                            else
                                            {
                                                Bitmap norBMP = (Bitmap)Bitmap.FromFile(norFile);
                                                Bitmap thisNor = new Bitmap(norBMP.Width / 4, norBMP.Height / 4);
                                                Bitmap thisEye = new Bitmap(eyeBMP.Width / 2, eyeBMP.Height / 4);
                                                Bitmap irisBMP = new Bitmap(norBMP.Width / 4, norBMP.Height / 4);
                                                Bitmap finalBMP = new Bitmap(norBMP.Width, norBMP.Height);

                                                int currentEye = 0;

                                                for (int eyeX = 0; eyeX < 4; eyeX++)
                                                {
                                                    if (eyeX == 0)
                                                    {
                                                        tempBMP.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                    }

                                                    tempBMP.RotateFlip(RotateFlipType.RotateNoneFlipX);

                                                    for (int eyeY = 0; eyeY < 4; eyeY++)
                                                    {
                                                        for (int x = eyeX * thisNor.Width; x < (eyeX + 1) * thisNor.Width; x++)
                                                        {
                                                            for (int y = eyeY * thisNor.Height; y < (eyeY + 1) * thisNor.Height; y++)
                                                            {
                                                                thisNor.SetPixel(x - (eyeX * thisNor.Width), y - (eyeY * thisNor.Height), norBMP.GetPixel(x, y));
                                                            }
                                                        }

                                                        for (int x = currentEye * thisEye.Width; x < (currentEye + 1) * thisEye.Width; x++)
                                                        {
                                                            for (int y = eyeY * thisEye.Height; y < (eyeY + 1) * thisEye.Height; y++)
                                                            {
                                                                thisEye.SetPixel(x - (currentEye * thisEye.Width), y - (eyeY * thisEye.Height), eyeBMP.GetPixel(x, y));
                                                            }
                                                        }

                                                        if (eyeX % 3 /* Don't ask. */ == 0)
                                                        {
                                                            thisEye.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                                        }

                                                        for (int x = 0; x < thisNor.Width; x++)
                                                        {
                                                            for (int y = 0; y < thisNor.Height; y++)
                                                            {
                                                                if (thisNor.GetPixel(x, y).A == 0x00)
                                                                {
                                                                    irisBMP.SetPixel(x, y, thisEye.GetPixel(x, y));
                                                                }
                                                                else
                                                                {
                                                                    irisBMP.SetPixel(x, y, tempBMP.GetPixel(x, y));
                                                                }
                                                            }
                                                        }

                                                        for (int x = 0; x < irisBMP.Width; x++)
                                                        {
                                                            for (int y = 0; y < irisBMP.Height; y++)
                                                            {
                                                                if (irisBMP.GetPixel(x, y).A == 0x00)
                                                                {
                                                                    irisBMP.SetPixel(x, y, Color.White);
                                                                }
                                                            }
                                                        }

                                                        // Because 3DS Max is picky about how to align UVs.
                                                        irisBMP.RotateFlip(RotateFlipType.RotateNoneFlipX);

                                                        for (int x = eyeX * thisNor.Width; x < (eyeX + 1) * thisNor.Width; x++)
                                                        {
                                                            for (int y = eyeY * thisNor.Width; y < (eyeY + 1) * thisNor.Width; y++)
                                                            {
                                                                finalBMP.SetPixel(x, y, irisBMP.GetPixel(x - (eyeX * thisNor.Width), y - (eyeY * thisNor.Height)));
                                                            }
                                                        }
                                                    }

                                                    if (eyeX % 2 != 0)
                                                    {
                                                        currentEye++;
                                                    }
                                                }

                                                finalBMP.Save(texFile.Replace("Textures", "Models/DAE"));
                                                model.texture.Add(new RenderBase.OTexture(finalBMP, texFile.Replace("Textures", "Models/DAE")));
                                            }
                                        }
                                    }
                                }
                                else if (texFile.Contains("Mouth1") && texFile.Contains("Nor") == false)
                                {
                                    bool hasNormals = false;

                                    foreach (string norFile in texFiles)
                                    {
                                        // If the mouth has a normal file, the UV coordinates expect it split across the image border.

                                        if (norFile.Contains("Mouth") && norFile.Contains("Nor"))
                                        {
                                            hasNormals = true;
                                        }
                                    }

                                    if (hasNormals == true)
                                    {
                                        Bitmap thisHalf = new Bitmap(tempBMP.Width / 2, tempBMP.Height / 4);
                                        Bitmap finalBMP = new Bitmap(tempBMP.Width * 2, tempBMP.Height);

                                        for (int halfY = 0; halfY < 4; halfY++)
                                        {
                                            for (int halfX = 0; halfX < 2; halfX++)
                                            {
                                                for (int x = halfX * (tempBMP.Width / 2); x < (halfX + 1) * (tempBMP.Width / 2); x++)
                                                {
                                                    for (int y = halfY * (tempBMP.Height / 4); y < (halfY + 1) * (tempBMP.Height / 4); y++)
                                                    {
                                                        thisHalf.SetPixel(x - (halfX * (tempBMP.Width / 2)), y - (halfY * (tempBMP.Height / 4)), tempBMP.GetPixel(x, y));
                                                    }
                                                }

                                                for (int x = halfX * (tempBMP.Width / 2); x < (halfX + 1) * (tempBMP.Width / 2); x++)
                                                {
                                                    for (int y = halfY * (tempBMP.Height / 4); y < (halfY + 1) * (tempBMP.Height / 4); y++)
                                                    {
                                                        finalBMP.SetPixel(x, y, thisHalf.GetPixel(x - (halfX * (tempBMP.Width / 2)), y - (halfY * (tempBMP.Height / 4))));
                                                    }
                                                }

                                                for (int x = halfX * (tempBMP.Width / 2); x < (halfX + 1) * (tempBMP.Width / 2); x++)
                                                {
                                                    for (int y = halfY * (tempBMP.Height / 4); y < (halfY + 1) * (tempBMP.Height / 4); y++)
                                                    {
                                                        finalBMP.SetPixel(finalBMP.Width - (x + 1), y, thisHalf.GetPixel(x - (halfX * (tempBMP.Width / 2)), y - (halfY * (tempBMP.Height / 4))));
                                                    }
                                                }
                                            }
                                        }

                                        finalBMP.Save(texFile.Replace("Textures", "Models/DAE"));
                                        model.texture.Add(new RenderBase.OTexture(finalBMP, texFile.Replace("Textures", "Models/DAE")));
                                    }
                                    else
                                    {
                                        // If the mouth doesn't have a normal file, the UV coordinates expect it as-is. Hooray!

                                        tempBMP.Save(texFile.Replace("Textures", "Models/DAE"));
                                        model.texture.Add(new RenderBase.OTexture(tempBMP, texFile.Replace("Textures", "Models/DAE")));
                                    }
                                }
                                else if (texFile.Contains("Eye1") && hasIris == false && texFile.Contains("AEye") == false && texFile.Contains("BEye") == false & texFile.Contains("CEye") == false)
                                {
                                    if (tempBMP.Width == tempBMP.Height)
                                    {
                                        tempBMP.Save(texFile.Replace("Textures", "Models/DAE"));
                                        model.texture.Add(new RenderBase.OTexture(tempBMP, texFile.Replace("Textures", "Models/DAE")));
                                    }
                                    else
                                    {
                                        Bitmap thisEye = new Bitmap(tempBMP.Width / 2, tempBMP.Height / 4);
                                        Bitmap finalBMP = new Bitmap(tempBMP.Width * 2, tempBMP.Height);

                                        for (int eyeY = 0; eyeY < 4; eyeY++)
                                        {
                                            for (int eyeX = 0; eyeX < 2; eyeX++)
                                            {
                                                for (int x = eyeX * (tempBMP.Width / 2); x < (eyeX + 1) * (tempBMP.Width / 2); x++)
                                                {
                                                    for (int y = eyeY * (tempBMP.Height / 4); y < (eyeY + 1) * (tempBMP.Height / 4); y++)
                                                    {
                                                        thisEye.SetPixel(x - (eyeX * (tempBMP.Width / 2)), y - (eyeY * (tempBMP.Height / 4)), tempBMP.GetPixel(x, y));
                                                    }
                                                }

                                                for (int x = eyeX * (tempBMP.Width / 2); x < (eyeX + 1) * (tempBMP.Width / 2); x++)
                                                {
                                                    for (int y = eyeY * (tempBMP.Height / 4); y < (eyeY + 1) * (tempBMP.Height / 4); y++)
                                                    {
                                                        finalBMP.SetPixel(x, y, thisEye.GetPixel(x - (eyeX * (tempBMP.Width / 2)), y - (eyeY * (tempBMP.Height / 4))));
                                                    }
                                                }

                                                for (int x = eyeX * (tempBMP.Width / 2); x < (eyeX + 1) * (tempBMP.Width / 2); x++)
                                                {
                                                    for (int y = eyeY * (tempBMP.Height / 4); y < (eyeY + 1) * (tempBMP.Height / 4); y++)
                                                    {
                                                        finalBMP.SetPixel(finalBMP.Width - (x + 1), y, thisEye.GetPixel(x - (eyeX * (tempBMP.Width / 2)), y - (eyeY * (tempBMP.Height / 4))));
                                                    }
                                                }
                                            }
                                        }

                                        finalBMP.Save(texFile.Replace("Textures", "Models/DAE"));
                                        model.texture.Add(new RenderBase.OTexture(finalBMP, texFile.Replace("Textures", "Models/DAE")));
                                    }
                                }
                                else if (texFile.Contains("AEye") || texFile.Contains("BEye") || texFile.Contains("CEye"))
                                {
                                    Bitmap finalBMP = new Bitmap(tempBMP, tempBMP.Width / 2, tempBMP.Height);

                                    finalBMP.Save(texFile.Replace("Textures", "Models/DAE"));
                                    model.texture.Add(new RenderBase.OTexture(finalBMP, texFile.Replace("Textures", "Models/DAE")));
                                }
                                else if (texFile.Contains("Fire"))
                                {
                                    Bitmap finalBMP = new Bitmap(tempBMP);

                                    foreach (string bodyFile in texFiles)
                                    {
                                        if (bodyFile.Contains("BodyA1") && bodyFile.Contains("Nor") == false)
                                        {
                                            Bitmap bodyBMP = (Bitmap)Bitmap.FromFile(bodyFile);

                                            byte r, g, b;

                                            // Most-Frequent Body Color
                                            Color MFBC = Color.FromArgb(0xFF, 0xFF, 0x00, 0x00), LFBC = Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00);
                                            int LF = 0, SF = 65536;

                                            List<Color> colors = new List<Color>();
                                            List<int> freqs = new List<int>();

                                            for (int x = 0; x < bodyBMP.Width; x++)
                                            {
                                                for (int y = 0; y < bodyBMP.Height; y++)
                                                {
                                                    r = bodyBMP.GetPixel(x, y).R;
                                                    g = bodyBMP.GetPixel(x, y).G;
                                                    b = bodyBMP.GetPixel(x, y).B;

                                                    if (bodyBMP.GetPixel(x, y).A != 0x00)
                                                    {
                                                        bool matchFound = false;

                                                        foreach (Color c in colors)
                                                        {
                                                            if (c.R == r && c.G == g && c.B == b)
                                                            {
                                                                matchFound = true;
                                                                int freq = freqs[colors.IndexOf(c)];
                                                                freq++;

                                                                freqs[colors.IndexOf(c)] = freq;
                                                            }
                                                        }

                                                        if (matchFound == false)
                                                        {
                                                            colors.Add(Color.FromArgb(r, g, b));
                                                            freqs.Add(1);
                                                        }
                                                    }
                                                }
                                            }

                                            int tolerance = 18;

                                            foreach (Color c in colors)
                                            {
                                                if (freqs[colors.IndexOf(c)] > LF)
                                                {
                                                    LF = freqs[colors.IndexOf(c)];
                                                    MFBC = c;
                                                }
                                                else if (freqs[colors.IndexOf(c)] + 150 < SF)
                                                {
                                                    if (c.R <= (c.G - tolerance) || c.R >= (c.G + tolerance))
                                                    {
                                                        if (c.B <= (c.G - tolerance) || c.B >= (c.G + tolerance))
                                                        {
                                                            SF = freqs[colors.IndexOf(c)];
                                                            LFBC = c;
                                                        }
                                                    }
                                                }
                                            }

                                            bool mfbcgray = false;
                                            bool lfbcgray = false;
                                            bool bothgray = false;
                                            bool neithergray = false;

                                            if (MFBC.R > (MFBC.G - tolerance) && MFBC.R < (MFBC.G + tolerance))
                                            {
                                                if (MFBC.B > (MFBC.G - tolerance) && MFBC.B < (MFBC.G + tolerance))
                                                {
                                                    mfbcgray = true;
                                                }
                                            }

                                            if (LFBC.R > (LFBC.G - tolerance) && LFBC.R < (LFBC.G + tolerance))
                                            {
                                                if (LFBC.B > (LFBC.G - tolerance) && LFBC.B < (LFBC.G + tolerance))
                                                {
                                                    lfbcgray = true;
                                                }
                                            }

                                            if (mfbcgray && lfbcgray)
                                            {
                                                bothgray = true;
                                            }

                                            if (!mfbcgray && !lfbcgray)
                                            {
                                                neithergray = true;
                                            }

                                            Color baseOperator;
                                            Color accentOperator;

                                            if (bothgray)
                                            {
                                                baseOperator = Color.FromArgb(0xFF, 0xFF, 0x00, 0x00);
                                                accentOperator = Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00);
                                            }
                                            else if (neithergray)
                                            {
                                                baseOperator = Color.FromArgb(0xFF, 0xFF, 0x00, 0x00);
                                                accentOperator = Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00);
                                            }
                                            else
                                            {
                                                if (mfbcgray)
                                                {
                                                    baseOperator = LFBC;
                                                    accentOperator = LFBC;
                                                }
                                                else if (lfbcgray)
                                                {
                                                    baseOperator = MFBC;
                                                    accentOperator = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                                                }
                                                else
                                                {
                                                    // Just to satisfy C#...
                                                    baseOperator = MFBC;
                                                    accentOperator = LFBC;
                                                }
                                            }

                                            for (int x = 0; x < finalBMP.Width; x++)
                                            {
                                                for (int y = 0; y < finalBMP.Height; y++)
                                                {
                                                    Color tempColor = tempBMP.GetPixel(x, y);
                                                    Color finalColor;

                                                    if (tempColor.R < 0x10 && tempColor.G < 0x10 && tempColor.B < 0x10)
                                                    {
                                                        finalColor = Color.FromArgb(0x1F, baseOperator.R, baseOperator.G, baseOperator.B);
                                                    }
                                                    else
                                                    {
                                                        finalColor = Color.FromArgb(0x1F, tempColor.R & accentOperator.R, tempColor.G & accentOperator.G, tempColor.B & accentOperator.B);
                                                    }

                                                    finalBMP.SetPixel(x, y, finalColor);
                                                }
                                            }

                                            finalBMP.Save(texFile.Replace("Textures", "Models/DAE"));
                                            model.texture.Add(new RenderBase.OTexture(finalBMP, texFile.Replace("Textures", "Models/DAE")));
                                        }
                                    }
                                }
                                else
                                {
                                    if (texFile.Contains("Iris2") == false && texFile.Contains("Eye2") == false && texFile.Contains("Mouth2") == false)
                                    {
                                        tempBMP.Save(texFile.Replace("Textures", "Models/DAE"));
                                        model.texture.Add(new RenderBase.OTexture(tempBMP, texFile.Replace("Textures", "Models/DAE")));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch // No packaged textures detected...
            {
            }

            // Parts of the string that are (*ENCASED*) will be String.Replace()'d with variables.
            string FBXBody = "Definitions: { Version: 100  Count: (*DEFCOUNT*)  ObjectType: \"GlobalSettings\" { Count: 1 } ";

            foreach (RenderBase.OMesh mesh in mdl.mesh)
            {
                // Still RE'ing the format...
            }
        }
    }
}
