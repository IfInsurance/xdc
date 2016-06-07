const exec = require('child_process').exec;
const execFile = require('child_process').execFile;
const fs = require('fs');

function autoBuild(solutionName, contractsProjectName) {
    exec(`msbuild ${solutionName}\\${solutionName}.sln`, (error, stdout, stderr) => {
        if (error) {
            console.error (`exec error: ${error}`);
            console.error (stdout);
            console.error (stderr);
            return;
        }
        
        exec(`move .\\${solutionName}\\artifacts\\bin\\${contractsProjectName}\\Debug\\*.nupkg C:\\lib`, error => {
            if (error) {
                console.error (`Error while moving nupkg: ${error}`);
            } 
        });
    });
}

autoBuild('OnPremiseService1', 'Public');
autoBuild('OnPremiseService2', 'Public');