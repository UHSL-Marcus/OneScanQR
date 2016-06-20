﻿using QRCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;


namespace OneScanWebApp.Utils
{
    class QRGen
    {
        private QRCode qrCode;

        public QRGen(string info, QRCodeGenerator.ECCLevel ecc)
        {
            QRCodeGenerator qrGen = new QRCodeGenerator();
            QRCodeData qrData = qrGen.CreateQrCode(info, ecc);
            qrCode = new QRCode(qrData); 
        }

        public Bitmap getBitmap(int width, int height)
        {
            int i = 1;
            while (true)
            {
                if (qrCode.GetGraphic(i).Width > width)    
                    return qrCode.GetGraphic(i - 1);

                i++;
            }
        }

        public byte[] get8BitBitmapArray(int width, int height, bool headers = true)
        {

            Bitmap QR = getBitmap(width, height);

            int qrWidth = QR.Width;
            int qrheight = QR.Height;

            if (qrWidth <= width && qrheight <= height)
            {
                int widthOffset = (width - qrWidth) / 2;
                int heightOffset = (height - qrheight) / 2;

                int rowLength = (int)calculateRowSize(width, 8);
                List<BitArray> rows = new List<BitArray>();
                byte[] pixelBytes = new byte[rowLength * Math.Abs(height)];
                for (int i = 0; i < pixelBytes.Length; i++)
                    pixelBytes[i] = 0xff;

                int currIdx = rowLength * heightOffset;

                for (int y = qrheight - 1; y > -1; y--)
                {
                    byte[] row = new byte[rowLength * 8];
                    for (int i = 0; i < row.Length; i++)
                        row[i] = 0xFF;
                        

                    for (int x = 0; x < qrWidth; x++)
                    {

                        Color pixel = QR.GetPixel(x, y);

                        if (((pixel.R + pixel.G + pixel.B) / 3) < 128)
                            row[x + widthOffset] = 0x00;
                    }


                    currIdx = addArrayToArray(pixelBytes, row, currIdx);
                }

                byte[] returnArray;
                if (headers)
                {
                    byte[] headerArray = buildHeader(width, height, pixelBytes.Length);
                    returnArray = new byte[headerArray.Length + pixelBytes.Length];
                    headerArray.CopyTo(returnArray, 0);
                    pixelBytes.CopyTo(returnArray, headerArray.Length);
                }
                else returnArray = pixelBytes;

                return returnArray;
            }

            return null;
        }

        public byte[] getBitmapArray(int width, int height, bool headers = true)
        {

            Bitmap QR = getBitmap(width, height);

            int qrWidth = QR.Width;
            int qrheight = QR.Height;

            if (qrWidth <= width && qrheight <= height)
            {
                int widthOffset = (width - qrWidth)/2;
                int heightOffset = (height - qrheight)/2;

                int rowLength = (int)calculateRowSize(width, 1);
                List<BitArray> rows = new List<BitArray>();
                byte[] pixelBytes = new byte[rowLength * Math.Abs(height)];
                for (int i = 0; i < pixelBytes.Length; i++)
                    pixelBytes[i] = 0xff;

                int currIdx = rowLength*heightOffset;

                for (int y = qrheight - 1; y > -1; y--)
                {
                    BitArray row = new BitArray(rowLength * 8);
                    row.SetAll(true);

                    for (int x = 0; x < qrWidth; x++)
                    {
                        
                        Color pixel = QR.GetPixel(x, y);

                        if (((pixel.R + pixel.G + pixel.B) / 3) < 128)
                            row.Set(x + widthOffset , false);
                    }

                    currIdx = addBitArrayToByteArray(row, ref pixelBytes, currIdx);
                }

                byte[] returnArray;
                if (headers)
                {
                    byte[] headerArray = buildHeader(width, height, pixelBytes.Length);
                    returnArray = new byte[headerArray.Length + pixelBytes.Length];
                    headerArray.CopyTo(returnArray, 0);
                    pixelBytes.CopyTo(returnArray, headerArray.Length);
                }
                else returnArray = pixelBytes;
                
                return returnArray;
            }

            return null;
        }

        private byte[] buildHeader(int width, int height, int pixelSize)
        {
            byte[] colourTable = new byte[8];
            fillArray(colourTable, 0xff, 3);
            //colourTable[7] = 0xff;

            byte[] DIBHeader = new byte[40];
            int currIdx = 0;

            currIdx = addArrayToArray(DIBHeader, intToByteArray(40), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(width), currIdx);
            currIdx = addArrayToArray(DIBHeader, intToByteArray(height), currIdx);
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

            currIdx = addArrayToArray(header, intToByteArray(header.Length + DIBHeader.Length + colourTable.Length + pixelSize), currIdx);
            currIdx = fillArray(header, 0x0, currIdx, 4);
            currIdx = addArrayToArray(header, intToByteArray(header.Length + DIBHeader.Length + colourTable.Length), currIdx);



            byte[] fullHeader = new byte[header.Length + DIBHeader.Length + colourTable.Length];
            header.CopyTo(fullHeader, 0);
            DIBHeader.CopyTo(fullHeader, header.Length);
            colourTable.CopyTo(fullHeader, header.Length + DIBHeader.Length);

            return fullHeader;
        }

        private double calculateRowSize(int imgWidth, int bits)
        {
            int x = (bits * imgWidth + 31) / 32;
            return Math.Floor((double)x) * 4;
        }

        private int addBitArrayToByteArray(BitArray bits, ref byte[] bytes, int off)
        {
            byte currentByte = 0;
            int count = 0;
            int diff = off;

            foreach (bool bit in bits)
            {
                if (bit) currentByte |= (byte)(1 << 8 - (count + 1));
                if (++count == 8)
                {
                    bytes[off++] = currentByte;
                    currentByte = 0; count = 0;
                }
            }
            if (count < 8 && count > 0)
                bytes[off++] = currentByte;

            return off;
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
