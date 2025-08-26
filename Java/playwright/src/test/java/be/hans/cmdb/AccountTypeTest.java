package be.hans.cmdb;

import be.cmdb.dao.AccountDAO;
import be.cmdb.helpers.AccountTypeHelper;
import be.cmdb.model.Account;
import be.cmdb.model.Type;
import be.cmdb.pages.LoginPage;
import be.cmdb.pages.MainPage;
import be.cmdb.pages.Type.CreateTypePage;
import be.cmdb.pages.Type.TypeDetailPage;
import be.cmdb.pages.Type.TypeOverviewPage;
import org.junit.jupiter.api.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class AccountTypeTest extends BaseTest{

    @Test
    void canCreateNewAccountType() {
        Type type = AccountTypeHelper.createRandomAccountType();
        LoginPage loginPage = new LoginPage(getPage());
        loginPage.navigate();
        AccountDAO accountDAO = new AccountDAO();
        Account account = accountDAO.findById(getSession(), getAdmin().getAccountId());
        MainPage mainPage = loginPage.login(account.getUserId(), defaultPassword());
        boolean isLoggedIn = loginPage.isUserLogedIn();
        assertThat(isLoggedIn).isTrue();
        TypeOverviewPage typeOverviewPage = mainPage.openAccountTypeOverview();
        CreateTypePage createTypePage = typeOverviewPage.openCreateTypePage();
        createTypePage.setType(type.getType());
        createTypePage.setDescription(type.getDescription());
        createTypePage.clickCreateButton();
        // Verify that the type was created successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeOverviewPage.openTypeDetailPage();
        String logline = detailPage.getLastLogline("accounttype");
        assertThat(logline).contains("table accounttype")
            .contains(type.getType())
            .contains(type.getDescription())
            .contains("by "+account.getUserId());
    }
}
