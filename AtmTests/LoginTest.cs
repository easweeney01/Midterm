using System.Data;
using login;
using dal;
using Moq;
using Xunit;

namespace AtmTests;

public class LoginTest {
	private static DataTable addToTestTable(DataTable dt, int num, string holder, double balance, bool isActive, string login, int pin) {
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
	public static void testLogin_Success() {
		var mockDal = new Mock<IDal>();

        mockDal.Setup(d => d.login("Xand20",56193)).Returns(2);

        var lm = new LoginManager(mockDal.Object);

        var result = lm.tryLogin("Xand20","56193");

        Assert.True(result == 2);
        mockDal.Verify(d => d.login("Xand20",56193), Times.Once);
	}

	[Fact]
	public static void testLogin_Invalid() {
		var mockDal = new Mock<IDal>();

        mockDal.Setup(d => d.login("Xand20",99999)).Returns(-1);

        var lm = new LoginManager(mockDal.Object);

        var result = lm.tryLogin("Xand20","99999");

        Assert.False(result != -1);
        mockDal.Verify(d => d.login("Xand20",99999), Times.Once);
	}
}
