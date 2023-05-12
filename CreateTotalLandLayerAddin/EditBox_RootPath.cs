using ArcGIS.Desktop.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateTotalLandLayerAddin
{
    public class TextEventArgs : EventArgs
    {
        public string text { get; set; }
    }

    class EditBox_RootPath : ArcGIS.Desktop.Framework.Contracts.EditBox
    {
        public EditBox_RootPath()
        {
            Module1.Current.EditBox_RootPath1 = this;
            Text = EnvSettings._srRootPath;

            this.PropertyChanged += EditBox_RootPath_PropertyChanged;
        }

        /// <summary>
        /// 最小值只能輸0.05
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditBox_RootPath_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            EditBox txtBox = sender as EditBox;

            EnvSettings._srRootPath = txtBox.Text;

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
