
namespace MonopolyTracker.Shared.Data
{
    using System;
    using System.IO;
    using Tesseract;
    using MonopolyTracker.Shared.Models;
    using static MonopolyTracker.Shared.Constants;

    public static class ImageClient
    {
        private const string Language = "eng";

        private static readonly string DataPath =
            Path.GetFullPath(
                Path.Combine(ResourcesPath, "tessdata"));

        public static (string Text, Result Result) GetImageText(Entry entry)
        {
            if (entry is null || string.IsNullOrWhiteSpace(entry.Contents))
            {
                return (default, new Result { Successful = false, Message = "Image path is null" });
            }

            try
            {
                using (var tEngine = new TesseractEngine(ImageClient.DataPath, Language, EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(entry.Contents))
                    {
                        using (var page = tEngine.Process(img))
                        {
                            var text = page.GetText();
                            var confidence = page.GetMeanConfidence();

                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                return (text, new Result { Successful = true, Message = $"OCR found {text.Length} characters" });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (default, new Result { Successful = false, Message = ex.ToString() });
            }

            return (default, new Result { Successful = true, Message = $"OCR found no text in {entry.Contents}" });
        }

        public static (string Text, Result Result) GetImageText(byte[] image)
        {
            try
            {
                using (var tEngine = new TesseractEngine(ImageClient.DataPath, Language, EngineMode.Default))
                {
                    using (var img = Pix.LoadTiffFromMemory(image))
                    {
                        using (var page = tEngine.Process(img))
                        {
                            var text = page.GetText();
                            var confidence = page.GetMeanConfidence();

                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                return (text, new Result { Successful = true, Message = $"OCR found {text.Length} characters" });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (default, new Result { Successful = false, Message = ex.ToString() });
            }

            return (default, new Result { Successful = true, Message = $"OCR found no text in input image" });
        }
    }
}