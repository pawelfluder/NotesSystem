using System.Collections.Generic;

namespace SharpRepoServiceProg.Operations.Files;

public interface IRepoAddressesObtainer
{
    List<string> Visit(string path);
}