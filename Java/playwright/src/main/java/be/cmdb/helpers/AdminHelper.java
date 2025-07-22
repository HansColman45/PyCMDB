package be.cmdb.helpers;

import java.util.List;

import org.hibernate.Session;
import org.hibernate.Transaction;

import be.cmdb.builders.AccountBuilder;
import be.cmdb.builders.AdminBuilder;
import be.cmdb.builders.IdentityBuilder;
import be.cmdb.builders.LogBuilder;
import be.cmdb.dao.AccountDAO;
import be.cmdb.dao.AccountTypeDAO;
import be.cmdb.dao.AdminDAO;
import be.cmdb.dao.ApplicationDAO;
import be.cmdb.dao.IdentityDAO;
import be.cmdb.dao.IdentityTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Account;
import be.cmdb.model.Admin;
import be.cmdb.model.Application;
import be.cmdb.model.Identity;
import be.cmdb.model.Log;
import be.cmdb.model.Type;

/**
 * Helper class for Admin operations in the CMDB application.
 * Provides methods to create and manage Admin entities.
 */
public final class AdminHelper {

    private AdminHelper() {
    }

    /**
    * Creates a new Admin that will be used for the test.
    * @param session the Hibernate session
    * @return a new Admin entity persisted in the database
    */
    public static Admin createNewCMDBAdmin(Session session) {
        Transaction transaction = session.beginTransaction();

        // Get the needed types from the database
        IdentityTypeDAO identityTypeDAO = new IdentityTypeDAO();
        Type identityType = identityTypeDAO.findByType(session, "Werknemer");
        AccountTypeDAO accountTypeDAO = new AccountTypeDAO();
        Type accountType = accountTypeDAO.findByType(session, "Administrator");
        ApplicationDAO applicationDAO = new ApplicationDAO();
        Application application = applicationDAO.findByName(session, "CMDB").get(0);

        //First create the account
        Account account = new AccountBuilder()
            .withLastModifiedAdminId(1)
            .withTypeId(accountType.getTypeId())
            .withApplicationId(application.getAppId())
            .build();
        AccountDAO accountDAO = new AccountDAO();
        account = accountDAO.save(session, account);

        Log accountLog = new LogBuilder()
            .withAccount(account)
            .withLogText("The Account with UserID: " + account.getUserId()
                +" for application " + application.getName() +
                " is created by Automation in table account")
            .build();
        LogDAO logDAO = new LogDAO();
        logDAO.save(session, accountLog);

        // Now create the admin
        Admin admin = new AdminBuilder()
            .withLastModifiedAdminId(1)
            .withLevel(9)
            .build();
        AdminDAO adminDAO = new AdminDAO();
        admin = adminDAO.save(session, admin);

        Log adminLog = new LogBuilder()
            .withAdmin(admin)
            .withLogText("The Admin with UserId: " + account.getUserId()
                + " is created by Automation in table admin")
            .build();
        logDAO.save(session, adminLog);

        //Create the identity for the account
        Identity identity = new IdentityBuilder()
            .withTypeId(identityType.getTypeId())
            .withUserId(account.getUserId())
            .build();
        IdentityDAO identityDAO = new IdentityDAO();
        identity = identityDAO.save(session, identity);

        Log identityLog = new LogBuilder()
            .withIdentity(identity)
            .withLogText("The Identity with UserID: " + account.getUserId()
                + " is created by Automation in table identity")
            .build();
        logDAO.save(session, identityLog);

        transaction.commit();
        return admin;
    }

    public static Admin createSimpleAdmin(Session session, Account account, Admin admin, int level, boolean active) {
        Transaction transaction = session.beginTransaction();
        Admin newAdmin = new AdminBuilder()
            .withAccountId(account.getAccId())
            .withLastModifiedAdminId(admin.getLastModifiedAdminId())
            .withLevel(level)
            .withActive(active ? 1 : 0)
            .build();
        AdminDAO adminDAO = new AdminDAO();
        newAdmin = adminDAO.save(session, newAdmin);

        Log log = new LogBuilder()
            .withAdmin(newAdmin)
            .withLogText("The Admin with AccountID: " + account.getAccId()
                + " and level " + level + " is created by Automation in table admin")
            .build();

        LogDAO logDAO = new LogDAO();
        logDAO.save(session, log);
        transaction.commit();
        return newAdmin;
    }

    public void deleteAllObjectsCreatedByAdmin(Session session, Admin admin) {
        Transaction transaction = session.beginTransaction();
        //Delete all Identities created by this admin
        IdentityDAO identityDAO = new IdentityDAO();
        List<Identity> idens = identityDAO.findByLastModifiedAdminId(session, admin.getAdminId());
        for( Identity identity : idens) {
            IdentityHelper.deleteIdentity(session, identity);
        }

        transaction.commit();
    }
}
