namespace E_Games.Tests
{
    public class UnitTest
    {
        [Fact]
        public void AddTwoNumbersReturnsCorrectSum()
        {
            // Arrange
            int x = 5;
            int y = 10;
            int expectedSum = 15;

            // Act
            int result = Add(x, y);

            // Assert
            Assert.Equal(expectedSum, result);
        }

        private int Add(int x, int y) => x + y;
    }
}