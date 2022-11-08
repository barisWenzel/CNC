using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNC
{
    public class MakeHoles : GH_Component
    {
        public MakeHoles() : base("Hole", "Hole", "Holes", "CNC", "CNC") { }

    

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Pt", "Pt", "Pt", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_Point> ptTree = new GH_Structure<GH_Point>();

            if (!DA.GetDataTree(0, out ptTree))return;

            List<GH_Point> ptList = ptTree.FlattenData();


        }

        public override Guid ComponentGuid => new Guid("CD8A5EAA-24CB-4C1F-BE42-C2FB0F346EBB");
    }
}
