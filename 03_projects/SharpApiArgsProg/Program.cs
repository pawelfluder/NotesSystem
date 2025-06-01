// See https://aka.ms/new-console-template for more information

using SharpApiArgsProg.Registrations;
using SharpApiArgsProg.Services;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.Workers.APublic.ItemWorkers;

RegistrationBox.Registration = new OutMockRegistration();
var isReg = MyBorder.IsRegistered;
new Registration().Start(false);

ApiArgsService service = new();

string serviceName = nameof(IRepoService);
string workerName = nameof(IItemWorker);
string methodName = nameof(IItemWorker.GetItem);
string param01 = "Notki";
string param02 = "";
string item = service.Resolve([serviceName, workerName, methodName, param01, param02]);
