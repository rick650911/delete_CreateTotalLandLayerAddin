# -*- coding: UTF-8 -*-
import sys
import arcpy

try:
    in_picfolder = sys.argv[1]
    out_feature_class = sys.argv[2]

    arcpy.SetSeverityLevel(1)
    result = arcpy.GeoTaggedPhotosToPoints_management(in_picfolder,out_feature_class,"","","NO_ATTACHMENTS")
    print(result[0])
except arcpy.ExecuteWarning:
    #不處理,因為arcpy會自己列印出警告訊息
    #(應該是arcpy bug,無解)
    pass    
except arcpy.ExecuteError:
    #需主動列印出錯誤訊息
    e = sys.exc_info()[1]
    print(e.args[0])
