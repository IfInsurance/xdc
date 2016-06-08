# About
Cross Data-Centre Logic, or *on-premise to the cloud and back again*!

## Local environment
Start `MSBuild Command Prompt` and then invoke `powershell` from there.
You'll want `node` and `nodemon` installed.

Start auto builds with `nodemon -e cs -w .\ -C .\build.js`

## Todo
### On Premise Service 1 - Environment information
1. ~~Create class library project `OnPremiseService1.Public` in solution `OnPremiseService1`~~
2. ~~Rename project to `Public`~~
3. ~~Add `SetEnvironmentVariable` command~~
4. ~~Add `EnvironmentVariableChanged` event~~
5. ~~Publish as NuGet~~
6. ~~Create console app project `OnPremiseService1.EnvironmentMessageHandler`~~
7. ~~Rename project to `EnvironmentMessageHandler`~~
8. ~~Reference NuGet~~

### On Premise Service 2 - Calculator
1. ~~Create class library project `OnPremiseService2.Public` in solution `OnPremiseService2`~~
2. ~~Rename project to `Public`~~
3. ~~Add `MutateNumber` command~~
4. ~~Add `ResultChanged` event~~
5. ~~Publish as NuGet~~
6. ~~Create console app project `OnPremiseService2.MathMessageHandler`~~
7. ~~Rename project to `MathMessageHandler`~~
8. ~~Reference NuGet~~

### Cloud Service 1 - Echo Service
1. Create a class library project `CloudService1.Public` in solution `CloudService1` 
2. Rename project to `Public`
3. Add `PleaseRepeatThis` command
4. Add `EchoedResponse` event
5. Publish as NuGet
6. Create a Azure Web Job `CloudService1.EchoMessageHandler`
7. Rename project to `EchoMessageHandler`
8. Reference NuGet 

### On-premises bridge
1. Reference OP1 and CS1
2. Double-Bus (Nimbus, NSb)