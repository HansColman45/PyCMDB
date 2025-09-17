package be.hans.cmdb.Actors;

import be.brightest.ScreenPlay.Actors.Actor;
import be.hans.cmdb.Questions.OpenDbConnection;
import be.hans.cmdb.Questions.Identity.OpenTheIdentityOverviewPage;
import be.hans.cmdb.Questions.OpenTheLoginPage;
import be.hans.cmdb.Questions.OpenTheMainPage;
import be.cmdb.dao.AccountDAO;
import be.cmdb.helpers.AdminHelper;
import be.cmdb.model.Account;
import be.cmdb.model.Admin;
import be.cmdb.pages.Identity.IdentityOverviewPage;
import be.cmdb.pages.LoginPage;
import be.cmdb.pages.MainPage;
import org.hibernate.Session;

/**
 * CMDB Actor with specific abilities.
 */
public class CMDBActor extends Actor {
    private Admin admin;
    private final Session session;
    private String expectedLogLine = "";

    /**
     * Constructor for CMDBActor.
     * @param name the name of the actor
     */
    public CMDBActor(String name) {
        super(name);
        session = asksFor(new OpenDbConnection());
    }

    /**
     * Get the current hibernate session.
     * @return Session
     */
    public Session getSession() {
        return session;
    }

    /**
     * Perform login action.
     * @param username the username
     * @param password the password
     * @return MainPage
     */
    public MainPage doLogin(String username, String password) {
        LoginPage loginPage = asksFor(new OpenTheLoginPage());
        loginPage.setUserId(username);
        loginPage.setPassword(password);
        return asksFor(new OpenTheMainPage());
    }

    /**
     * Create a new admin in the CMDB.
     * @param session the current hibernate session
     * @return Admin the created admin
     */
    public Admin createNewAdmin(Session session) {
        admin = AdminHelper.createNewCMDBAdmin(session);
        return admin;
    }

    /**
     * Open the identity overview page.
     * @return IdentityOverviewPage
     */
    public IdentityOverviewPage openIdentityOverviewPage() {
        IdentityOverviewPage identityOverviewPage = asksFor(new OpenTheIdentityOverviewPage());
        addAbility(identityOverviewPage);
        return identityOverviewPage;
    }

    /**
     * Get the Account related to the admin.
     * @return Account the account related to the admin
     */
    public Account getAccount() {
        AccountDAO accountDAO = new AccountDAO();
        return accountDAO.findById(session, admin.getAccountId());
    }

    /**
     * Delete all objects created by the admin.
     * @param session the current hibernate session
     */
    public void deleteAllObjectsCreatedByAdmin(Session session) {
        AdminHelper.deleteAllObjectsCreatedByAdmin(session, admin);
    }

    /**
     * Get the expected log line.
     * @return String the expected log line
     */
    public String getExpectedLogLine(){
        return expectedLogLine;
    }

    /**
     * Set the expected log line.
     * @param logLine the expected log line
     */
    protected void setExpectedLogLine(String logLine){
        this.expectedLogLine = logLine;
    }
}
