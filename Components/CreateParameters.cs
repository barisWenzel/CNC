using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNC.Components
{
    public class CreateParameters : GH_Component
    {
        public CreateParameters() : base("cp", "cp", "cp", "CNC", "CNC") {  }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Material", "mat", "Material you want to cut"
                + Environment.NewLine + "Materials aviable: Steel,Aluminium,Plastic,Wood",GH_ParamAccess.item);
            pManager.AddNumberParameter("Thickness", "t", "Thickness of material you want to cut", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Teeth", "Tt", "Number of teeth of the endmill", GH_ParamAccess.item);
            pManager.AddNumberParameter("Toolradius", "r", "Radius of endmill", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Schnittgeschwindigkeit", "sg", "Schnittgeschwindigkeit "
               + Environment.NewLine + "Steel = 40-100"
               + Environment.NewLine + "Aluminium = 100-500"
               + Environment.NewLine + "Plastic = 50-150"
               + Environment.NewLine + "Wood = 400-3000"
               + Environment.NewLine + "Attention: Take care of the max. revolutions of your machine!", GH_ParamAccess.item);
            pManager.AddNumberParameter("VorschubProZahn", "vpz", "VorschubProZahn "
               + Environment.NewLine + "Steel = 0.005-0.01"
               + Environment.NewLine + "Aluminium = 0.005-0.025"
               + Environment.NewLine + "Plastic = 0.005-0.025"
               + Environment.NewLine + "Wood = 0.01-0.03"
               + Environment.NewLine +"The softer the material the higher should be this value", GH_ParamAccess.item);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Zustellungen", "zustellungen", "Anzahl Zustellungen", GH_ParamAccess.item);
            pManager.AddNumberParameter("Zustelltiefe", "tiefe", "Zustelltiefe", GH_ParamAccess.item);            
            pManager.AddIntegerParameter("Drehzahl", "drehzahl", "Drehzahl"
                + Environment.NewLine + "Attention: Take care of the max. revolutions of your machine!", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Vorschub", "vorschub", "Vorschub", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string material = string.Empty;
            if(!DA.GetData(0, ref material))return;

            double thickness = 0.0;
            if (!DA.GetData(1, ref thickness)) return;

            int teeth = 0;
            if (!DA.GetData(2, ref teeth)) return;
            if (teeth >= 4) teeth = 4; // sonst ist der vorschub zu schnell

            double durchmesser = 0.0;
            if (!DA.GetData(3, ref durchmesser)) return;

            int schnittgeschwindigkeit = 0;
            if (!DA.GetData(4, ref schnittgeschwindigkeit)) return;

            double vorschubProZahn = 0.0;
            if (!DA.GetData(5, ref vorschubProZahn)) return;


            double zm = thickness / UTIL.getZustellungenByMaterial(material,durchmesser);
            int zustellungen = (int)Math.Ceiling(zm);
            DA.SetData(0, zustellungen);

            double zustelltiefe = thickness / zustellungen;
            DA.SetData(1, zustelltiefe);

            //int[] schnittgeschwindigkeit = UTIL.getSchnittgeschwindigkeit(material);
            //DA.SetData(2, schnittgeschwindigkeit);

            int drehzahl = UTIL.getDrehzahl(schnittgeschwindigkeit, durchmesser);

            if(drehzahl>10000)
            {
                int drehzahlHigh = drehzahl;
                drehzahl = 10000;
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "The calculated Spindlespeed was " + drehzahlHigh.ToString() + "and was lowered to 10000(Machines Maximum)");
            }
            DA.SetData(2, drehzahl);


           // double vorschubProZahn = UTIL.getVorschubProZahn(schnittgeschwindigkeit, teeth, drehzahl);

            double vorschub = UTIL.getVorschub(teeth,drehzahl, vorschubProZahn);//Vorschub pro zahn
            DA.SetData(3, vorschub);

            this.Message = "Create Cutting Parameters";
        }



        public override Guid ComponentGuid => new Guid("FCB002CC-ECBA-4046-AB6E-048791F99EF3");
    }
}
