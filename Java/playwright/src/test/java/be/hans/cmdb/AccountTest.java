package be.hans.cmdb;

import be.cmdb.helpers.AccountHelper;
import be.cmdb.model.Account;
import be.cmdb.pages.Account.AccountOverviewPage;
import be.cmdb.pages.Account.CreateAccountPage;
import be.cmdb.pages.MainPage;
import org.junit.jupiter.api.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class AccountTest extends BaseTest{

    @Test
    void canCreateNewAccount(){
        Account newaccount = AccountHelper.createRandomAccount();
        MainPage mainPage = doLogin();
        AccountOverviewPage overviewPage = mainPage.openAccountOverview();
        CreateAccountPage createAccountPage = overviewPage.openCreateAccount();

    }
}
