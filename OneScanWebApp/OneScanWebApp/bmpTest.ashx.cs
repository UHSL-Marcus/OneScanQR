using OneScanWebApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for bmpTest
    /// </summary>
    public class bmpTest : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ProcessRequest(HttpContext context)
        {




            Bitmap bmp = QRGen.GenerateQRCode("http://ensygnia.com/?pn0gENH/cWIC+AXqLEII5Q==");
            
            

            int rowLength = (int)calculateRowSize(bmp.Width);
            List<BitArray> rows = new List<BitArray>();
            byte[] pixelBytes = new byte[rowLength * Math.Abs(bmp.Height)];
            int currIdx = 0;
 
            try
            {

                for (int i = 0; i < bmp.Height; i++)
                {
                    BitArray row = new BitArray(rowLength*8);
                    row.SetAll(false);

                    for (int j = 0; j < bmp.Width; j++)
                    {
                        Color pixel = bmp.GetPixel(j, i);
                        string output = j + "," + i + " -> R: " + pixel.R + " G: " + pixel.G + " B: " + pixel.B + " Ave: " + (pixel.R + pixel.G + pixel.B) / 3;
                        //System.Diagnostics.Debug.WriteLine(output);

                        if (((pixel.R + pixel.G + pixel.B) / 3) >= 128)
                            row.Set(j, true);
                    }

                    currIdx = addBitArrayToByteArray(row, ref pixelBytes, currIdx);
                    
                }
            }
            catch (Exception e) { }



            byte[] colourTable = new byte[8];
            fillArray(colourTable, 0xff, 3);

            byte[] DIBHeader = new byte[40];
            currIdx = 0;

            currIdx = addArrayToArray(DIBHeader, intToByteArray(40), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(bmp.Width), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(bmp.Height), currIdx);
            currIdx = addArrayToArray(DIBHeader, int16ToByteArray(1), currIdx);
            currIdx = addArrayToArray(DIBHeader, int16ToByteArray(1), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(0), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(0), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(3780), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(3780), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(2), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(2), currIdx);

            byte[] header = new byte[14];
            currIdx = 0;
            header[currIdx++] = 0x42;
            header[currIdx++] = 0x4D;

            currIdx = addArrayToArray(header, intToByteArray(header.Length + DIBHeader.Length + colourTable.Length + pixelBytes.Length), currIdx);
            currIdx = fillArray(header, 0x0, currIdx, 4);
            currIdx = addArrayToArray(header, intToByteArray(header.Length + DIBHeader.Length + colourTable.Length), currIdx);



            byte[] fullHeader = new byte[header.Length + DIBHeader.Length + colourTable.Length];
            header.CopyTo(fullHeader, 0);
            DIBHeader.CopyTo(fullHeader, header.Length);
            colourTable.CopyTo(fullHeader, header.Length + DIBHeader.Length);

            byte[] fullBmp = new byte[fullHeader.Length + pixelBytes.Length];
            fullHeader.CopyTo(fullBmp, 0);
            //pixelBytes.CopyTo(fullBmp, fullHeader.Length);

            File.WriteAllBytes(@"c:\byteHeaderFile", fullHeader);
            File.WriteAllBytes(@"c:\byteFile", pixelBytes);
            File.WriteAllBytes(@"c:\bmpFile", fullBmp);







            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                Bitmap convBmp = BitmapConvert.CopyToBpp(bmp, 1);
                convBmp.Save(stream, ImageFormat.Bmp);
                stream.Close();

                byteArray = stream.ToArray();
            }

            context.Response.ContentType = "image/bmp";
            context.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
            //context.Response.OutputStream.Write(fullBmp, 0, fullBmp.Length);

            File.WriteAllBytes(@"c:\byte32File", byteArray);

        }

        private int addArrayToArray(byte[] ba, byte[] add, int off)
        {
            for (int i = 0; i < add.Length; i++)
                ba[off++] = add[i];

            return off;
        }

        private int fillArray(byte[] ba, byte add, int off, int count = -1)
        {
            if ((count += off) < off) count = ba.Length;

            for (int i = off; i < count; i++)
                ba[off++] = add;

            return off;
        }

        private int addBitArrayToByteArray(BitArray bits, ref byte[] bytes, int off)
        {
            byte currentByte = 0;
            int count = 0;

            foreach (bool bit in bits)
            {
                if (bit) currentByte |= (byte)(1 << count);
                if (++count == 8)
                {
                    bytes[off++] = currentByte;
                    currentByte = 0; count = 0;
                }
            }
            if (count < 8) bytes[off++] = currentByte;

            return off;
        }

        private double calculateRowSize(int imgWidth)
        {
            return Math.Floor((double)(1 * imgWidth + 31) / 32) * 4;
        }



        
        private byte[] intToByteArray(int i)
        {
            byte lolo = (byte)(i & 0xff);
            byte hilo = (byte)((i >> 8) & 0xff);
            byte lohi = (byte)((i >> 16) & 0xff);
            byte hihi = (byte)(i >> 24);

            return new byte[] { lolo, hilo, lohi, hihi };
        }

        private byte[] int16ToByteArray(short i)
        {
            byte lo = (byte)(i & 0xff);
            byte hi = (byte)((i >> 8) & 0xff);

            return new byte[] { lo, hi };
        }


    }
}