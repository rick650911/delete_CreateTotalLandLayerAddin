using ArcGIS.Desktop.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoaGeoProcessorAddin
{
    class EditBox_BufferSearchTolerance : ArcGIS.Desktop.Framework.Contracts.EditBox
    {
        public EditBox_BufferSearchTolerance()
        {
            Module1.Current.EditBox_BufferSearchTolerance1 = this;
            Text = LandUpdateManager._strBufferSearchTolerance; ;// "";

            this.PropertyChanged += EditBox_BufferSearchTolerance_PropertyChanged;
        }

        private void EditBox_BufferSearchTolerance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            EditBox txtBox = sender as EditBox;

            try
            {
                double dblBufferSearchTolerance;

                bool blParseOk = double.TryParse(txtBox.Text,out dblBufferSearchTolerance);

                if (blParseOk)
                {
                    LandUpdateManager._strBufferSearchTolerance = txtBox.Text;
                    LandUpdateManager._dblBufferSearchTolerance = dblBufferSearchTolerance;

                    

                }
                
            }
            catch (Exception ex)
            {
                txtBox.Text = LandUpdateManager._strBufferSearchTolerance;// "0.01";
                //LandUpdateManager._strBufferSearchTolerance = "0.01";
                //LandUpdateManager._dblBufferSearchTolerance = 0.01;
            }

        }

    }
}
