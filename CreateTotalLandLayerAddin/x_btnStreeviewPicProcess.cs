using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityClassLibrary;

namespace CreateTotalLandLayerAddin
{
    internal class x_btnStreeviewPicProcess : Button
    {
        protected override async void OnClick()
        {
           
            await QueuedTask.Run(async () =>
            {
              



            }/*, RasterlizeManager._CancelableProgressorSource.Progressor*/);
        }
    }
}
