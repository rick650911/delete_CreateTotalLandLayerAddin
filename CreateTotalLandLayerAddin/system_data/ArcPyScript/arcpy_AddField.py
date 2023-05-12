# -*- coding: utf-8 -*
import sys
import arcpy

try:
    arcpy.SetSeverityLevel(1)
    in_table = sys.argv[1]
    field_name = sys.argv[2]
    fieldtype = sys.argv[3]
    field_length_or_precision=int(sys.argv[4])
    if (fieldtype == "TEXT"):
        arcpy.AddField_management(in_table, field_name, fieldtype, field_length=field_length_or_precision)
    else:
        arcpy.AddField_management(in_table, field_name, fieldtype, field_precision=field_length_or_precision)
except arcpy.ExecuteWarning:
    #不處理,因為arcpy會自己列印出警告訊息
    #(應該是arcpy bug,無解)
    pass    
except arcpy.ExecuteError:
    #需主動列印出錯誤訊息
    e = sys.exc_info()[1]
    print(e.args[0])


