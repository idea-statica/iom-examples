import clr
import sys
import json
import math

script_path  = __file__ 
print (script_path)

assembly_path = r"C:\git\iom-examples\python-examples\bin"
idea_path = r"C:\git\is-1\bin\Release"
connection_project_path = r"C:\git\iom-examples\python-examples\projects\parameters-anchors.ideaCon"

sys.path.append(assembly_path)

clr.AddReference('IdeaStatiCa.Plugin')
from IdeaStatiCa.Plugin import ConnHiddenClientFactory

factory = ConnHiddenClientFactory(idea_path)

ideaConnectionClient = factory.Create()

ideaConnectionClient.OpenProject(connection_project_path)

projectInfo = ideaConnectionClient.GetProjectInfo()

connections = projectInfo.Connections

for conn in connections:
    print(f'{conn.Name} { conn.Identifier}')

firstCon = connections[0]

params_json_string = ideaConnectionClient.GetParametersJSON(firstCon.Identifier)
connectionParams = json.loads(params_json_string)

loading_json_string = ideaConnectionClient.GetConnectionLoadingJSON(firstCon.Identifier)
connectionLoading = json.loads(loading_json_string)

pltThickStep = 0.0005
pltThick = 0.005 - pltThickStep

j = 0
while j < 5:
    i = 1
    pltThick += pltThickStep
    j += 1   
    anchorLength = 0.100
    anchorLengthStep = 0.050
    while i < 3:
        print(f'anchor length = {anchorLength} plate thickness = {pltThick} ')

        pltThickParam = connectionParams[0]
        pltThickParam['value'] = pltThick

        anchorLengthParam = connectionParams[1]
        anchorLengthParam['value'] = anchorLength

        updated_params_json_string = json.dumps(connectionParams)
        ideaConnectionClient.ApplyParameters(firstCon.Identifier, updated_params_json_string)
        briefResults = ideaConnectionClient.Calculate(firstCon.Identifier)
        summary_res_weld = briefResults.ConnectionCheckRes[0].CheckResSummary[2]
        print(f'Anchor check value = {summary_res_weld.CheckValue}')

        checkResults_json_string = ideaConnectionClient.GetCheckResultsJSON(firstCon.Identifier)
        checkResults = json.loads(checkResults_json_string)

        boltsAnchor = checkResults['boltsAnchor']

        anchorInx = 1
        for key in boltsAnchor:
            anchor = boltsAnchor[key]
            forceInAnchor = anchor['boltTensionForce']
            print(f'Tension force in bolt {anchorInx} = {forceInAnchor}')
            anchorInx += 1       

        anchorLength += anchorLengthStep
        i += 1


ideaConnectionClient.Close()
