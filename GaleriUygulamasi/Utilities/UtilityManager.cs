using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GaleriUygulamasi.Utilities
{
    public static class UtilityManager
    {
        static string[] dosyaTipleri = { "excel", "pdf", "word", "image", "text", "audio", "video" };
        static string[] dosyaIconlar = { "glyphicon glyphicon-book", "glyphicon glyphicon-text-width", "glyphicon glyphicon-print", "glyphicon glyphicon-picture", "glyphicon glyphicon-italic", "glyphicon glyphicon-volume-up", "glyphicon glyphicon-play-circle" };

        public static byte[] ByteBirlestir(byte[] arrayA, byte[] arrayB)
        {
            byte[] outputBytes = new byte[arrayA.Length + arrayB.Length];
            Buffer.BlockCopy(arrayA, 0, outputBytes, 0, arrayA.Length);
            Buffer.BlockCopy(arrayB, 0, outputBytes, arrayA.Length, arrayB.Length);
            return outputBytes;
        }

        public static string setIcon(string contentType)
        {
            for (int i = 0; i < dosyaTipleri.Length; i++)
            {
                if (contentType.Contains(dosyaTipleri[i]))
                {
                    return dosyaIconlar[i];
                }
            }
            return "glyphicon glyphicon-paperclip";
        }

        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}