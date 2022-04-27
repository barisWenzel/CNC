using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace CNC
{
    public class CNCInfo : GH_AssemblyInfo
    {
        public override string Name => "CNC";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("F683520E-CA1C-4056-8FF8-D1BCA14A6B73");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}