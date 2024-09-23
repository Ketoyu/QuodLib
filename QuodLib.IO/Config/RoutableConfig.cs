using System.IO;
using IODirectory = System.IO.Directory;
using IOFile = System.IO.File;
using IOPath = System.IO.Path;

namespace QuodLib.IO.Config {
    /// <summary>
    /// Controls a file "reroute.ini" for giving users the option to specify where a program stores its settings; 
    /// outputs the effective directory to <see cref="Directory"/>. Creates the file if it does not exist.
    /// </summary>
    public class RoutableConfig {
        /// <summary>
        /// The effective directory, after routing has been processed.
        /// </summary>
        public string Directory { get; protected init; }

        /// <summary>
        /// Rerouts via <code>$"{<paramref name="directory"/>}\reroute.ini"</code>
        /// </summary>
        /// <param name="directory"></param>
        /// <remarks>
        ///     Replaces invalid characters with underscores.<br />
        ///     See also <see cref="IOPath.GetInvalidPathChars"/>
        /// </remarks>
        public RoutableConfig(string directory) {
            string file = IOPath.Combine(directory, @"reroute.ini");

            bool exists = IOFile.Exists(file);

            if (exists && Ini.Load(file).TryGetValue("Path", out string? dir_) && !string.IsNullOrEmpty(dir_)) {
                IODirectory.CreateDirectory(dir_);
                Directory = dir_;
                return;
            }

            if (!exists)
                Ini.Save(
                    new() { { "Path", directory } },
                    file
                );

            Directory = directory;
        }

        /// <summary>
        /// Rerouts via <code>$"{<paramref name="directory"/>}\reroute.ini"</code> and sets the <see cref="Directory"/> to <code>$"{routedDirectory}\{<paramref name="subPath"/>}"</code>
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="subPath"></param>
        protected RoutableConfig(string directory, string subPath) : this(directory) {
            Directory = IOPath.Combine(Directory, subPath);
        }
    }
}
