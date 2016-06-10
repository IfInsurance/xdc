# About
Cross Data-Centre Logic, or *on-premise to the cloud and back again*!

## Local environment
Start `MSBuild Command Prompt` and then invoke `powershell` from there.
You'll want `node` and `nodemon` installed.

Start auto builds with `nodemon -e cs -w .\ -C .\build.js`

## Todo
### On Premise Service 1 - Environment information, using NServiceBus 4
1. ~~Create a *Class Library (Package)* project `OnPremiseService1.Public` in solution `OnPremiseService1`~~
2. ~~Rename project to `Public`~~
3. ~~Add `SetEnvironmentVariable` command~~
4. ~~Add `EnvironmentVariableChanged` event~~
5. ~~Publish as NuGet~~
6. ~~Create console app project `OnPremiseService1.EnvironmentMessageHandler`~~
7. ~~Rename project to `EnvironmentMessageHandler`~~
8. ~~Reference NuGet~~

### On Premise Service 2 - Calculator, using NServiceBus 4
1. ~~Create a *Class Library (Package)* project `OnPremiseService2.Public` in solution `OnPremiseService2`~~
2. ~~Rename project to `Public`~~
3. ~~Add `MutateNumber` command~~
4. ~~Add `ResultChanged` event~~
5. ~~Publish as NuGet~~
6. ~~Create console app project `OnPremiseService2.MathMessageHandler`~~
7. ~~Rename project to `MathMessageHandler`~~
8. ~~Reference NuGet~~

### Cloud Service 1 - Echo Service, using NServiceBus 6 and Azure ServiceBus Transport
1. ~~Create a *Class Library (Package)* project `CloudService1.Public` in solution `CloudService1`~~ 
2. ~~Rename project to `Public`~~
3. ~~Add `PleaseRepeatThis` command~~
4. ~~Add `EchoedResponse` event~~
5. ~~Publish as NuGet~~
6. ~~Create an Azure Web Job `CloudService1.EchoMessageHandler`~~
7. ~~Rename project to `EchoMessageHandler`~~
8. ~~Reference NuGet~~

### Cloud Service 2 - Color Service, using custom-built interoperation layer
1. ~~Create a *Class Library (Package)* project `CloudService2.Public` in solution `CloudService2`~~
2. ~~Rename project to `Public`~~
3. ~~Add `TranslateColorNameToRgb` Command~~
4. ~~Add `ColorNameToRgbTranslationComplete` Event~~
5. ~~Create an Azure Web Job `CloudService2.ColorMessageHandler`~~
6. ~~Rename project to `ColorMessageHandler`~~
7. ~~Reference NuGet 

### Interop layer
1. ~~Extract the interop layer from Cloud Service 2 into a separate NuGet~~
2. ~~Reference and test~~

### On-premises bridge
#### Introduction
The on-premises bridge is a small "repeater" or "forwarder" that contains no business logic of its own. It
facilitates communicating between an on-premises service and a cloud service by using the on-premises
integration stack (NServiceBus 4) and a custom-built interop-layer that communicates with an Azure Service
Bus namespace (from which an NServiceBus 6 service then can communicate). By being small and without its own
business logic, the bridge can easily participate in on-premises distributed transactions. It then uses the
NServiceBus retry logic together with Azure Service Bus' duplicate message detection to implement once and
only once delivery.

#### Tasks
1. Reference *OnPremiseService1* and *CloudService1*
2. Reference local NServiceBus (4.0.2)
3. Reference the *Interop layer* (which, in turn, references the Azure ServiceBus SDK)




Cannot use current nServicebus because [NServiceBus 5.2.14 Doesn’t Support HTTPS When Using WindowsAzure.ServiceBus 2.8.2](https://www.sslvpn.online/nservicebus-5-2-14-doesnt-support-https-when-using-windowsazure-servicebus-2-8-2/)

Cannot use nservicebus beta 6 because it has missing types

If you run the webjob before the nservicebus one, the web job will create an outgoing queue for its event, rather than a topic, which will cause the nservicebus job to fail


