﻿using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpOperationsProg.Service;

namespace SharpOperationsProg.AAPublic;

public class OutBorder
{
    public static IOperationsService OperationsService(
        IFileService fileService)
    {
        return new OperationService(fileService);
    }
}