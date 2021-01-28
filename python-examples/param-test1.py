import clr
import sys
import json
import math
import os

script_path  = __file__ 
script_dir = os.path.dirname(__file__)

# the name of idea connection project which is used in this script
ideaCon_filename = r"parameters-anchors.ideaCon"

# the path to the idea connection installation directory
idea_path = r"C:\Program Files\IDEA StatiCa\StatiCa 21.0"

assembly_path = os.path.join(script_dir, 'bin') 
connection_project_path = os.path.join(script_dir, 'projects', ideaCon_filename) 

# modify path to be able to load .net assemblies
sys.path.append(assembly_path)

# load the assembly IdeaStatiCa.Plugin which is responsible for communication with IdeaStatiCa 
clr.AddReference('IdeaStatiCa.Plugin')
from IdeaStatiCa.Plugin import ConnHiddenClientFactory

# create the instance of the client which communicates with IdeaStatiCa
factory = ConnHiddenClientFactory(idea_path)
ideaConnectionClient = factory.Create()

# open idea connection project 
ideaConnectionClient.OpenProject(connection_project_path)

# get information about connections in the project and print them
projectInfo = ideaConnectionClient.GetProjectInfo()
connections = projectInfo.Connections

for conn in connections:
    print(f'{conn.Name} { conn.Identifier}')

firstCon = connections[0]

# get parameters from the idea connection project
params_json_string = ideaConnectionClient.GetParametersJSON(firstCon.Identifier)
connectionParams = json.loads(params_json_string)

# get loading from the idea connection project
loading_json_string = ideaConnectionClient.GetConnectionLoadingJSON(firstCon.Identifier)
connectionLoading = json.loads(loading_json_string)

# variables related to the thickness of the baseplate
pltThickStep = 0.005
pltThick = 0.005 - pltThickStep

j = 0
# loop for modification of the base plate thickness
while j < 5:
    i = 1
    pltThick += pltThickStep
    j += 1   

    # parameters related to the length of anchors
    anchorLength = 0.100
    anchorLengthStep = 0.050

    # loop for modification of the length of anchors
    while i < 3:
        print(f'anchor length = {anchorLength} plate thickness = {pltThick} ')

        # modify the thickness of the base plate
        pltThickParam = connectionParams[0]
        pltThickParam['value'] = pltThick

        # modify the length of the anchor
        anchorLengthParam = connectionParams[1]
        anchorLengthParam['value'] = anchorLength

        # update the model of the connection
        updated_params_json_string = json.dumps(connectionParams)
        ideaConnectionClient.ApplyParameters(firstCon.Identifier, updated_params_json_string)

        # calculate the modified connection and get brief results
        briefResults = ideaConnectionClient.Calculate(firstCon.Identifier)
        summary_res_weld = briefResults.ConnectionCheckRes[0].CheckResSummary[2]
        print(f'Anchor check value = {summary_res_weld.CheckValue}')

        # get the results of the check of the first connection
        checkResults_json_string = ideaConnectionClient.GetCheckResultsJSON(firstCon.Identifier)
        checkResults = json.loads(checkResults_json_string)

        # results of anchors
        boltsAnchor = checkResults['boltsAnchor']

        # print forces in all anchors
        anchorInx = 1
        for key in boltsAnchor:
            anchor = boltsAnchor[key]
            forceInAnchor = anchor['boltTensionForce']
            print(f'Tension force in bolt {anchorInx} = {forceInAnchor}')
            anchorInx += 1       

        anchorLength += anchorLengthStep
        i += 1

# close idea connection project
ideaConnectionClient.Close()