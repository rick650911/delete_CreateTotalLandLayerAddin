# -*- coding: utf-8 -*

import sys
import arcpy

inFeatures = sys.argv[1]
dstFeature = sys.argv[2]
schema_type = sys.argv[3]

arcpy.Append_management(inFeatures,dstFeature, schema_type)
