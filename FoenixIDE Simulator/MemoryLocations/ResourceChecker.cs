﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoenixIDE.Simulator.MemoryLocations
{
    public class ResourceChecker
    {
        public class Resource
        {
            public int StartAddress = 0;
            public int Length = 0;
            public string Name;
            public string SourceFile;
        }

        List<Resource> resources = new List<Resource>();
        public bool Add(Resource resource)
        {
            // Check if there is an overlap
            foreach (Resource res in resources)
            {
                int beginRange = res.StartAddress;
                int endRange = res.StartAddress + res.Length;

                if (resource.StartAddress > beginRange && resource.StartAddress < endRange ||
                    (resource.StartAddress+ resource.Length) > beginRange && (resource.StartAddress + resource.Length) < endRange)
                {
                    if (MessageBox.Show(String.Format("This image overlap resource {0} which starts at {1:X6} and ends at {2:X6}.\r\nDo you want to load it anyway?", res.Name, res.StartAddress, res.StartAddress + res.Length),
                        "Overlap Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return false;
                    }
                }
            }
            resources.Add(resource);
            return true;
        }
        public void Clear()
        {
            resources.Clear();
        }
        public List<Resource> Items
        {
            get
            {
                return resources;
            }
        }
    }
}
