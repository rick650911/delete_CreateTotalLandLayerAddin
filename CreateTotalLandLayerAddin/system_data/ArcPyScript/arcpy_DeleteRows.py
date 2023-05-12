# -*- coding: utf-8 -*
import sys
import arcpy

try:
    arcpy.SetSeverityLevel(1)
    in_rows = sys.argv[1]
    
    arcpy.DeleteRows_management(in_rows)
except arcpy.ExecuteWarning:
    #不處理,因為arcpy會自己列印出警告訊息
    #(應該是arcpy bug,無解)
    pass    
except arcpy.ExecuteError:
    #需主動列印出錯誤訊息
    e = sys.exc_info()[1]
    print(e.args[0])


