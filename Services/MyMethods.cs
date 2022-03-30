using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EnergyTrade.Services
{
    public static class MyMethods
    {
        public static byte[] ConverToBytes(HttpPostedFileBase file)
        {
            var length = file.InputStream.Length; //Length: 103050706
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileData;
        }
        public static string ConvertToImg(byte[] file)
        {
            var base64 = Convert.ToBase64String(file);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
            return imgSrc;
        }

    }
}