﻿namespace SharpRepoBackendProg.Repetition;

internal partial class ConfigServiceRegistration : AdditionalRegistrationBase
{
    public void Register()
    {
        //MyBorder.Registration.RegisterByFunc<IConfigService, IFileService>(
        //    OutBorder2.ConfigService,
        //    MyBorder.Container.Resolve<IFileService>());
        //var configService = MyBorder.Container.Resolve<IConfigService>();
        //configService.Prepare();
        //configService.Prepare(typeof(IConfigService.ILocalProgramDataPreparer));
    }
}