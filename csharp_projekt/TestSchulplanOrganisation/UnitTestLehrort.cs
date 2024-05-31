using SchulplanOrganisation.Daten;
using System.Xml.Linq;

namespace TestSchulplanOrganisation
{
    [Ignore]
    [TestClass]
    public class UnitTestLehrort
    {
        [TestMethod]
        public void TestCreateRaeume()
        {
            Lehrort G1S2R1234 = Lehrort.CreateLehrort(
                idGebaeude: Gebaeude.GebaeudeEins,
                idStockWerk: 2,
                idRaumNummer: 1234,
                name: "Chemie Gustav"
            );

            Lehrort aula = Lehrort.CreateLehrort(
                idGebaeude: Gebaeude.GebaeudeEins,
                idStockWerk: 0,
                idName: "aula-gross",
                name: "Große Aula"
            );

            Lehrort bibliothek = Lehrort.CreateLehrort(
                idGebaeude: Gebaeude.GebaeudeEins,
                idStockWerk: 0,
                idName: "bibliothek",
                name: "Bibliothek"
            );

            Lehrort sporthallePaula = Lehrort.CreateLehrort(
                idGebaeude: Gebaeude.Sporthalle,
                idName: "sporthalle-paula",
                name: "Sporthalle Paula"
            );

            Assert.AreEqual("Lehrort[room.g1-s2-r1234] => Chemie Gustav", $"{G1S2R1234}");
            Assert.AreEqual("Lehrort[name.g1-s0-aula-gross] => Große Aula", $"{aula}");
            Assert.AreEqual("Lehrort[name.g1-s0-bibliothek] => Bibliothek", $"{bibliothek}");
            Assert.AreEqual("Lehrort[name.Sporthalle-S0-sporthalle-paula] => Sporthalle Paula", $"{sporthallePaula}");
        }
    }
}