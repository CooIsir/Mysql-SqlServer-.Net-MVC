using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace com.Utility
{
    public class zipHelper
    {
        public static byte[] Compress(byte[] pBytes)
        {
            MemoryStream mMemory = new MemoryStream(); 
            GZipStream zipStream = new GZipStream(mMemory, CompressionMode.Compress,true);
            zipStream.Write(pBytes, 0, pBytes.Length);
            zipStream.Close(); 
            return mMemory.ToArray();
        }
        public static byte[] DeCompress(byte[] pBytes) 
        {             
            MemoryStream mMemory = new MemoryStream(); 
            GZipStream mStream = new GZipStream(new MemoryStream(pBytes), CompressionMode.Decompress, true);
            int mSize;
            byte[] mWriteData = new byte[4096];
            while(true)
            {
                mSize = mStream.Read(mWriteData, 0, mWriteData.Length);
                if (mSize > 0)
                {
                    mMemory.Write(mWriteData, 0, mSize);
                }
                else
                {
                     break;
                }
            }
            mStream.Close();
            return mMemory.ToArray();
        }
    }
}
