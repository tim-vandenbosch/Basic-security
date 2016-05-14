using System;
using System.Drawing;

namespace CryptoProgramma
{
    class StenografieHelper
    {
        public enum Opdracht
        {
            Verbergen,
            Vullen_met_nullen
        };

        /// <summary>
        /// TODO: een regel tekst verbergen in een afbeelding dmv een kleine aanpassing van pixels
        /// </summary>
        /// <param name="text"></param>
        /// <param name="afbeelding"></param>
        /// <returns></returns>
        public static Bitmap embedText(string text, Bitmap afbeelding)
        {
            Opdracht opdracht = Opdracht.Verbergen;
            int letterPositie = 0;
            int letterWaarde = 0;
            long pixelPositie = 0;
            int nullen = 0;
            // één pixel in rgb
            int R = 0, G = 0, B = 0;

            // TODO: De gehele afbeelding afgaan
            for (int i = 0; i < afbeelding.Height; i++)
            {
                for (int j = 0; j < afbeelding.Width; j++)
                {
                    // TODO: De pixel nemen en de kleinste bit beschikbaar maken
                    Color pixel = afbeelding.GetPixel(j, i);

                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    // TODO: R, G en B invullen
                    for (int n = 0; n < 3; n++)
                    {
                        if (pixelPositie % 8 == 0) // Elke rgb bestaat uit 8 bits
                        {
                            // TODO: Als de 8 bits klaar zijn
                            if (opdracht == Opdracht.Vullen_met_nullen && nullen == 8)
                            {
                                //TODO: vul de laatste pixel in
                                if ((pixelPositie - 1) % 3 < 2)
                                {
                                    afbeelding.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                return afbeelding;
                            }

                            // TODO: Verberg eerst de volledige boodschap om vervolgens met nullen te vullen
                            if (letterPositie >= text.Length)
                            {
                                opdracht = Opdracht.Vullen_met_nullen;
                            }
                            else
                            {
                                letterWaarde = text[letterPositie++];
                            }
                        }

                        // TODO: De pixel en rgb waarde waar de data word ingestoken
                        switch (pixelPositie % 3)
                        {
                            // in geval van R
                            case 0:
                                {
                                    if (opdracht == Opdracht.Verbergen)
                                    {
                                        // the rightmost bit in the character will be (charValue % 2)
                                        // to put this value instead of the LSB of the pixel element
                                        // just add it to it
                                        // recall that the LSB of the pixel element had been cleared
                                        // before this operation
                                        R += letterWaarde % 2;

                                        // removes the added rightmost bit of the character
                                        // such that next time we can reach the next one
                                        letterWaarde /= 2;
                                    }
                                }
                                break;
                            // in geval van G
                            case 1:
                                {
                                    if (opdracht == Opdracht.Verbergen)
                                    {
                                        G += letterWaarde % 2;

                                        letterWaarde /= 2;
                                    }
                                }
                                break;
                            // in geval van B
                            case 2:
                                {
                                    if (opdracht == Opdracht.Verbergen)
                                    {
                                        B += letterWaarde % 2;

                                        letterWaarde /= 2;
                                    }

                                    afbeelding.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelPositie++;

                        if (opdracht == Opdracht.Vullen_met_nullen)
                        {
                            // increment the value of zeros until it is 8
                            nullen++;
                        }
                    }
                }
            }
            return afbeelding;
        }

        /// <summary>
        /// TODO: een regel tekst uit een afbeelding halen dmv de kleine aapassingen in pixels te vinden
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static string extractText(Bitmap bmp)
        {
            int colorUnitIndex = 0;
            int charValue = 0;

            // holds the text that will be extracted from the image
            string extractedText = String.Empty;

            // pass through the rows
            for (int i = 0; i < bmp.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

                    // for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            case 0:
                                {
                                    // get the LSB from the pixel element (will be pixel.R % 2)
                                    // then add one bit to the right of the current character
                                    // this can be done by (charValue = charValue * 2)
                                    // replace the added bit (which value is by default 0) with
                                    // the LSB of the pixel element, simply by addition
                                    charValue = charValue * 2 + pixel.R % 2;
                                }
                                break;
                            case 1:
                                {
                                    charValue = charValue * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charValue = charValue * 2 + pixel.B % 2;
                                }
                                break;
                        }

                        colorUnitIndex++;

                        // if 8 bits has been added, then add the current character to the result text
                        if (colorUnitIndex % 8 == 0)
                        {
                            // reverse? of course, since each time the process happens on the right (for simplicity)
                            charValue = reverseBits(charValue);

                            // can only be 0 if it is the stop character (the 8 zeros)
                            if (charValue == 0)
                            {
                                return extractedText;
                            }

                            // convert the character value from int to char
                            char c = (char)charValue;

                            // add the current character to the result text
                            extractedText += c.ToString();
                        }
                    }
                }
            }

            return extractedText;
        }

        /// <summary>
        /// TODO: nullen invullen waar de data is uitgehaald
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int reverseBits(int n)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;

                n /= 2;
            }

            return result;
        }
    }
}
