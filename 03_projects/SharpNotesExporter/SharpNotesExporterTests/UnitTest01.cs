namespace SharpNotesExporterTests
{
    using SharpNotesExporterTests.Repetition;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpTinderComplexTests;
    using GoogleDriveCoreApp.Contract;
    using SharpConfigProg.Service;
    using Unity;
    using SharpConfigProg.Preparer;

    [TestClass]
    public class UnitTest01 : UnitTest01Base
    {
        private readonly IGoogleDriveService driveService;
        private IConfigService configService;

        public UnitTest01()
        {
            driveService = Border.NewGoogleDriveService();
        }

        [TestMethod]
        public void PrepareConfig()
        {
            configService = MyBorder.Container.Resolve<IConfigService>();
            configService.Prepare(typeof(IPreparer.INotesSystem));
        }

        //https://developers.google.com/docs/api/concepts/structure


        //[DataRow("8ce8792f-dc83-4978-a1d8-1a49c71937ec", "Sprawy", "08/02/01")] //zrobić-dla-siebie
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/07/02")] //cele
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/07/05/02")] //cele
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/01/05/07/10")] //artykuły
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/06/09/25")] //23-01-27_r1tf_Albina_Schevchenko
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "02/02/04")] //rady-do_budowanie-luźnych-relacji; dorota-fluder
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "02/02/20")] //rady-do_budowanie-luźnych-relacji; patryk-karpiński
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "02/02/17")] //rady-do_budowanie-luźnych-relacji; kacper-żmigrodzki
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/06/11/01")] //23-02-03_todd-infield-uncut
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "02/02/19")] //rady-do_budowanie-luźnych-relacji; łukasz-kocoń
        //[DataRow("8ce8792f-dc83-4978-a1d8-1a49c71937ec", "Sprawy", "12/02/23")] //procedura-wstawanie
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/06/09/28/26")] //23-02-09_tf_katarzyna_siewniak
        //[DataRow("8ce8792f-dc83-4978-a1d8-1a49c71937ec", "Sprawy", "05/16/12", true)] //procedura-wstawanie
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/01/09/02/04", true)] //zadania; mówienie-do-ściany-randka
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/01/09/02/05", true)] //zadania; filmik-jak-być-ciekawym-człowiekiem
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/01/09/02/01", false)] //zadania; 10-filmów
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/01/09/02/06", false)] //zadania; czy nie lepiej iść do pracy
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/01/09/01", false)] //zadania; czy nie lepiej iść do pracy
        //[DataRow("8ce8792f-dc83-4978-a1d8-1a49c71937ec", "Sprawy", "12/01/04", false)] //8ce8792f-dc83-4978-a1d8-1a49c71937ec/Sprawy/12/01/04/
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/06/09/28/30", false)] //
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/06/09/28/31", false)] //
        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/06/09/28/29", false)] //
        //[DataRow("8ce8792f-dc83-4978-a1d8-1a49c71937ec", "Sprawy", "08/03/06/01/01", false)] //

        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/01/09/02/14", false)] //

        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/01/09/02/15", false)] //

        //[DataRow("8ce8792f-dc83-4978-a1d8-1a49c71937ec", "Sprawy", "05/02", false)] //

        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/06/09/28/32", false)] //

        //[DataRow("8ce8792f-dc83-4978-a1d8-1a49c71937ec", "Sprawy", "12/02/24", false)] //

        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/01/09/02/03/04/20", false)] //

        //[DataRow("8ce8792f-dc83-4978-a1d8-1a49c71937ec", "Sprawy", "08/02/06/03/01", false)] //

        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/03/22/09/04", false)] //

        //[DataRow("8ce8792f-dc83-4978-a1d8-1a49c71937ec", "Sprawy", "08/02/06/03/02", true)] //

        //[DataRow("ebf8d4ba-06c2-43eb-a201-4d32d13656e4", "Rama", "03/03/22/09/05", false)] //

        //Sprawy; 08/03/06/01/01; artykuły-kupić; szablon; 23-04-07
        //[DataRow("Sprawy", "08/03/06/01/01", true)]

        //Sprawy; 08/03/06/01/01; artykuły-kupić; szablon; 23-04-07
        //[DataRow("Sprawy", "08/02/07", false)]

        //Sprawy; 08/03/06/01/01; artykuły-kupić; szablon; 23-04-07
        //[DataRow("SprawyDorota", "02/04/06", false)]

        //Sprawy; 08/02/06/01/02; planowanie-dnia; templates; podstawowy
        //[DataRow("Sprawy", "08/02/06/01/02", true)]

        //Sprawy; 08/02/06/01/03; planowanie-dnia; templates; pusty
        //[DataRow("Sprawy", "08/02/06/01/03", true)]

        //Sprawy/08/03/06/01/02; artykuły-kupić; szablon; 23-05-17
        //[DataRow("Sprawy", "08/03/06/01/02", true)]

        //Sprawy/08/03/06/01/02; artykuły-kupić; szablon; 23-05-17
        [DataRow("Notki", "12/02/04/22/01/", false)]

        [TestMethod]
        public void TestMethod2(string repo, string loca, bool createTwoColumns = false)
        {
            // assert
            var inputName = repoService.Methods.GetLocalName((repo, loca));

            // act
            (var id, var outputName) = CreateNewDocFile(inputName);
            notesExporterService.ExportNotesToGoogleDoc(repo, loca, id, createTwoColumns);

            // assert
            Assert.AreEqual(outputName, inputName);
        }

        [TestMethod]
        public void TestMethod1()
        {
            //assert
            //var repo = (Guid.Parse("ebf8d4ba-06c2-43eb-a201-4d32d13656e4"), );
            var repo = "Rama";

            var problemsSection = "03/07/05/02";
            //var problemsDocId = "";

            var celeSection = "03/07/02";
            //var celeDocId = "";

            var kacperZmigrodzkiSection = "02/02/17";
            var kacperZmigrodzkiDocId = "12d633lcLJaXrVjIWShBt1MVeI7TVc7vYNnNw44lhzs4";

            var kamilKuchnickiSection = "02/02/18";
            var kamilKuchnickiDocId = "1EIqJqbH5UrBX_Et2-yF7xULmOJEhncoCRoASJlTREY8";

            var section = kamilKuchnickiSection;
            var docId = kamilKuchnickiDocId;

            //act
            notesExporterService.ExportNotesToGoogleDoc(repo, section, docId);

            Console.WriteLine("Finish");
        }

        private (string id, string name) CreateNewDocFile(string fileName)
        {
            var folder2023 = "13gY7OdaPCMwHQKmJZWZpcou7xtMrxNlg";
            var result = driveService.Worker.CreateNewDocFile(folder2023, fileName);
            return result;
        }
    }
}