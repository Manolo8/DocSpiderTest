using System.IO;
using Microsoft.AspNetCore.Hosting;
using Shared;

namespace DocSpiderTest {
    public class Configuration : IConfiguration {
        public string WebRootPath { get; set; }


        public Configuration(IWebHostEnvironment env) {
            WebRootPath = env.WebRootPath;
        }
    }
}