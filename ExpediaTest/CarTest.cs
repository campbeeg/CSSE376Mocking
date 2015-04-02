using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestClass]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[TestInitialize]
		public void TestInitialize()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[TestMethod]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[TestMethod]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[TestMethod]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[TestMethod]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [TestMethod()]
        public void TestThatCarDoesGetCarLocationFromTheDatabase()
        {
            IDatabase mockDB = mocks.DynamicMock<IDatabase>();

            String carLocation = "Hotel";
            String anotherCarLocation = "Parking Garage";

            using (mocks.Ordered())
            {
                Expect.Call(mockDB.getCarLocation(1025)).Return(anotherCarLocation);
                Expect.Call(mockDB.getCarLocation(24)).Return(carLocation);
            }

            mockDB.Stub(x => x.getCarLocation(Arg<int>.Is.Anything)).Return("No car");

            mocks.ReplayAll();

            Car target = new Car(10);
            target.Database = mockDB;
            String result;

            result = target.getCarLocation(1025);
            Assert.AreEqual(anotherCarLocation, result);

            result = target.getCarLocation(24);
            Assert.AreEqual(carLocation, result);

            result = target.getCarLocation(25);
            Assert.AreEqual("No car", result);

            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestThatCarDoesGetMileageFromDatabase()
        {
            IDatabase mockDatabase = mocks.StrictMock<IDatabase>();
            int Miles = 30;

            Expect.Call(mockDatabase.Miles).PropertyBehavior();

            mocks.ReplayAll();

            mockDatabase.Miles = Miles;

            var target = new Car(10);
            target.Database = mockDatabase;

            int mileage = target.Mileage;
            Assert.AreEqual(mileage, Miles);

            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestThatCarReturnsName()
        {
            String expectedName = "BMW M5 Coupe";
            var target = ObjectMother.BMW();
            String actualName = target.Name;
            Assert.AreEqual(expectedName, actualName);
        }
	}
}
