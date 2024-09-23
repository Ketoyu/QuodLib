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
        /// Rerouts via <code>$"{MyDocuments}\{<paramref name="publisher"/>}\reroute.ini"</code> and sets the <see cref="Directory"/> to <code>$"{routedDirectory}\{<paramref name="programName"/>}"</code>
        /// </summary>
        /// <param name="publisher"></param>
        /// <param name="programName"></param>
        /// <remarks>
        ///     Replaces invalid characters with underscores.
        ///     <list type="bullet">
        ///         <item>See also <see cref="Environment.SpecialFolder.MyDocuments"/></item>
        ///         <item>See also <see cref="Environment.GetFolderPath(Environment.SpecialFolder)"/></item>
        ///         <item>See also <see cref="IOPath.GetInvalidPathChars"/></item>
        ///     </list>
        /// </remarks>
        protected RoutableConfig(string publisher, string programName) : this(MyDocuments(publisher)) {
            //At this point Directory is "...\Documents\{publisher}\reroute.ini"

            Directory = IOPath.Combine(
                Directory, 
                string.Join('_', programName.Split(IOPath.GetInvalidPathChars()))
            );
        }

        /// <summary>
        /// Rerouts via <code>$"{MyDocuments}\{<paramref name="folderName"/>}\reroute.ini"</code>
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Replaces invalid characters with underscores.
        ///     <list type="bullet">
        ///         <item>See also <see cref="Environment.SpecialFolder.MyDocuments"/></item>
        ///         <item>See also <see cref="Environment.GetFolderPath(Environment.SpecialFolder)"/></item>
        ///         <item>See also <see cref="IOPath.GetInvalidPathChars"/></item>
        ///     </list>
        /// </remarks>
        public static RoutableConfig FromMyDocuments(string folderName)
            => new(MyDocuments(folderName));

        /// <summary>
        /// Rerouts via <code>$"{MyDocuments}\{<paramref name="publisher"/>}\reroute.ini"</code> and sets the <see cref="Directory"/> to <code>$"{routedDirectory}\{<paramref name="programName"/>}"</code>
        /// </summary>
        /// <param name="publisher"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Replaces invalid characters with underscores.
        ///     <list type="bullet">
        ///         <item>See also <see cref="Environment.SpecialFolder.MyDocuments"/></item>
        ///         <item>See also <see cref="Environment.GetFolderPath(Environment.SpecialFolder)"/></item>
        ///         <item>See also <see cref="IOPath.GetInvalidPathChars"/></item>
        ///     </list>
        /// </remarks>
        public static RoutableConfig FromMyDocuments(string publisher, string programName)
            => new(publisher, programName);

        /// <summary>
        /// Returns <code>$"{MyDocuments}\{<paramref name="folderName"/>}"</code>
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Replaces invalid characters with underscores.
        ///     <list type="bullet">
        ///         <item>See also <see cref="Environment.SpecialFolder.MyDocuments"/></item>
        ///         <item>See also <see cref="Environment.GetFolderPath(Environment.SpecialFolder)"/></item>
        ///         <item>See also <see cref="IOPath.GetInvalidPathChars"/></item>
        ///     </list>
        /// </remarks>
        protected static string MyDocuments(string folderName)
            => IOPath.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    string.Join('_', folderName.Split(IOPath.GetInvalidPathChars()))
                );
    }
}
