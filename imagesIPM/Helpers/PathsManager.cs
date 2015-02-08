using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imagesIPM.Helpers
{
    public class PathsManager
    {
        private int PATHS_TO_REMEMBER = 10;
        private List<string> paths = new List<string>();

        public List<string> Paths
        {
            get { return paths; }
            set { paths = value; }
        }

        public void PushPath(string path)
        {
            if (paths.Count > PATHS_TO_REMEMBER-1)
            {
                paths.RemoveAt(0);
            }
            paths.Add(path);
        }

        public List<string> list()
        {
            return paths;
            
        }
    }
}
