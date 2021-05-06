namespace Sharptimizer.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Text;

    public class Loader
    {
        /// <summary>
        /// Downloads a file given its URL and the output path to be saved.
        /// <param name="url">URL to download the file.</param>
        /// <param name="output">Path to save the downloaded file.</param>
        /// </summary>
        public static void DownloadFile(string url, string output)
        {
            // Checks if file exists
            var fileExists = File.Exists(output);

            // If file does not exist
            if (!fileExists)
            {
                // Creates the folder
                Directory.CreateDirectory("data/");

                // Downloads the file
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, output);
                }
            }
        }

        /// <summary>
		/// Extracts a <i>.tar.gz</i> archive to the specified directory.
		/// </summary>
		/// <param name="filename">The <i>.tar.gz</i> to decompress and extract.</param>
		/// <param name="outputDir">Output directory to write the files.</param>
		public static void ExtractTarGz(string filename, string outputDir)
        {
            using (var stream = File.OpenRead(filename))
                ExtractTarGz(stream, outputDir);
        }

        /// <summary>
        /// Extracts a <i>.tar.gz</i> archive stream to the specified directory.
        /// </summary>
        /// <param name="stream">The <i>.tar.gz</i> to decompress and extract.</param>
        /// <param name="outputDir">Output directory to write the files.</param>
        public static void ExtractTarGz(Stream stream, string outputDir)
        {
            // A GZipStream is not seekable, so copy it first to a MemoryStream
            using (var gzip = new GZipStream(stream, CompressionMode.Decompress))
            {
                const int chunk = 4096;
                using (var memStr = new MemoryStream())
                {
                    int read;
                    var buffer = new byte[chunk];
                    do
                    {
                        read = gzip.Read(buffer, 0, chunk);
                        memStr.Write(buffer, 0, read);
                    } while (read == chunk);

                    memStr.Seek(0, SeekOrigin.Begin);
                    ExtractTar(memStr, outputDir);
                }
            }
        }

        /// <summary>
        /// Extractes a <c>tar</c> archive to the specified directory.
        /// </summary>
        /// <param name="filename">The <i>.tar</i> to extract.</param>
        /// <param name="outputDir">Output directory to write the files.</param>
        public static void ExtractTar(string filename, string outputDir)
        {
            using (var stream = File.OpenRead(filename))
                ExtractTar(stream, outputDir);
        }

        /// <summary>
        /// Extractes a <c>tar</c> archive to the specified directory.
        /// </summary>
        /// <param name="stream">The <i>.tar</i> to extract.</param>
        /// <param name="outputDir">Output directory to write the files.</param>
        public static void ExtractTar(Stream stream, string outputDir)
        {
            var buffer = new byte[100];
            while (true)
            {
                stream.Read(buffer, 0, 100);
                var name = Encoding.ASCII.GetString(buffer).Trim('\0');
                if (String.IsNullOrWhiteSpace(name))
                    break;
                stream.Seek(24, SeekOrigin.Current);
                stream.Read(buffer, 0, 12);
                var size = Convert.ToInt64(Encoding.UTF8.GetString(buffer, 0, 12).Trim('\0').Trim(), 8);

                stream.Seek(376L, SeekOrigin.Current);

                var output = Path.Combine(outputDir, name);
                if (!Directory.Exists(Path.GetDirectoryName(output)))
                    Directory.CreateDirectory(Path.GetDirectoryName(output));
                if (!name.Equals("./", StringComparison.InvariantCulture))
                {
                    using (var str = File.Open(output, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        var buf = new byte[size];
                        stream.Read(buf, 0, buf.Length);
                        str.Write(buf, 0, buf.Length);
                    }
                }

                var pos = stream.Position;

                var offset = 512 - (pos % 512);
                if (offset == 512)
                    offset = 0;

                stream.Seek(offset, SeekOrigin.Current);
            }
        }

        /// <summary>
        /// Loads auxiliary data for CEC-based benchmarking functions.
        /// <param name="name">Name of function to be loaded.</param>
        /// <param name="year">Year of function to be loaded.</param>
        /// <returns>An array holding the auxiliary data.</returns>
        /// </summary>
        public static double[] LoadCECAuxiliary(string name, string year)
        {

            // Defines some common-use variables
            var baseURL = "http://recogna.tech/files/opytimark/";
            var tarName = $"{year}.tar.gz";
            var tarPath = $"data/{tarName}";

            // Downloads the file
            DownloadFile(baseURL + tarName, tarPath);

            // De-compresses the file
            ExtractTarGz(tarPath, $"data/{year}");

            // Loads the auxiliary data
            var data = File.ReadAllText($"data/{year}/{name}.txt")
                .Split(new[] { " ", "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => double.Parse(s, CultureInfo.GetCultureInfo("en-US"))).ToArray();

            return data;
        }

        public static T LoadCECAuxiliary<T>(string name, string year)
        {
            // Defines some common-use variables
            var baseURL = "http://recogna.tech/files/opytimark/";
            var tarName = $"{year}.tar.gz";
            var tarPath = $"data/{tarName}";

            // Downloads the file
            DownloadFile(baseURL + tarName, tarPath);

            // De-compresses the file
            ExtractTarGz(tarPath, $"data/{year}");

            var data = File.ReadLines($"data/{year}/{name}.txt");
            
            if (data.Count() == 1)
            {
                var result = data.FirstOrDefault()
                                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(value => double.Parse(value, CultureInfo.GetCultureInfo("en-US")))
                                    .ToArray();
                
                return (T) Convert.ChangeType(result, typeof(T));
            }
            else
            {
                var result = data
                    .Select(line => line
                                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(value => double.Parse(value, CultureInfo.GetCultureInfo("en-US")))
                                .ToArray())
                    .ToArray();

                return (T) Convert.ChangeType(result, typeof(T));
            }
        }
    }
}