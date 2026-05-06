using System.Data;
using AdminManager;
using dal;
using Moq;
using Xunit;

namespace AtmTests;

public class AdminTests {
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
    public void Admin_CreateAccount() {
        var mockDal = new Mock<IDal>();

        mockDal.Setup(d => d.createAccount("Mario", "Mario", 11111, 100.0, true))
               .Returns(8);

        var admin = new adminManager(mockDal.Object);

        admin.createNewAccount("Mario", "Mario", 11111, 100.0, "y");

        mockDal.Verify(d =>
            d.createAccount("Mario", "Mario", 11111, 100.0, true),
            Times.Once);
    }

    [Fact]
    public void Admin_Delete_Success() {
        var dt = testTable();
        dt = addToTestTable(dt,2,"Alex",100.62,true,"Xand20",56193);

        var mockDal = new Mock<IDal>();

        mockDal.Setup(d => d.deleteAccount(2)).Returns(1);
        mockDal.Setup(d => d.searchID(2)).Returns(dt);

        var admin = new adminManager(mockDal.Object);

        var result = admin.deleteAccount(2, 2);

        Assert.True(result);
        mockDal.Verify(d => d.deleteAccount(2), Times.Once);
    }

    [Fact]
    public void Admin_Delete_NoMatch() {
        var mockDal = new Mock<IDal>();
        var admin = new adminManager(mockDal.Object);
        var result = admin.deleteAccount(2, 1);

        Assert.False(result);
        mockDal.Verify(d => d.deleteAccount(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Admin_Delete_NotFound() {
        int id = 500;
        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.deleteAccount(id)).Returns(0);

        var admin = new adminManager(mockDal.Object);
        var result = admin.deleteAccount(id, id);

        Assert.False(result);
        mockDal.Verify(d => d.deleteAccount(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void Admin_Search_Success() {
        var id = 2;
        var dt = testTable();
        dt = addToTestTable(dt,id,"Alex",100.62,true,"Xand20",56193);

        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.searchID(id)).Returns(dt);

        var admin = new adminManager(mockDal.Object);
        var result = admin.searchForAccount(id);

        Assert.True(result);
        mockDal.Verify(d => d.searchID(id),Times.Once);
    }

    [Fact]
    public void Admin_Search_NotFound() {
        var dt = testTable();
        var id = 500;

        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.searchID(id)).Returns(dt);

        var admin = new adminManager(mockDal.Object);
        var result = admin.searchForAccount(id);

        Assert.False(result);
        mockDal.Verify(d => d.searchID(id),Times.Once);
    }

    /*
        This part is kinda big, so I'm separating it from the other admin tests.
    */

    //Updates -- Holder
    [Fact]
    public void Admin_UpdateHolder_Success() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;

        var dt = testTable();
        dt = addToTestTable(dt,id,"hold",balance,status,login,pin);

        var mockDal = new Mock<IDal>();

        mockDal.Setup(d => d.updateAccount(id, "Alexander", login, pin, status)).Returns(1);

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateHolder(id,"Alexander",login,pin,status);

        Assert.True(result);
        mockDal.Verify(d => d.updateAccount(id, "Alexander", login, pin, status), Times.Once);
    }

    [Fact]
    public void Admin_UpdateHolder_Invalid() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;

        var mockDal = new Mock<IDal>();

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateHolder(id,null!,login,pin,status);

        Assert.False(result);
        mockDal.Verify(d => d.updateAccount(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public void Admin_UpdateHolder_NotFound() {
        var id = 999; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.updateAccount(id, hold, login, pin, status)).Returns(0);

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateHolder(id,hold,login,pin,status);
        Assert.False(result);
    }

    //Update Login
    [Fact]
    public void Admin_UpdateLogin_Success() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;

        var mockDal = new Mock<IDal>();

        mockDal.Setup(d => d.updateAccount(id, hold, "Xand24", pin, status)).Returns(1);

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateLogin(id,hold,"Xand24",pin,status);

        Assert.True(result);
    }

    [Fact]
    public void Admin_UpdateLogin_Invalid() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;

        var mockDal = new Mock<IDal>();

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateLogin(id,hold,null!,pin,status);

        Assert.False(result);
        mockDal.Verify(d => d.updateAccount(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public void Admin_UpdateLogin_NotFound() {
        var id = 999; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.updateAccount(id, hold, login, pin, status)).Returns(0);

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateHolder(id,hold,"Xand24",pin,status);
        Assert.False(result);
    }

    //Update PIN
    [Fact]
    public void Admin_UpdatePin_Success() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        var newVal = 10101;

        var mockDal = new Mock<IDal>();

        mockDal.Setup(d => d.updateAccount(id, hold, login, newVal, status)).Returns(1);

        var admin = new adminManager(mockDal.Object);
        var result = admin.updatePIN(id,hold,login,newVal,status);

        Assert.True(result);
    }

    [Fact]
    public void Admin_UpdatePin_Invalid_TooLarge() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        var newVal = 10101;

        var mockDal = new Mock<IDal>();

        var admin = new adminManager(mockDal.Object);
        var result = admin.updatePIN(id,hold,login,100000,status);

        Assert.False(result);
        mockDal.Verify(d => d.updateAccount(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public void Admin_UpdatePin_Invalid_Negative() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        var newVal = 10101;

        var mockDal = new Mock<IDal>();

        var admin = new adminManager(mockDal.Object);
        var result = admin.updatePIN(id,hold,login,-12345,status);

        Assert.False(result);
        mockDal.Verify(d => d.updateAccount(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public void Admin_UpdatePin_NotFound() {
        var id = 999; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        var newVal = 10101;
        
        var mockDal = new Mock<IDal>();
        mockDal.Setup(d => d.updateAccount(id, hold, login, pin, status)).Returns(0);

        var admin = new adminManager(mockDal.Object);
        var result = admin.updatePIN(id,hold,"Xand24",pin,status);
        Assert.False(result);
    }

    //Update Status
    [Fact]
    public void Admin_UpdateStatus_Success_Yes() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        var newVal = "y";

        var mockDal = new Mock<IDal>();

        mockDal.Setup(d => d.updateAccount(id, hold, login, pin, true)).Returns(1);

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateStatus(id,hold,login,pin,status,newVal);

        Assert.True(result);
    }

    [Fact]
    public void Admin_UpdateStatus_Success_No() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        var newVal = "n";

        var mockDal = new Mock<IDal>();

        mockDal.Setup(d => d.updateAccount(id, hold, login, pin, false)).Returns(1);

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateStatus(id,hold,login,pin,status,newVal);

        Assert.True(result);
    }

    [Fact]
    public void Admin_UpdateStatus_Invalid_Null() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        string newVal = null;

        var mockDal = new Mock<IDal>();

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateStatus(id,hold,login,pin,status,newVal);

        Assert.False(result);
        mockDal.Verify(d => d.updateAccount(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public void Admin_UpdateStatus_Invalid_NotYN() {
        var id = 2; var hold = "Alex"; var balance = 100.62; var status = true; var login="Xand20"; var pin = 56193;
        var newVal = "a";

        var mockDal = new Mock<IDal>();

        var admin = new adminManager(mockDal.Object);
        var result = admin.updateStatus(id,hold,login,pin,status,null!);

        Assert.False(result);
        mockDal.Verify(d => d.updateAccount(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
    }

    
}