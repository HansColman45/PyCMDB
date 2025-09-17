package be.hans.cmdb.Actors;

import be.cmdb.dao.ApplicationDAO;
import be.cmdb.helpers.AccountHelper;
import be.cmdb.helpers.LogLineHelper;
import be.cmdb.model.Account;
import be.cmdb.model.Admin;
import be.cmdb.model.Application;
import be.cmdb.pages.Account.CreateAccountPage;
import be.hans.cmdb.Questions.Account.OpenTheCreateAccountPage;

public class AccountActor extends CMDBActor{
    private final String table = "account";
    /**
     * Constructor for CMDBActor.
     * @param name the name of the actor
     */
    public AccountActor(String name) {
        super(name);
    }

    /**
     * Create a random account
     * @return the created Account entity
     */
    public Account createAccount(){
        return AccountHelper.createRandomAccount(getSession());
    }

    /**
     * Create a simple account with specified admin
     * @param admin the Admin who is creating the account
     * @param active boolean indicating if the account is active
     * @return the created Account entity
     */
    public Account createAccount(Admin admin, boolean active){
        return AccountHelper.createSimpleAccount(getSession(), admin, active);
    }

    /**
     * automated creation of an account
     */
    public void doCreateAccount(Account account){
        Application app = new ApplicationDAO().findById(getSession(),account.getApplicationId());
        CreateAccountPage createPage = asksFor(new OpenTheCreateAccountPage());
        createPage.setUserId(account.getUserId());
        createPage.selectType(String.valueOf(account.getType().getTypeId()));
        createPage.selectApplication(String.valueOf(account.getApplicationId()));
        createPage.clickCreateButton();
        String value = "Account with UserID: "+ account.getUserId()+" and type "+ account.getType().getType() +" for application "+app.getName();
        setExpectedLogLine(LogLineHelper.createLogLine(value, getAccount().getUserId(), table));
    }
}
