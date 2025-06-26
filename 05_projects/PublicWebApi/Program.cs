using SharpConfigProg.AAPublic;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

IPreparer defaultPreparer = OutBorder01.DefaultPreparer();
defaultPreparer.Prepare();
defaultPreparer.AppFasade.Run();
