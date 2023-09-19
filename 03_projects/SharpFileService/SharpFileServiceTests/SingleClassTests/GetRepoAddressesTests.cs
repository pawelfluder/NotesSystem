using SharpFileServiceProg.Service;
using SharpRepoBackendProg.Repetition;
using SharpRepoServiceProg.FileOperations;
using SharpRepoServiceProg.Service;
using Unity;

namespace SharpFileServiceTests.SingleClassTests
{
    [TestClass]
    public class GetRepoAddressesTests
    {
        private readonly IFileService fileService;
        private readonly IRepoService repoService;
        private readonly IRepoAddressesObtainer getRepoAddresses;

        public GetRepoAddressesTests()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            repoService = MyBorder.Container.Resolve<IRepoService>();
            getRepoAddresses = fileService.File.NewRepoAddressesObtainer();
        }

        [TestMethod]
        public void Method01()
        {
            // arrange
            var repo = "Notki";
            var loca = "01";
            var path = repoService.Methods.GetElemPath((repo, loca));

            // act
            var addressList = getRepoAddresses.Visit(path);
        }
    }
}
