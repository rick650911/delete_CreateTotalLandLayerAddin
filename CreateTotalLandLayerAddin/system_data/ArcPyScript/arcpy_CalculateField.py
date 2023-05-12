# -*- coding: utf-8 -*

import sys
import arcpy

inFeatures = sys.argv[1]
field = sys.argv[2]
expression = sys.argv[3]
expression_type = sys.argv[4]

arcpy.CalculateField_management(inFeatures, field, expression, expression_type)

