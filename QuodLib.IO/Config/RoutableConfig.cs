using IODirectory = System.IO.Directory;
using IOFile = System.IO.File;
using IOPath = System.IO.Path;

namespace QuodLib.IO.Config {
    /// <summary>
    /// Controls a file "reroute.ini" for giving users the option to specify where a program stores its settings; 
    /// outputs the effective directory to <see cref="Directory"/>. Creates the file if it does not exist.
    /// </summary>
    public class RoutableConfig {
        protected string Directory { get; }

        public RoutableConfig(string publisher, string programName) {
            string path = IOPath.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    string.Join('_', publisher.Split(IOPath.GetInvalidPathChars()))
                );

            string file = IOPath.Combine(path, @"reroute.ini");

            if (!IOFile.Exists(file)) {
                Ini.Save(new Dictionary<string, string>() {
                        {
                            "Path",
                            path
                        }
                    }, file);
                Directory = IOPath.Combine(path, programName);
                return;
            }

            if (Ini.Load(path).TryGetValue("Path", out string? path_) && !string.IsNullOrEmpty(path_)) {
                IODirectory.CreateDirectory(path_);
                Directory = IOPath.Combine(path_, programName);
                return;
            }

            Directory = IOPath.Combine(path, programName);
        }
    }
}
