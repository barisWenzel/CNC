using Grasshopper.Kernel;
using System;
using Rhino.Geometry;
using System.Collections.Generic;

namespace CNC
{
    public class CreateCurvePts : GH_Component
    {
    

        public CreateCurvePts() : base("Outline", "Outline", "Curve to pts", "CNC", "Curve") { }

       

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Crv", "Crv", "Outline", GH_ParamAccess.item);//probably list!
            pManager.AddNumberParameter("SegmentSize", "SegmentSize", "SegmentSize", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("ToolRadius", "ToolRadius", "Tool radius for offset correction, use a negative value for inside, positive for outside", GH_ParamAccess.item);
            pManager.AddNumberParameter("Depth of cut", "Depth of cut", "Thickness of material you want to cut through", GH_ParamAccess.item);
            pManager.AddTextParameter("Material", "Material", "Material you want to cut through", GH_ParamAccess.item);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("pts", "pts", "pts", GH_ParamAccess.list);
            //pManager.AddNumberParameter("Debug", "Debug", "Debug", GH_ParamAccess.item); "Debugging"
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            #region retrieving the inputs
            Curve crv = null;
           if(! DA.GetData(0, ref crv))return;

            double segmentSize = 0.0;
            if (!DA.GetData(1, ref segmentSize)) return;

            double toolRadius = 0.0;
            if (!DA.GetData(2, ref toolRadius)) return;

            double depthOfCut = 0.0;
            if (!DA.GetData(3, ref depthOfCut)) return;

            string material= string.Empty;
            if (!DA.GetData(4, ref material)) return;


            //Exceptionhandling? cleaning?

            #endregion

            //Collecitons
            List<Polyline> plines = new List<Polyline>();

           //radiuskorretion
            Curve[] offset = crv.Offset(Plane.WorldXY, toolRadius, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, CurveOffsetCornerStyle.Sharp);

            if (offset.Length>1)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Something with the offset went wrong, please try to simplyfy the curve");
            }
            
            //segmentieren
            Polyline pline = offset[0].ToPolyline(Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, Rhino.RhinoDoc.ActiveDoc.ModelAngleToleranceRadians, segmentSize, segmentSize).ToPolyline();
            
            
            int zustellungen = (int) (depthOfCut / UTIL.getZustellungenByMaterial(material, toolRadius));
            double zustelltiefe =  depthOfCut/zustellungen;

            for (int i = 0; i < zustellungen+1; i++)
            {
                Polyline pCopy = pline.Duplicate();
                pCopy.Transform(Transform.Translation(0, 0, -zustelltiefe*i));
                plines.Add(pCopy);
            }
            DA.SetDataList(0, plines);        
        }



        public override Guid ComponentGuid => new Guid("1272B3C8-C732-4EBF-8882-5EBA3304D689");


    }
}
