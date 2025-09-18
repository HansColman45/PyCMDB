package be.hans.cmdb;

import be.cmdb.model.Account;
import be.cmdb.pages.Account.AccountDetailsPage;
import be.cmdb.pages.Account.AccountOverviewPage;
import be.hans.cmdb.Actors.AccountActor;
import be.hans.cmdb.Questions.Account.OpenTheAccountDetailPage;
import be.hans.cmdb.Questions.Account.OpenTheAccountOverviewPage;
import org.junit.jupiter.api.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class AccountTest extends BaseTest{

    @Test
    void canCreateNewAccount(){
        AccountActor accountActor = new AccountActor("AccountCreator");
        registerActor(accountActor);
        accountActor.createNewAdmin();
        accountActor.doLogin(accountActor.getAccount().getUserId(), defaultPassword());
        AccountOverviewPage overviewPage = accountActor.asksFor(new OpenTheAccountOverviewPage());
        Account newaccount = accountActor.createAccount();
        accountActor.doCreateAccount(newaccount);
        //Verify that the account was created successfully
        overviewPage.search(newaccount.getUserId());
        AccountDetailsPage detailsPage = accountActor.asksFor(new OpenTheAccountDetailPage());
        String logline = detailsPage.getLastLogline();
        assertThat(logline).isNotEmpty()
            .isEqualToIgnoringCase(accountActor.getExpectedLogLine());
    }
}
