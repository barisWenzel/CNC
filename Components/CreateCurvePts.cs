using Grasshopper.Kernel;
using System;
using Rhino.Geometry;

namespace CNC
{
    class CreateCurvePts : GH_Component
    {
    

        public CreateCurvePts() : base("CNC", "CNC", "CNC", "CNC", "CNC") { }

       

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Crv", "Crv", "Crv", GH_ParamAccess.item);//probably list!
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            throw new NotImplementedException();
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Curve crv = null;
           if(! DA.GetData(0, ref crv))return;

           //Exceptionhandling? cleaning?








        }



        public override Guid ComponentGuid => new Guid("29CA6E02 - E5A7 - 4358 - 9347 - D53B19F415D2");


    }
}
