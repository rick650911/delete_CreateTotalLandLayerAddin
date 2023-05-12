using ArcGIS.Desktop.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateTotalLandLayerAddin
{
    

    class EditBox_Versions : ArcGIS.Desktop.Framework.Contracts.EditBox
    {
        public EditBox_Versions()
        {
            Module1.Current.EditBox_Versions1 = this;
            Text = EnvSettings._srVersions;

            this.PropertyChanged += EditBox_Versions_PropertyChanged;
        }

        /// <summary>
        /// 最小值只能輸0.05
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditBox_Versions_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            EditBox txtBox = sender as EditBox;

            EnvSettings._srVersions = txtBox.Text;

            try
            {
                //string strText = txtBox.Text;// + e.Text;

                //double min = 0.05;

                //double number = double.Parse(strText);

                //if (number < min)
                //{
                //    txtBox.Text = "0.05";
                //}

                
            }
            catch (Exception ex)
            {
                //txtBox.Text = "0.05";
            }

        }

    }
}
