using Moq;
using System.Collections.Generic;
using System.Linq;
using TEntS.Types.Materials;

namespace TEntS.ClientBLL.Tests
{
    public class Class1
    {
        public void RetrieveActiveMaterialsByIdTest()
        {
            //Arrange
            var sut = new Mock<IMaterial>();
            var materialList = new List<Material>
            {
                new Material { Code="M1-001", Description="Anchor block, concrete", Cost = new Costing {  UnitPrice = 1} }
                ,new Material { Code="M1-002", Description="Anchor log, concrete", Cost = new Costing {  UnitPrice = 1.50} }
                ,new Material { Code="M1-003", Description="Armor rod, preformed, double support, #1/0 AWG", Cost = new Costing {  UnitPrice = 2}, }
            };

            sut.Setup(m => m.RetrieveActiveMaterialsById(It.IsAny<int>())).Returns(materialList.FirstOrDefault);

            var result = sut.Object.RetrieveActiveMaterialsById("M1-001");


        }
    }
}
