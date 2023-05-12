# -*- coding: utf-8 -*
import sys
import arcpy

try:   
    # Set local variables
    inFeatures = sys.argv[1]
    outLocation = sys.argv[2]
    outFeatureClass = sys.argv[3]     
    # Execute FeatureClassToFeatureClass
    arcpy.SetSeverityLevel(1)  
    if len(sys.argv)<=4:
        arcpy.FeatureClassToFeatureClass_conversion(inFeatures, outLocation, outFeatureClass)
    #帶有expression
    elif len(sys.argv)==5:
        expression=sys.argv[4]
        arcpy.FeatureClassToFeatureClass_conversion(inFeatures, outLocation, outFeatureClass,expression)
    #帶有輸出指定field
    else:
        expression=sys.argv[4]
        outputfieldarray=sys.argv[5].split(";")
        #加入input所有欄位
        fieldMappings = arcpy.FieldMappings()
        fieldMappings.addTable(inFeatures)
        #移除非指定欄位    
        for field in fieldMappings.fields:
            if field.name not in outputfieldarray:
                fieldMappings.removeFieldMap(fieldMappings.findFieldMapIndex(field.name))

        #檢測是否至少有一個符合的輸出欄位
        #沒有則回傳錯誤訊息
        if fieldMappings.fieldCount>0:
            arcpy.FeatureClassToFeatureClass_conversion(inFeatures, outLocation, outFeatureClass,expression,fieldMappings)
        else:
            print("ERROR : input檔案中不包含指定輸出欄位,無法執行處理")
              
except arcpy.ExecuteWarning:
    #不處理,因為arcpy會自己列印出警告訊息
    #(應該是arcpy bug,無解)
    pass    
except arcpy.ExecuteError:
    #需主動列印出錯誤訊息
    e = sys.exc_info()[1]
    print(e.args[0])
