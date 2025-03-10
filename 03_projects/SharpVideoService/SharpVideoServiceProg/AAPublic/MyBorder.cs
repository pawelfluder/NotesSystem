﻿using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpVideoServiceProg.Service;

namespace SharpVideoServiceProg.AAPublic;

public static class OutBorder
{
    public static IVideoService VideoService(
        IOperationsService operationsService)
    {
        return new VideoService(operationsService);
    }
}