using System.Data;
using CustomerManager;
using dal;
using Moq;
using Xunit;

namespace AtmTests;

public class CustomerTests
{

    private static DataTable addToTestTable(DataTable dt, int num, string holder, double balance, bool isActive, string login, int pin)
    {
        dt.Rows.Add(num, holder, balance, login, pin, isActive);
        return dt;
    }

    private static DataTable testTable() {
        DataTable dt = new DataTable();

        dt.Columns.Add("accountNum", typeof(int));
        dt.Columns.Add("holder",     typeof(string));
        dt.Columns.Add("balance",    typeof(double));
        dt.Columns.Add("login",      typeof(string));
        dt.Columns.Add("pin",        typeof(int));
        dt.Columns.Add("isActive",   typeof(bool));

        return dt;
    }

    [Fact]
    public async Task Customer_GetInfo() {
        var dt = testTable();
        dt = addToTestTable(dt,2,"Alex",100.62,true,"Xand20",56193);

        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.searchID(2)).Returns(dt);

        var cm = new customerManager(2, mockDal.Object);

        mockDal.Verify(d => d.searchID(2), Times.Once);
    }

    [Fact]
    public void Customer_Withdraw_Success() {
        int accNum = 2;
        double curr = 50.62;

        var dt = testTable();
        dt = addToTestTable(dt,2,"Alex",100.62,true,"Xand20",56193);

        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.searchID(accNum))
               .Returns(dt);
        mockDal.Setup(d => d.updateAccountBalance(accNum, It.Is<double>(b => Math.Abs(b - curr) < 0.001)))
               .Returns(1);
        mockDal.Setup(d => d.getAccountBalance(accNum))
               .Returns(curr);

        double withdrawAmount = 50;
        var cm = new customerManager(2, mockDal.Object);
        cm.withdraw(withdrawAmount.ToString());

        mockDal.Verify(d => d.updateAccountBalance(accNum, It.Is<double>(b => Math.Abs(b - curr) < 0.001)), Times.Once);
        mockDal.Verify(d => d.getAccountBalance(accNum), Times.Once);
    }

    [Fact]
    public void Customer_Withdraw_NaN() {
        int accNum = 2;

        var dt = testTable();
        dt = addToTestTable(dt, accNum, "Alex", 100.62, true, "Xand20", 56193);

        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.searchID(accNum)).Returns(dt);

        var cm = new customerManager(accNum, mockDal.Object);
        Assert.Throws<FormatException>(() => cm.withdraw("ABCDE"));

        mockDal.Verify(d => d.updateAccountBalance(It.IsAny<int>(), It.IsAny<double>()), Times.Never);
        mockDal.Verify(d => d.getAccountBalance(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Customer_Withdraw_Insufficient() {
        int accNum = 2;
        double balance = 100.62;

        var dt = testTable();
        dt = addToTestTable(dt, accNum, "Alex", balance, true, "Xand20", 56193);

        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.searchID(accNum)).Returns(dt);

        var cm = new customerManager(accNum, mockDal.Object);
        
        Assert.False(cm.withdraw("200"));

        mockDal.Verify(d => d.updateAccountBalance(It.IsAny<int>(), It.IsAny<double>()), Times.Never);
        mockDal.Verify(d => d.getAccountBalance(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Customer_Deposit_Success() {
        int accNum = 2;
        double curr = 150.62;

        var dt = testTable();
        dt = addToTestTable(dt,2,"Alex",100.62,true,"Xand20",56193);

        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.searchID(accNum))
               .Returns(dt);
        mockDal.Setup(d => d.updateAccountBalance(accNum, It.Is<double>(b => Math.Abs(b - curr) < 0.001)))
               .Returns(1);
        mockDal.Setup(d => d.getAccountBalance(accNum))
               .Returns(curr);

        double depositAmount = 50;
        var cm = new customerManager(2, mockDal.Object);
        cm.deposit(depositAmount.ToString());

        mockDal.Verify(d => d.updateAccountBalance(accNum, It.Is<double>(b => Math.Abs(b - curr) < 0.001)), Times.Once);
        mockDal.Verify(d => d.getAccountBalance(accNum), Times.Once);
    }

    [Fact]
    public void Customer_Deposit_NaN() {
        int accNum = 2;

        var dt = testTable();
        dt = addToTestTable(dt, accNum, "Alex", 100.62, true, "Xand20", 56193);

        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.searchID(accNum)).Returns(dt);

        var cm = new customerManager(accNum, mockDal.Object);
        Assert.Throws<FormatException>(() => cm.deposit("ABCDE"));

        mockDal.Verify(d => d.updateAccountBalance(It.IsAny<int>(), It.IsAny<double>()), Times.Never);
        mockDal.Verify(d => d.getAccountBalance(It.IsAny<int>()), Times.Never);
    }
}
