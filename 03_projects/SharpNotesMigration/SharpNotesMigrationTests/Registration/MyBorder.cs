﻿using SharpContainerProg.AAPublic;

namespace SharpNotesMigrationTests.Registration;

internal static class MyBorder
{
    public static bool IsRegistered;
    public static Registration Registration = new Registration();
    public static IContainer Container => Registration.Start(ref IsRegistered);
}