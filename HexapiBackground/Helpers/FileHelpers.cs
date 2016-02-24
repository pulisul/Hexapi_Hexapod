﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HexapiBackground
{
    internal static class FileHelpers
    {
        internal static async Task<string> ReadStringFromFile(string filename)
        {
            var text = string.Empty;
            try
            {
                var file =
                    await
                        ApplicationData.Current.LocalFolder.CreateFileAsync(filename,
                            CreationCollisionOption.OpenIfExists).AsTask();

                Debug.WriteLine($"Reading {file.Path}");

                using (var stream = await file.OpenStreamForReadAsync())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        text = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return text;
        }

        internal static void SaveStringToFile(string filename, string content)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    var bytesToAppend = Encoding.UTF8.GetBytes(content.ToCharArray());
                    var file =
                        await
                            ApplicationData.Current.LocalFolder.CreateFileAsync(filename,
                                CreationCollisionOption.OpenIfExists).AsTask();

                    Debug.WriteLine($"Writing {file.Path}");

                    using (var stream = await file.OpenStreamForWriteAsync())
                    {
                        stream.Position = stream.Length;
                        stream.Write(bytesToAppend, 0, bytesToAppend.Length);
                    }
                }
                catch
                {
                    Debug.WriteLine("Save failed for " + filename);
                }
            });
        }
    }
}