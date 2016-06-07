const exec = require('child_process').exec;
const execFile = require('child_process').execFile;
const fs = require('fs');

function autoBuild(solutionName, contractsProjectName) {
    exec(`msbuild ${solutionName}\\${solutionName}.sln`, (error, stdout, stderr) => {
        if (error) {
            console.error (`exec error: ${error}`);
            return;
        }
        
        fs.rename('.\\${solutionName}\\artifacts\\bin\\${contractsProjectName}\\Debug\\*.nupkg', 'C:\\lib\\*.nupkg', error => {
            if (error) {
                console.error (`Error while moving nupkg: ${error}`);
            } 
        });
    });
}

autoBuild('OnPremiseMessageHandler1', 'Contracts');
autoBuild('OnPremiseMessageHandler2', 'OnPremiseMessageHandler2.Contracts');