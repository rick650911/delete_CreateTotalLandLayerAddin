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
using System.Text;
using System.Threading.Tasks;

namespace CreateTotalLandLayerAddin
{
    public class GpTools
    {
        public static async void MakeFeatureLayer(object in_features,
                                                object out_layer,
                                                string where_clause,//AFFINE、PROJECTIVE、SIMILARITY 
                                                List<IGPResult> aList_output_GpResult,
                                                List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(in_features, out_layer, where_clause);

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("MakeFeatureLayer_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);
            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 MakeFeatureLayer_management 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void CreateFishnet(string aStrOutFeatureClass,
                                              string aStrCellSize,
                                              string aStrTemplateExtent,
                                              List<IGPResult> aList_output_GpResult,
                                              List<string> aList_output_ErrorMsg)
        {
            try 
            {
                // Place parameters into an array
                //string outFeatureClass = System.IO.Path.Combine(FLPathCombine, "fn_" + featLayer.Name);
                string originCoordinate = "150970.000000278 2422129.99999811";//左下(地政司dtm)

                //string originCoordinate = "147522.2188 2406567.9796";//左下
                string yAxisCoordinate = "150970.000000278 2422129.99999811";// "147522.2188 2406567.9796";
                string cellSizeWidth = aStrCellSize;// "1000";
                string cellSizeHeight = aStrCellSize;// "1000";
                string numRows = "0";
                string numColumns = "0";
                string oppositeCoorner = "150970.000000278 2422129.99999811";// "147522.2188 2406567.9796";//disabled if the origin, Y-axis, cell size, and number of rows and columns are set.
                string labels = "NO_LABELS";
                //string templateExtent = "147522.2188 2406567.9796 351690.1143 2799148.9622";
                string geometryType = "POLYGON";

                var parameters = Geoprocessing.MakeValueArray(aStrOutFeatureClass,
                                                            originCoordinate,
                                                            yAxisCoordinate,
                                                            cellSizeWidth,
                                                            cellSizeHeight,
                                                            numRows,
                                                            numColumns,
                                                            oppositeCoorner,
                                                            labels,
                                                            aStrTemplateExtent,
                                                            geometryType);

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                // var gpResult = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", parameters, environments);
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("CreateFishnet_management", parameters, environments);
                
                aList_output_GpResult.Add(gpResult);
            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 CreateFishnet_management 發生錯誤："+exc.Message);
                return;
            }
          

        }


        public static async void Dissolve(object aStrFeatureClass_input,
                                            string aStrFeatureClass_output,
                                            List<string> aStrDis_Fields,
                                            List<string> aStrSta_Fields,
                                            string aStrMULTI_PART,
                                            List<IGPResult> aList_output_GpResult,
                                            List<string> aList_output_ErrorMsg)
        {
            try
            {
                var parameters = Geoprocessing.MakeValueArray(aStrFeatureClass_input,
                                                              aStrFeatureClass_output,
                                                              aStrDis_Fields,//      ["LANDUSE", "TAXCODE"],
                                                              aStrSta_Fields,
                                                              aStrMULTI_PART);//,//"SINGLE_PART", "MULTI_PART "
                                                              //"DISSOLVE_LINES");

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                // var gpResult = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", parameters, environments);
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("PairwiseDissolve_analysis", parameters, environments);
                //IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("Pairwise_Dissolve_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);

                
            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 Dissolve_management 發生錯誤：" + exc.Message);
                return;
            }


        }

        public static async void Intersect(List<string> aStrFeatureClass_inputs,
                                           string aStrFeatureClass_output,
                                           List<IGPResult> aList_output_GpResult,
                                           List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(aStrFeatureClass_inputs, aStrFeatureClass_output, "ALL", "", "");

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                // var gpResult = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", parameters, environments);
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("PairwiseIntersect_analysis", parameters, environments);

                aList_output_GpResult.Add(gpResult);


            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 Intersect_analysis 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void AddJoin(object layerName,string inputField, object joinTable,string joinField,
                                           List<IGPResult> aList_output_GpResult,
                                           List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(layerName, inputField, joinTable, joinField);

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                // var gpResult = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", parameters, environments);
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("AddJoin_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);
            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 AddJoin_management 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void RemoveJoin(object layerName, object joinTable, 
                                           List<IGPResult> aList_output_GpResult,
                                           List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(layerName, joinTable);

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                // var gpResult = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", parameters, environments);
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("RemoveJoin_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);


            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 AddJoin_management 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void AddField(object layerName, string fieldName, string strType,
                                         string fieldPrecision, string strScale, string strLength,
                                         string fieldAlias, string strNULLABLE,
                                         List<IGPResult> aList_output_GpResult,
                                         List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(layerName,  fieldName,  strType,  fieldPrecision,  strScale,  strLength,
                                         fieldAlias,  strNULLABLE);

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                // var gpResult = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", parameters, environments);
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("AddField_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);


            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 AddField_management 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void AddIndex(object layerName, List<string> aFields,
                                         List<IGPResult> aList_output_GpResult,
                                         List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(layerName, aFields,"my_index");

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                // var gpResult = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", parameters, environments);
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("AddIndex_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);
            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 AddIndex_management 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void CalculateField(object inFeatures,string fieldName,string expression,string strExpType,string codeblick,
                                         List<IGPResult> aList_output_GpResult,
                                         List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(inFeatures, fieldName, expression, strExpType, codeblick);//"Python 3""Arcade"

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                // var gpResult = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", parameters, environments);
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("CalculateField_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);


            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 CalculateField_management 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void Delete(object inData, 
                                         List<IGPResult> aList_output_GpResult,
                                         List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(inData);//"Python 3""Arcade"

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                // var gpResult = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", parameters, environments);
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("Delete_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);


            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 CalculateField_management 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void PolygonToRaster(string inFeatures,string valField,string outRaster,
                                 string assignmentType, string priorityField, string cellSize,
                                        List<IGPResult> aList_output_GpResult,
                                        List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(inFeatures, valField, outRaster,
                                 assignmentType, priorityField, cellSize);

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("PolygonToRaster_conversion", parameters, environments);

                aList_output_GpResult.Add(gpResult);


            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 PolygonToRaster_conversion 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void FeatureClassToFeatureClass(object inFeatures,
            string outLocation, 
            string outName,
                                                            string aStrExpression, 
                                                            List<IGPResult> aList_output_GpResult,
                                                            List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(inFeatures, outLocation, outName,
                                                            aStrExpression);

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("FeatureClassToFeatureClass_conversion", parameters, environments);

                aList_output_GpResult.Add(gpResult);


            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 FeatureClassToFeatureClass_conversion 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void GeoTaggedPhotosToPoints(object Input_Folder,
    string Output_Feature_Class,
 
                                                    List<IGPResult> aList_output_GpResult,
                                                    List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(Input_Folder, Output_Feature_Class, "","ONLY_GEOTAGGED", "NO_ATTACHMENTS");

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("GeoTaggedPhotosToPoints_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);


            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 FeatureClassToFeatureClass_conversion 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void Append(List<object> aStrFeatureClass_inputs,
                                        object outTarget,
                                        List<IGPResult> aList_output_GpResult,
                                        List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(aStrFeatureClass_inputs, outTarget, "NO_TEST");

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("Append_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);
            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 Append_management 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void SubdividePolygon(object inFeatureLayer,
                                                    string outName,
                                                    int intNumOfArea,
                                                    int intSplitAngle,
                                                    List<IGPResult> aList_output_GpResult,
                                                    List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(inFeatureLayer, outName, 
                                                               "NUMBER_OF_EQUAL_PARTS", intNumOfArea, "", "", intSplitAngle,"STRIPS");

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("SubdividePolygon_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);


            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 SubdividePolygon_management 發生錯誤：" + exc.Message);
                return;
            }
        }

        public static async void SelectLayerByLocation(object inFeatureLayer,
                                                    object select_features,
                                                    string overlap_type,
                                                    string search_distance,
                                                    List<IGPResult> aList_output_GpResult,
                                                    List<string> aList_output_ErrorMsg)
        {
            try
            {
                // Place parameters into an array
                var parameters = Geoprocessing.MakeValueArray(inFeatureLayer, overlap_type, select_features, search_distance);

                // Place environment settings in an array, in this case, OK to over-write
                var environments = Geoprocessing.MakeEnvironmentArray(overwriteoutput: true);

                // Execute the GP tool with parameters
                IGPResult gpResult = await Geoprocessing.ExecuteToolAsync("SelectLayerByLocation_management", parameters, environments);

                aList_output_GpResult.Add(gpResult);
            }
            catch (Exception exc)
            {
                // Catch any exception found and display in a message box
                //ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Exception caught while trying to run GP tool: " + exc.Message);
                aList_output_ErrorMsg.Add("執行GP工具 SelectLayerByLocation_management 發生錯誤：" + exc.Message);
                return;
            }
        }

    }
}
